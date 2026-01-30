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
using System.IO;

namespace SevenZipSharp
{
#if UNMANAGED
    /// <summary>
    /// Readable archive format enumeration.
    /// </summary>
    public enum InArchiveFormat
    {
        /// <summary>
        /// Open 7-zip archive format.
        /// </summary>  
        /// <remarks><a href="http://en.wikipedia.org/wiki/7-zip">Wikipedia information</a></remarks> 
        SevenZip,

        /// <summary>
        /// Open Bzip2 archive format.
        /// </summary>
        /// <remarks><a href="http://en.wikipedia.org/wiki/Bzip2">Wikipedia information</a></remarks>
        BZip2,

        /// <summary>
        /// Open Gzip archive format.
        /// </summary>
        /// <remarks><a href="http://en.wikipedia.org/wiki/Gzip">Wikipedia information</a></remarks>
        GZip,

        /// <summary>
        /// RarLab Rar archive format.
        /// </summary>
        /// <remarks><a href="http://en.wikipedia.org/wiki/Rar">Wikipedia information</a></remarks>
        Rar,

        /// <summary>
        /// RarLab Rar4 archive format.
        /// </summary>
        Rar4,

        /// <summary>
        /// Open Tar archive format.
        /// </summary>
        /// <remarks><a href="http://en.wikipedia.org/wiki/Tar_(file_format)">Wikipedia information</a></remarks>
        Tar,

        /// <summary>
        /// Open Zip archive format.
        /// </summary>
        /// <remarks><a href="http://en.wikipedia.org/wiki/ZIP_(file_format)">Wikipedia information</a></remarks>
        Zip,
    }

    /// <summary>
    /// Writable archive format enumeration.
    /// </summary>    
    public enum OutArchiveFormat
    {
        /// <summary>
        /// Open 7-zip archive format.
        /// </summary>
        /// <remarks><a href="http://en.wikipedia.org/wiki/7-zip">Wikipedia information</a></remarks>
        SevenZip,

        /// <summary>
        /// Open Zip archive format.
        /// </summary>
        /// <remarks><a href="http://en.wikipedia.org/wiki/ZIP_(file_format)">Wikipedia information</a></remarks>
        Zip,

        /// <summary>
        /// Open Gzip archive format.
        /// </summary>
        /// <remarks><a href="http://en.wikipedia.org/wiki/Gzip">Wikipedia information</a></remarks>
        GZip,

        /// <summary>       
        /// Open Bzip2 archive format.
        /// </summary>
        /// <remarks><a href="http://en.wikipedia.org/wiki/Bzip2">Wikipedia information</a></remarks>
        BZip2,

        /// <summary>
        /// Open Tar archive format.
        /// </summary>
        /// <remarks><a href="http://en.wikipedia.org/wiki/Tar_(file_format)">Wikipedia information</a></remarks>
        Tar,
    }

    /// <summary>
    /// Compression level enumeration
    /// </summary>
    public enum CompressionLevel
    {
        /// <summary>
        /// No compression
        /// </summary>
        None,

        /// <summary>
        /// Very low compression level
        /// </summary>
        Fast,

        /// <summary>
        /// Low compression level
        /// </summary>
        Low,

        /// <summary>
        /// Normal compression level (default)
        /// </summary>
        Normal,

        /// <summary>
        /// High compression level
        /// </summary>
        High,

        /// <summary>
        /// The best compression level (slow)
        /// </summary>
        Ultra
    }

    /// <summary>
    /// Compression method enumeration.
    /// </summary>
    /// <remarks>Some methods are applicable only to Zip format, some - only to 7-zip.</remarks>
    public enum CompressionMethod
    {
        /// <summary>
        /// Zip or 7-zip|no compression method.
        /// </summary>
        Copy,

        /// <summary>
        /// Zip|Deflate method.
        /// </summary>
        Deflate,

        /// <summary>
        /// Zip|Deflate64 method.
        /// </summary>
        Deflate64,

        /// <summary>
        /// Zip or 7-zip|Bzip2 method.
        /// </summary>
        /// <remarks><a href="http://en.wikipedia.org/wiki/Cabinet_(file_format)">Wikipedia information</a></remarks>
        BZip2,

        /// <summary>
        /// Zip or 7-zip|LZMA method based on Lempel-Ziv algorithm, it is default for 7-zip.
        /// </summary>
        Lzma,

        /// <summary>
        /// 7-zip|LZMA version 2, LZMA with improved multithreading and usually slight archive size decrease.
        /// </summary>
        Lzma2,

        /// <summary>
        /// Zip or 7-zip|PPMd method based on Dmitry Shkarin's PPMdH source code, very efficient for compressing texts.
        /// </summary>
        /// <remarks><a href="http://en.wikipedia.org/wiki/Prediction_by_Partial_Matching">Wikipedia information</a></remarks>
        Ppmd,

        /// <summary>
        /// No method change.
        /// </summary>
        Default
    }

    /// <summary>   Settings for how errors compressing files are handled.</summary>
    public enum CompressionErrorBehavior
    {
        /// <summary>   Default - Throws exception.</summary>
        ThrowException = 0,

        /// <summary>   The file is skipped - no entry in the archive.</summary>
        SkipFile = 1,

        /// <summary>
        /// The file is replaced by a UTF-8 encoded text file that explains the reason for the compression failure.
        /// </summary>
        ReplaceFileWithTextOfFailureReason = 2,
    }

    /// <summary>   Type of error encountered when compressing.</summary>
    public enum CompressionErrorType
    {
        /// <summary>  The source file does not exist.</summary>
        SourceFileMissing = 0,

        /// <summary>  The source file cannot be opened. </summary>
        SourceFileCannotBeOpened = 1,
    }

    /// <summary>
    /// Archive format routines
    /// </summary>
    public static class Formats
    {
        /// <summary>
        /// List of readable archive format interface guids for 7-zip COM interop.
        /// </summary>
        internal static readonly Dictionary<InArchiveFormat, Guid> InFormatGuids =
            new Dictionary<InArchiveFormat, Guid>(20)

                #region InFormatGuids initialization
            // added support for rar5 
                {
                    {InArchiveFormat.SevenZip, new Guid("23170f69-40c1-278a-1000-000110070000")},
                    {InArchiveFormat.BZip2, new Guid("23170f69-40c1-278a-1000-000110020000")},
                    {InArchiveFormat.GZip, new Guid("23170f69-40c1-278a-1000-000110ef0000")},
                    {InArchiveFormat.Rar, new Guid("23170f69-40c1-278a-1000-000110CC0000")},
                    {InArchiveFormat.Rar4, new Guid("23170f69-40c1-278a-1000-000110030000")},
                    {InArchiveFormat.Tar, new Guid("23170f69-40c1-278a-1000-000110ee0000")},
                    {InArchiveFormat.Zip, new Guid("23170f69-40c1-278a-1000-000110010000")}
                };

        #endregion

        /// <summary>
        /// List of writable archive format interface guids for 7-zip COM interop.
        /// </summary>
        internal static readonly Dictionary<OutArchiveFormat, Guid> OutFormatGuids =
            new Dictionary<OutArchiveFormat, Guid>(2)

                #region OutFormatGuids initialization

                {
                    {OutArchiveFormat.SevenZip, new Guid("23170f69-40c1-278a-1000-000110070000")},
                    {OutArchiveFormat.Zip, new Guid("23170f69-40c1-278a-1000-000110010000")},
                    {OutArchiveFormat.BZip2, new Guid("23170f69-40c1-278a-1000-000110020000")},
                    {OutArchiveFormat.GZip, new Guid("23170f69-40c1-278a-1000-000110ef0000")},
                    {OutArchiveFormat.Tar, new Guid("23170f69-40c1-278a-1000-000110ee0000")}
                };

        #endregion

        internal static readonly Dictionary<CompressionMethod, string> MethodNames =
            new Dictionary<CompressionMethod, string>(6)

                #region MethodNames initialization

                {
                    {CompressionMethod.Copy, "Copy"},
                    {CompressionMethod.Deflate, "Deflate"},
                    {CompressionMethod.Deflate64, "Deflate64"},
                    {CompressionMethod.Lzma, "LZMA"},
                    {CompressionMethod.Lzma2, "LZMA2"},
                    {CompressionMethod.Ppmd, "PPMd"},
                    {CompressionMethod.BZip2, "BZip2"}
                };

        #endregion

        internal static readonly Dictionary<OutArchiveFormat, InArchiveFormat> InForOutFormats =
            new Dictionary<OutArchiveFormat, InArchiveFormat>(6)

                #region InForOutFormats initialization

                {
                    {OutArchiveFormat.SevenZip, InArchiveFormat.SevenZip},
                    {OutArchiveFormat.GZip, InArchiveFormat.GZip},
                    {OutArchiveFormat.BZip2, InArchiveFormat.BZip2},
                    {OutArchiveFormat.Tar, InArchiveFormat.Tar},
                    {OutArchiveFormat.Zip, InArchiveFormat.Zip}
                };

        #endregion

        /// <summary>
        /// List of archive formats corresponding to specific extensions
        /// </summary>
        private static readonly Dictionary<string, InArchiveFormat> InExtensionFormats =
            new Dictionary<string, InArchiveFormat>

                #region InExtensionFormats initialization

                {
                    {"7z", InArchiveFormat.SevenZip},
                    {"gz", InArchiveFormat.GZip},
                    {"tar", InArchiveFormat.Tar},
                    {"rar", InArchiveFormat.Rar},
                    {"zip", InArchiveFormat.Zip},
                    {"bz2", InArchiveFormat.BZip2}
                };

        #endregion

        /// <summary>
        /// List of archive formats corresponding to specific signatures
        /// </summary>
        /// <remarks>Based on the information at <a href="http://www.garykessler.net/library/file_sigs.html">this site.</a></remarks>
        internal static readonly Dictionary<string, InArchiveFormat> InSignatureFormats =
            new Dictionary<string, InArchiveFormat>

                #region InSignatureFormats initialization

                {
                    {"37-7A-BC-AF-27-1C", InArchiveFormat.SevenZip},
                    {"1F-8B-08", InArchiveFormat.GZip},
                    {"75-73-74-61-72", InArchiveFormat.Tar},
                    //257 byte offset
                    {"52-61-72-21-1A-07-00", InArchiveFormat.Rar4},
                    {"52-61-72-21-1A-07-01-00", InArchiveFormat.Rar},
                    {"50-4B-03-04", InArchiveFormat.Zip},
                    {"50-4B-07-08", InArchiveFormat.Zip}, // multivolume
                    //^ 2 byte offset
                    {"42-5A-68", InArchiveFormat.BZip2}
                };

        #endregion

        internal static Dictionary<InArchiveFormat, string> InSignatureFormatsReversed;

        static Formats()
        {
            InSignatureFormatsReversed = new Dictionary<InArchiveFormat, string>(InSignatureFormats.Count);
            foreach (var pair in InSignatureFormats)
            {
                if (!InSignatureFormatsReversed.TryGetValue(pair.Value, out var someString))
                    InSignatureFormatsReversed.Add(pair.Value, pair.Key);
            }
        }

        /// <summary>
        /// Gets InArchiveFormat for specified archive file name
        /// </summary>
        /// <param name="fileName">Archive file name</param>
        /// <param name="reportErrors">Indicates whether to throw exceptions</param>
        /// <returns>InArchiveFormat recognized by the file name extension</returns>
        /// <exception cref="System.ArgumentException"/>
        public static InArchiveFormat FormatByFileName(string fileName, bool reportErrors)
        {
            if (string.IsNullOrEmpty(fileName) && reportErrors)
            {
                throw new ArgumentException("File name is null or empty string!");
            }

            var extension = Path.GetExtension(fileName).Substring(1);
            if (!InExtensionFormats.ContainsKey(extension) && reportErrors)
            {
                throw new ArgumentException("Extension \"" + extension +
                                            "\" is not a supported archive file name extension.");
            }

            return InExtensionFormats[extension];
        }
    }
#endif
}