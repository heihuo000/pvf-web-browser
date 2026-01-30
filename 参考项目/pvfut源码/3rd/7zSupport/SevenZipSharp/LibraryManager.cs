/*  This file is part of SevenZipSharp.

    SevenZipSharp is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    SevenZipSharp is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with SevenZipSharp.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace SevenZipSharp
{
#if UNMANAGED
    /// <summary>
    /// 7-zip library low-level wrapper.
    /// </summary>
    internal static class SevenZipLibraryManager
    {
        /// <summary>
        /// Synchronization root for all locking.
        /// </summary>
        private static readonly object _syncRoot = new object();

        /// <summary>
        /// Path to the 7-zip dll.
        /// </summary>
        /// <remarks>7zxa.dll supports only decoding from .7z archives.
        /// Features of 7za.dll: 
        ///     - Supporting 7z format;
        ///     - Built encoders: LZMA, PPMD, BCJ, BCJ2, COPY, AES-256 Encryption.
        ///     - Built decoders: LZMA, PPMD, BCJ, BCJ2, COPY, AES-256 Encryption, BZip2, Deflate.
        /// 7z.dll (from the 7-zip distribution) supports every InArchiveFormat for encoding and decoding.
        /// </remarks>
        private static string _libraryFileName;

        /// <summary>
        /// 7-zip library handle.
        /// </summary>
        private static IntPtr _modulePtr;

        /// <summary>
        /// 7-zip library features.
        /// </summary>
        private static LibraryFeature? _features;

        private static Dictionary<object, Dictionary<InArchiveFormat, IInArchive>> _inArchives;
        private static Dictionary<object, Dictionary<OutArchiveFormat, IOutArchive>> _outArchives;
        private static int _totalUsers;

        // private static string _LibraryVersion;
        private static bool? _modifyCapabale;

        private static void InitUserInFormat(object user, InArchiveFormat format)
        {
            if (!_inArchives.ContainsKey(user))
            {
                _inArchives.Add(user, new Dictionary<InArchiveFormat, IInArchive>());
            }

            if (!_inArchives[user].ContainsKey(format))
            {
                _inArchives[user].Add(format, null);
                _totalUsers++;
            }
        }

        private static void InitUserOutFormat(object user, OutArchiveFormat format)
        {
            if (!_outArchives.ContainsKey(user))
            {
                _outArchives.Add(user, new Dictionary<OutArchiveFormat, IOutArchive>());
            }

            if (!_outArchives[user].ContainsKey(format))
            {
                _outArchives[user].Add(format, null);
                _totalUsers++;
            }
        }

        private static void Init()
        {
            _inArchives = new Dictionary<object, Dictionary<InArchiveFormat, IInArchive>>();
            _outArchives = new Dictionary<object, Dictionary<OutArchiveFormat, IOutArchive>>();
        }

        /// <summary>
        /// Loads the 7-zip library if necessary and adds user to the reference list
        /// </summary>
        /// <param name="user">Caller of the function</param>
        /// <param name="format">Archive format</param>
        public static void LoadLibrary(object user, Enum format)
        {
            lock (_syncRoot)
            {
                if (_inArchives == null || _outArchives == null)
                {
                    Init();
                }

                if (_modulePtr == IntPtr.Zero)
                {
                    var libraryFileName = GetLibraryPath();
                    if (!File.Exists(libraryFileName))
                    {
                        throw new SevenZipLibraryException("DLL file does not exist.");
                    }

                    if ((_modulePtr = NativeMethods.LoadLibrary(libraryFileName)) == IntPtr.Zero)
                    {
                        throw new SevenZipLibraryException("failed to load library.");
                    }

                    if (NativeMethods.GetProcAddress(_modulePtr, "GetHandlerProperty") == IntPtr.Zero)
                    {
                        NativeMethods.FreeLibrary(_modulePtr);
                        throw new SevenZipLibraryException("library is invalid.");
                    }
                }

                switch (format)
                {
                    case InArchiveFormat _:
                        InitUserInFormat(user, (InArchiveFormat) format);
                        return;
                    case OutArchiveFormat _:
                        InitUserOutFormat(user, (OutArchiveFormat) format);
                        return;
                }

                throw new ArgumentException("Enum " + format + " is not a valid archive format attribute!");
            }
        }

        /// <summary>
        /// Gets the value indicating whether the library supports modifying archives.
        /// </summary>
        public static bool ModifyCapable
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_modifyCapabale.HasValue) return _modifyCapabale.Value;
                    var dllVersionInfo = FileVersionInfo.GetVersionInfo(GetLibraryPath());
                    _modifyCapabale = dllVersionInfo.FileMajorPart >= 9;
                    return _modifyCapabale.Value;
                }
            }
        }

        static readonly string Namespace = Assembly.GetExecutingAssembly().GetManifestResourceNames()[0].Split('.')[0];

        private static string GetResourceString(string str)
        {
            return Namespace + ".arch." + str;
        }

        private static bool ExtractionBenchmark(string archiveFileName, Stream outStream, ref LibraryFeature? features,
            LibraryFeature testedFeature)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetResourceString(archiveFileName));
            try
            {
                using (var extr = new SevenZipExtractor(stream))
                {
                    extr.ExtractFile(0, outStream);
                }
            }
            catch (Exception)
            {
                return false;
            }

            features |= testedFeature;
            return true;
        }

        private static bool CompressionBenchmark(Stream inStream, Stream outStream, OutArchiveFormat format,
            CompressionMethod method, ref LibraryFeature? features, LibraryFeature testedFeature)
        {
            try
            {
                var compr = new SevenZipCompressor {ArchiveFormat = format, CompressionMethod = method};
                compr.CompressStream(inStream, outStream);
            }
            catch (Exception)
            {
                return false;
            }

            features |= testedFeature;
            return true;
        }

        public static LibraryFeature CurrentLibraryFeatures
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_features != null && _features.HasValue)
                    {
                        return _features.Value;
                    }

                    _features = LibraryFeature.None;

                    #region Benchmark

                    #region Extraction features

                    using (var outStream = new MemoryStream())
                    {
                        ExtractionBenchmark("Test.lzma.7z", outStream, ref _features, LibraryFeature.Extract7z);
                        ExtractionBenchmark("Test.lzma2.7z", outStream, ref _features, LibraryFeature.Extract7zLZMA2);
                        var i = 0;
                        if (ExtractionBenchmark("Test.bzip2.7z", outStream, ref _features, _features.Value))
                        {
                            i++;
                        }

                        if (ExtractionBenchmark("Test.ppmd.7z", outStream, ref _features, _features.Value))
                        {
                            i++;
                            if (i == 2 && (_features & LibraryFeature.Extract7z) != 0 &&
                                (_features & LibraryFeature.Extract7zLZMA2) != 0)
                            {
                                _features |= LibraryFeature.Extract7zAll;
                            }
                        }

                        ExtractionBenchmark("Test.rar", outStream, ref _features, LibraryFeature.ExtractRar);
                        ExtractionBenchmark("Test.tar", outStream, ref _features, LibraryFeature.ExtractTar);
                        ExtractionBenchmark("Test.txt.bz2", outStream, ref _features, LibraryFeature.ExtractBzip2);
                        ExtractionBenchmark("Test.txt.gz", outStream, ref _features, LibraryFeature.ExtractGzip);
                        //ExtractionBenchmark("Test.txt.xz", outStream, ref _features, LibraryFeature.ExtractXz);
                        ExtractionBenchmark("Test.zip", outStream, ref _features, LibraryFeature.ExtractZip);
                    }

                    #endregion

                    #region Compression features

                    using (var inStream = new MemoryStream())
                    {
                        inStream.Write(Encoding.UTF8.GetBytes("Test"), 0, 4);
                        using (var outStream = new MemoryStream())
                        {
                            CompressionBenchmark(inStream, outStream, OutArchiveFormat.SevenZip, CompressionMethod.Lzma,
                                ref _features, LibraryFeature.Compress7z);
                            CompressionBenchmark(inStream, outStream, OutArchiveFormat.SevenZip,
                                CompressionMethod.Lzma2, ref _features, LibraryFeature.Compress7zLZMA2);
                            var i = 0;
                            if (CompressionBenchmark(inStream, outStream, OutArchiveFormat.SevenZip,
                                CompressionMethod.BZip2, ref _features, _features.Value))
                            {
                                i++;
                            }

                            if (CompressionBenchmark(inStream, outStream, OutArchiveFormat.SevenZip,
                                CompressionMethod.Ppmd, ref _features, _features.Value))
                            {
                                i++;
                                if (i == 2 && (_features & LibraryFeature.Compress7z) != 0 &&
                                    (_features & LibraryFeature.Compress7zLZMA2) != 0)
                                {
                                    _features |= LibraryFeature.Compress7zAll;
                                }
                            }

                            CompressionBenchmark(inStream, outStream, OutArchiveFormat.Zip, CompressionMethod.Default,
                                ref _features, LibraryFeature.CompressZip);
                            CompressionBenchmark(inStream, outStream, OutArchiveFormat.BZip2, CompressionMethod.Default,
                                ref _features, LibraryFeature.CompressBzip2);
                            CompressionBenchmark(inStream, outStream, OutArchiveFormat.GZip, CompressionMethod.Default,
                                ref _features, LibraryFeature.CompressGzip);
                            CompressionBenchmark(inStream, outStream, OutArchiveFormat.Tar, CompressionMethod.Default,
                                ref _features, LibraryFeature.CompressTar);
                        }
                    }

                    #endregion

                    #endregion

                    if (ModifyCapable && (_features.Value & LibraryFeature.Compress7z) != 0)
                    {
                        _features |= LibraryFeature.Modify;
                    }

                    return _features.Value;
                }
            }
        }

        /// <summary>
        /// Removes user from reference list and frees the 7-zip library if it becomes empty
        /// </summary>
        /// <param name="user">Caller of the function</param>
        /// <param name="format">Archive format</param>
        public static void FreeLibrary(object user, Enum format)
        {
            var sp = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
            sp.Demand();
            lock (_syncRoot)
            {
                if (_modulePtr != IntPtr.Zero)
                {
                    if (format is InArchiveFormat)
                    {
                        if (_inArchives != null && _inArchives.ContainsKey(user) &&
                            _inArchives[user].ContainsKey((InArchiveFormat) format) &&
                            _inArchives[user][(InArchiveFormat) format] != null)
                        {
                            try
                            {
                                Marshal.ReleaseComObject(_inArchives[user][(InArchiveFormat) format]);
                            }
                            catch (InvalidComObjectException)
                            {
                            }

                            _inArchives[user].Remove((InArchiveFormat) format);
                            _totalUsers--;
                            if (_inArchives[user].Count == 0)
                            {
                                _inArchives.Remove(user);
                            }
                        }
                    }

                    if (format is OutArchiveFormat)
                    {
                        if (_outArchives != null && _outArchives.ContainsKey(user) &&
                            _outArchives[user].ContainsKey((OutArchiveFormat) format) &&
                            _outArchives[user][(OutArchiveFormat) format] != null)
                        {
                            try
                            {
                                Marshal.ReleaseComObject(_outArchives[user][(OutArchiveFormat) format]);
                            }
                            catch (InvalidComObjectException)
                            {
                            }

                            _outArchives[user].Remove((OutArchiveFormat) format);
                            _totalUsers--;
                            if (_outArchives[user].Count == 0)
                            {
                                _outArchives.Remove(user);
                            }
                        }
                    }

                    if ((_inArchives == null || _inArchives.Count == 0) &&
                        (_outArchives == null || _outArchives.Count == 0))
                    {
                        _inArchives = null;
                        _outArchives = null;
                        if (_totalUsers == 0)
                        {
                            NativeMethods.FreeLibrary(_modulePtr);
                            _modulePtr = IntPtr.Zero;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets IInArchive interface to extract 7-zip archives.
        /// </summary>
        /// <param name="format">Archive format.</param>
        /// <param name="user">Archive format user.</param>
        public static IInArchive InArchive(InArchiveFormat format, object user)
        {
            lock (_syncRoot)
            {
                if (_inArchives[user][format] == null)
                {
                    var sp = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
                    sp.Demand();
                    if (_modulePtr == IntPtr.Zero)
                    {
                        LoadLibrary(user, format);
                        if (_modulePtr == IntPtr.Zero)
                        {
                            throw new SevenZipLibraryException();
                        }
                    }

                    var createObject = (NativeMethods.CreateObjectDelegate) Marshal.GetDelegateForFunctionPointer(
                        NativeMethods.GetProcAddress(_modulePtr, "CreateObject"),
                        typeof(NativeMethods.CreateObjectDelegate));

                    object result;
                    var interfaceId = typeof(IInArchive).GUID;
                    var classID = Formats.InFormatGuids[format];
                    try
                    {
                        createObject(ref classID, ref interfaceId, out result);
                    }
                    catch (Exception)
                    {
                        throw new SevenZipLibraryException("Your 7-zip library does not support this archive type.");
                    }

                    InitUserInFormat(user, format);
                    _inArchives[user][format] = result as IInArchive;
                }

                return _inArchives[user][format];
            }
        }

        /// <summary>
        /// Gets IOutArchive interface to pack 7-zip archives.
        /// </summary>
        /// <param name="format">Archive format.</param>  
        /// <param name="user">Archive format user.</param>
        public static IOutArchive OutArchive(OutArchiveFormat format, object user)
        {
            lock (_syncRoot)
            {
                if (_outArchives[user][format] == null)
                {
                    var sp = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
                    sp.Demand();
                    if (_modulePtr == IntPtr.Zero)
                    {
                        throw new SevenZipLibraryException();
                    }

                    var createObject = (NativeMethods.CreateObjectDelegate) Marshal.GetDelegateForFunctionPointer(
                        NativeMethods.GetProcAddress(_modulePtr, "CreateObject"),
                        typeof(NativeMethods.CreateObjectDelegate));

                    object result;
                    var interfaceId = typeof(IOutArchive).GUID;
                    var classID = Formats.OutFormatGuids[format];
                    try
                    {
                        createObject(ref classID, ref interfaceId, out result);
                    }
                    catch (Exception)
                    {
                        throw new SevenZipLibraryException("Your 7-zip library does not support this archive type.");
                    }

                    InitUserOutFormat(user, format);
                    _outArchives[user][format] = result as IOutArchive;
                }

                return _outArchives[user][format];
            }
        }

        /// <summary>   Gets the 7zip library path.</summary>
        /// <exception cref="TimeoutException"> Thrown when a Timeout error condition occurs.</exception>
        /// <returns>   The library path.</returns>
        /// <remarks> In WindowsCE, a file called 7z.dll in the same directory as this assembly.  If it does not exist,
        ///           an Embedded resource is extracted to this directory and used.  All other platforms use the following
        ///           logic:
        ///           1. [All] The value provided to a previous call to SetLibraryPath() is used.
        ///           2. [All] Embedded resource is extracted to %TEMP% and used. (assuming build with embedded 7z.dll is used)  
        ///           If not found, we give up and fail.
        /// </remarks>
        private static string GetLibraryPath()
        {
            if (_libraryFileName != null && (_modulePtr != IntPtr.Zero || File.Exists(_libraryFileName)))
                return _libraryFileName;

            var bitness = IntPtr.Size == 4 ? "x86" : "x64";
            var thisType = typeof(SevenZipLibraryManager);
            var version = thisType.Assembly.GetName().Version.ToString(3);
            _libraryFileName = Path.Combine(Path.GetTempPath(),
                string.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture),
                    new string[] {"SevenZipSharp", version, bitness, "7z.dll"}));
            if (File.Exists(_libraryFileName)) return _libraryFileName;

            //NOTE: This is the approach used in https://github.com/jacobslusser/ScintillaNET for handling the native component.
            //      I liked it, so I added it to this project.  We could have a build configuration that doesn't embed the dlls
            //      to make our assembly smaller and future proof, but you'd need to handle distributing and setting the dll yourself.
            // Extract the embedded DLL http://stackoverflow.com/a/768429/2073621
            // Synchronize access to the file across processes http://stackoverflow.com/a/229567/2073621
            var guid = ((GuidAttribute) thisType.Assembly.GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0))
                .Value.ToString(CultureInfo.InvariantCulture);
            var name = string.Format(CultureInfo.InvariantCulture, "Global\\{{{0}}}", guid);
            using (var mutex = new Mutex(false, name))
            {
                var access = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                    MutexRights.FullControl, AccessControlType.Allow);
                var security = new MutexSecurity();
                security.AddAccessRule(access);
                mutex.SetAccessControl(security);
                var ownsHandle = false;
                try
                {
                    try
                    {
                        ownsHandle = mutex.WaitOne(5000, false); // 5 sec
                        if (!ownsHandle)
                        {
                            var timeoutMessage = string.Format(CultureInfo.InvariantCulture,
                                "Timeout waiting for exclusive access to '{0}'.", _libraryFileName);
                            throw new TimeoutException(timeoutMessage);
                        }
                    }
                    catch (AbandonedMutexException)
                    {
                        // Previous process terminated abnormally
                        ownsHandle = true;
                    }

                    // Double-checked (process) lock
                    if (File.Exists(_libraryFileName)) return _libraryFileName;

                    // Write the embedded file to disk
                    var directory = Path.GetDirectoryName(_libraryFileName);
                    if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                    Exception ex;
                    try
                    {
                        var resource = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.7z.dll.gz",
                            thisType.Namespace, bitness); //packing of resources differ
                        var resourceStream = thisType.Assembly.GetManifestResourceStream(resource);
                        if (resourceStream == null)
                        {
                            ex = new InvalidProgramException(
                                $"Could not extract resource named '{resource}' from assembly '{thisType.Assembly.FullName}'");
                        }
                        else
                        {
                            using (var gzipStream = new GZipStream(resourceStream,
                                System.IO.Compression.CompressionMode.Decompress))
                            {
                                using (var fileStream = File.Create(_libraryFileName))
                                {
                                    gzipStream.CopyTo(fileStream);
                                    return _libraryFileName;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ex = e;
                        if (File.Exists(_libraryFileName)) File.Delete(_libraryFileName);
                    }

                    _libraryFileName = null;
                    throw new SevenZipLibraryException(
                        "Unable to locate the 7z.dll. Please call SetLibraryPath() or set app.config AppSetting '7zLocation' " +
                        "which must be path to the proper bit 7z.dll", ex);
                }
                finally
                {
                    if (ownsHandle) mutex.ReleaseMutex();
                }
            }
        }

        /// <summary>
        /// Sets the application-wide default module path of the native 7zip library. In WindowsCE this is a no-op.  The library MUST be in the app directory.
        /// </summary>
        /// <param name="libraryPath">The native 7zip module path.</param>
        /// <remarks>
        /// This method must be called prior to any other calls to this library.
        /// The <paramref name="libraryPath" /> can be relative or absolute.
        /// </remarks>
        public static void SetLibraryPath(string libraryPath)
        {
            if (_modulePtr != IntPtr.Zero && !Path.GetFullPath(libraryPath)
                    .Equals(Path.GetFullPath(_libraryFileName), StringComparison.OrdinalIgnoreCase))
            {
                throw new SevenZipLibraryException("can not change the library path while the library \"" +
                                                   _libraryFileName + "\" is being used.");
            }

            if (!File.Exists(libraryPath))
            {
                throw new SevenZipLibraryException("can not change the library path because the file \"" + libraryPath +
                                                   "\" does not exist.");
            }

            _libraryFileName = libraryPath;
            _features = null;
        }

        /// <summary>
        /// Returns the version information of the native 7zip library.
        /// </summary>
        /// <returns>An object representing the version information of the native 7zip library.</returns>
        public static FileVersionInfo GetLibraryVersion()
        {
            var path = GetLibraryPath();
            var version = FileVersionInfo.GetVersionInfo(path);
            return version;
        }
    }
#endif
}