/**
 * BmpFormat.cs
 *
 * PURPOSE
 *  This class represents an image format, by its ID, file extensions and MIME types, for Windows and OS/2 BMP files.
 *
 * CONTACTS
 *  For any question or bug report, regarding any portion of the "CaetanoSoft.Graphics.FileFormats.BMP.BmpWin32Structures" project:
 *      https://github.com/caetanator/Era8Bit
 *
 * COPYRIGHT
 *  This file is distributed under the terms of the GNU General Public
 *  License (GPL) v3. Copies of the GPL can be obtained from:
 *      ftp://prep.ai.mit.edu/pub/gnu/GPL
 *  Each contributing author retains all rights to their own work.
 *
 *  (C)2019-2024 José Caetano Silva
 *
 * HISTORY
 *  2019-09-16: Created.
 *  2024-12-10: Renamed and updated.
 */

using System.Collections.Generic;

namespace CaetanoSoft.Graphics.FileFormats.Common
{
  /// <summary>
  /// This class represents an image format, by its ID, file extensions and MIME types, for Windows and OS/2 BMP files.
  /// </summary>
  internal sealed class BmpFormat : IImageFormat
  {
        // ** Constants

        /// Internal static string array for FileExtensions property
        private static readonly IEnumerable<string> _FileExtensions = new[] { "bmp", "dib" };

        /// Internal static string array for MimeTypes property
        private static readonly IEnumerable<string> _MimeTypes = new[] { "image/bmp", "image/x-windows-bmp" };

        // ** Override properties of IImageFormat

        /// <inheritdoc/>
        public string ID => "BMP";

        /// <inheritdoc/>
        public IEnumerable<string> FileExtensions => _FileExtensions;

        /// <inheritdoc/>
        public IEnumerable<string> MimeTypes => _MimeTypes;
    }
}
