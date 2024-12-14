/**
 * IImageDecoder.cs
 *
 * PURPOSE
 *  This interface encapsulates the properties and methods needed for decoding an image from a stream.
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
    /// This interface encapsulates the properties and methods required for decoding an image from a stream..
    /// </summary>
    public interface IImageDecoder
    {
        /// <summary>
        /// Decodes the image from the specified stream to the <see cref="Image{TPixel}"/>.
        /// </summary>
        /// <typeparam name="TPixel">The type of the pixel.</typeparam>
        /// <param name="stream">The <see cref="Stream"/> containing the image data and information to be decoded from.</param>
        /// <param name="configuration">The configuration from the image.</param>
        /// <returns>
        /// If successful decoded, returns the decoded <see cref="Image{TPixel}"/>, otherwise returns <c>null</c>.
        /// </returns>
        Image<TPixel> Decode<TPixel>(Stream stream, ImageConfiguration configuration) where TPixel : struct, IPixel<TPixel>;
    }
}
