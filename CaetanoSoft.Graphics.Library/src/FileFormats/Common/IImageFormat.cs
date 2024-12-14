/**
 * IImageFormat.cs
 *
 * PURPOSE
 *  This interface encapsulates the properties and methods needed to represents an image format, 
 *  by its ID, file extensions and MIME types.
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
    /// This interface encapsulates the properties and methods needed to represents an image format, 
    /// by its ID, file extensions and MIME types.
    /// </summary>
    public interface IImageFormat
    {
        // ** Properties

        /// <summary>
        /// Gets the ID that describes this image format.
        /// </summary>
        /// <value>The ID that describes this image format.</value>
        string ID { get; }

        /// <summary>
        /// Gets all known file extensions commonly used by this image format.
        /// Default file extension at the top of the list.
        /// </summary>
        /// <value>The file extensions used by this image format.</value>
        IEnumerable<string> FileExtensions { get; }

        /// <summary>
        /// Gets all MIME types used by this image format.
        /// Default MIME type at the top of the list.
        /// </summary>
        /// <value>The MIME types used by this image format.</value>
        IEnumerable<string> MimeTypes { get; }
    }
}
