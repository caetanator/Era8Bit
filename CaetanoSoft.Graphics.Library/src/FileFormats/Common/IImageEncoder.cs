/**
 * IImageEncoder.cs
 *
 * PURPOSE
 *  This interface encapsulates the properties and methods needed for encoding an image from a stream.
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

using System.IO;
using CaetanoSoft.Graphics.PixelFormats;

namespace CaetanoSoft.Graphics.FileFormats.Common
{
    /// <summary>
    /// This interface encapsulates the properties and methods needed for encoding an image from a stream.
    /// </summary>
    public interface IImageEncoder
    {
        /// <summary>
        /// .
        /// </summary>
        /// <typeparam name="TPixel">The pixel format.</typeparam>
        /// <param name="image">.</param>
        /// <param name="stream">.</param>
        /// 
        /// <summary>Encodes the image to the specified stream from the <see cref="Image{TPixel}"/>.</summary>
        /// <typeparam name="TPixel">The type of the pixel.</typeparam>
        /// <param name="stream">The <see cref="Stream"/> to encode the image data to.</param>
        /// <param name="image">The <see cref="Image{TPixel}"/> to encode from.</param>
        void Encode<TPixel>(Stream stream, Image<TPixel> image) where TPixel : struct, IPixel<TPixel>;
    }
}
