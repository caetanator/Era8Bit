/**
 * Image.cs
 *
 * PURPOSE
 *  This interface encapsulates the properties and methods needed for decoding an image from a stream.
 *
 * CONTACTS
 *  For any question or bug report, regarding any portion of the "CaetanoSoft.Graphics.FileFormats.BMP.BmpWin32Structures" project:
 *      https://github.com/caetanator/Era8Bit
 *  COPYRIGHT
 *  This file is distributed under the terms of the GNU General Public License (GPL) v3. Copies of the GPL can be obtained from:
 *      ftp://prep.ai.mit.edu/pub/gnu/GPL
 *  Each contributing author retains all rights to their own work.
 *  (C) 2024-2026 José Caetano Silva
 * HISTORY
 *  2024-12-10: Created.
 *  2024-12-10: Renamed and updated.
 */

using CaetanoSoft.Graphics.PixelFormats;

namespace CaetanoSoft.Graphics.FileFormats.Common
{
    public class Image<TPixel> where TPixel : struct, IPixel<TPixel>
    {
        
    }
}
