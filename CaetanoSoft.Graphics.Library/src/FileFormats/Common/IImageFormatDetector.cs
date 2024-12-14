/**
 * IImageFormatDetector.cs
 *
 * PURPOSE
 *  This interface encapsulates the properties and methods needed to detect the file format 
 *  from an array of bytes, containing the file Magic ID.
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

using System;

namespace CaetanoSoft.Graphics.FileFormats.Common
{
    /// <summary>
    /// This interface encapsulates the properties and methods needed to detect the file format 
    /// from an array of bytes, containing the file Magic ID.
    /// </summary>
    public interface IImageFormatDetector
    {
        // ** Properties

        /// <summary>Gets the size of the Magic ID bytes needed to detect this image type..</summary>
        /// <value>The size of the Magic ID, in bytes.</value>
        int MagicIdSize { get; }

        // ** Methods

        /// <summary>Detects the image format.</summary>
        /// <param name="magicBytes">The magic bytes needed to identify the image format.</param>
        /// <returns>
        /// If successful detected, returns <see cref="IImageFormat"/>, otherwise returns <c>null</c>.
        /// </returns>
        IImageFormat DetectFormat(ReadOnlySpan<byte> magicBytes);
    }
}
