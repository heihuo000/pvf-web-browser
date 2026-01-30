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

using System.IO;
using System.Windows.Threading;

namespace SevenZipSharp
{
    partial class SevenZipExtractor
    {
        #region Asynchronous core methods

        /// <summary>
        /// Recreates the instance of the SevenZipExtractor class.
        /// Used in asynchronous methods.
        /// </summary>
        private void RecreateInstanceIfNeeded()
        {
            if (NeedsToBeRecreated)
            {
                NeedsToBeRecreated = false;
                Stream backupStream = null;
                string backupFileName = null;
                if (string.IsNullOrEmpty(_fileName))
                    backupStream = _inStream;
                else
                    backupFileName = _fileName;
                CommonDispose();
                if (backupStream == null)
                    Init(backupFileName);
                else
                    Init(backupStream);
            }
        }

        internal override void SaveContext(DispatcherPriority eventPriority = DispatcherPriority.Normal)
        {
            DisposedCheck();
            _asynchronousDisposeLock = true;
            base.SaveContext(eventPriority);
        }

        internal override void ReleaseContext()
        {
            base.ReleaseContext();
            _asynchronousDisposeLock = false;
        }

        #endregion

        #region Delegates

        /// <summary>
        /// The delegate to use in BeginExtractArchive.
        /// </summary>
        /// <param name="directory">The directory where the files are to be unpacked.</param>
        private delegate void ExtractArchiveDelegate(string directory);

        /// <summary>
        /// The delegate to use in BeginExtractFile (by file name).
        /// </summary>
        /// <param name="fileName">The file full name in the archive file table.</param>
        /// <param name="stream">The stream where the file is to be unpacked.</param>
        private delegate void ExtractFileByFileNameDelegate(string fileName, Stream stream);

        /// <summary>
        /// The delegate to use in BeginExtractFile (by index).
        /// </summary>
        /// <param name="index">Index in the archive file table.</param>
        /// <param name="stream">The stream where the file is to be unpacked.</param>
        private delegate void ExtractFileByIndexDelegate(int index, Stream stream);

        /// <summary>
        /// The delegate to use in BeginExtractFiles(string directory, params int[] indexes).
        /// </summary>
        /// <param name="indexes">indexes of the files in the archive file table.</param>
        /// <param name="directory">Directory where the files are to be unpacked.</param>
        private delegate void ExtractFiles1Delegate(string directory, int[] indexes);

        /// <summary>
        /// The delegate to use in BeginExtractFiles(string directory, params string[] fileNames).
        /// </summary>
        /// <param name="fileNames">Full file names in the archive file table.</param>
        /// <param name="directory">Directory where the files are to be unpacked.</param>
        private delegate void ExtractFiles2Delegate(string directory, string[] fileNames);

        /// <summary>
        /// The delegate to use in BeginExtractFiles(ExtractFileCallback extractFileCallback).
        /// </summary>
        /// <param name="extractFileCallback">The callback to call for each file in the archive.</param>
        private delegate void ExtractFiles3Delegate(ExtractFileCallback extractFileCallback);

        #endregion

        /// <summary>
        /// Unpacks the whole archive asynchronously to the specified directory name at the specified priority.
        /// </summary>
        /// <param name="directory">The directory where the files are to be unpacked.</param>
        /// <param name="eventPriority">The priority of events, relative to the other pending operations in the System.Windows.Threading.Dispatcher event queue, the specified method is invoked.</param>
        public void BeginExtractArchive(string directory, DispatcherPriority eventPriority = DispatcherPriority.Normal)
        {
            SaveContext(eventPriority);
            (new ExtractArchiveDelegate(ExtractArchive)).BeginInvoke(directory, AsyncCallbackImplementation, this);
        }

        /// <summary>
        /// Unpacks the file asynchronously by its name to the specified stream.
        /// </summary>
        /// <param name="fileName">The file full name in the archive file table.</param>
        /// <param name="stream">The stream where the file is to be unpacked.</param>
        /// <param name="eventPriority">The priority of events, relative to the other pending operations in the System.Windows.Threading.Dispatcher event queue, the specified method is invoked.</param>
        public void BeginExtractFile(string fileName, Stream stream,
            DispatcherPriority eventPriority = DispatcherPriority.Normal)
        {
            SaveContext(eventPriority);
            (new ExtractFileByFileNameDelegate(ExtractFile)).BeginInvoke(fileName, stream, AsyncCallbackImplementation,
                this);
        }

        /// <summary>
        /// Unpacks the file asynchronously by its index to the specified stream.
        /// </summary>
        /// <param name="index">Index in the archive file table.</param>
        /// <param name="stream">The stream where the file is to be unpacked.</param>
        /// <param name="eventPriority">The priority of events, relative to the other pending operations in the System.Windows.Threading.Dispatcher event queue, the specified method is invoked.</param>
        public void BeginExtractFile(int index, Stream stream,
            DispatcherPriority eventPriority = DispatcherPriority.Normal)
        {
            SaveContext(eventPriority);
            (new ExtractFileByIndexDelegate(ExtractFile)).BeginInvoke(index, stream, AsyncCallbackImplementation, this);
        }

        /// <summary>
        /// Unpacks files asynchronously by their indices to the specified directory.
        /// </summary>
        /// <param name="indexes">indexes of the files in the archive file table.</param>
        /// <param name="directory">Directory where the files are to be unpacked.</param>
        /// <param name="eventPriority">The priority of events, relative to the other pending operations in the System.Windows.Threading.Dispatcher event queue, the specified method is invoked.</param>
        public void BeginExtractFiles(string directory, DispatcherPriority eventPriority = DispatcherPriority.Normal,
            params int[] indexes)
        {
            SaveContext(eventPriority);
            (new ExtractFiles1Delegate(ExtractFiles)).BeginInvoke(directory, indexes, AsyncCallbackImplementation,
                this);
        }

        /// <summary>
        /// Unpacks files asynchronously by their full names to the specified directory.
        /// </summary>
        /// <param name="fileNames">Full file names in the archive file table.</param>
        /// <param name="directory">Directory where the files are to be unpacked.</param>
        /// <param name="eventPriority">The priority of events, relative to the other pending operations in the System.Windows.Threading.Dispatcher event queue, the specified method is invoked.</param>
        public void BeginExtractFiles(string directory, DispatcherPriority eventPriority = DispatcherPriority.Normal,
            params string[] fileNames)
        {
            SaveContext(eventPriority);
            new ExtractFiles2Delegate(ExtractFiles).BeginInvoke(directory, fileNames, AsyncCallbackImplementation,
                this);
        }

        /// <summary>
        /// Extracts files from the archive asynchronously, giving a callback the choice what
        /// to do with each file. The order of the files is given by the archive.
        /// 7-Zip (and any other solid) archives are NOT supported.
        /// </summary>
        /// <param name="extractFileCallback">The callback to call for each file in the archive.</param>
        /// <param name="eventPriority">The priority of events, relative to the other pending operations in the System.Windows.Threading.Dispatcher event queue, the specified method is invoked.</param>
        public void BeginExtractFiles(ExtractFileCallback extractFileCallback,
            DispatcherPriority eventPriority = DispatcherPriority.Normal)
        {
            SaveContext(eventPriority);
            new ExtractFiles3Delegate(ExtractFiles).BeginInvoke(extractFileCallback, AsyncCallbackImplementation, this);
        }
    }
}
