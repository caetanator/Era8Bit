/**
 * EnumOS2ColorEncoding.cs
 *
 * PURPOSE
 *  This enumeration contains all possible Color Encoding values for IBM OS/2 BMP v2 bitmaps.
 *  Introduced in IBM OS/2 2.0.
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
 *  (C)2009-2024 José Caetano Silva
 *
 * HISTORY
 *  2009-09-15: Created.
 *  2017-04-13: Major rewrite.
 *  2023-09-16: Renamed and updated.
 *  2024-12-10: More documentation updates.
 */

using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures
{
    /// <summary>
    /// This is the IBM OS/2 2.x (and above) BMP v2 Bitmap Color Encoding model used to describe the stored bitmap data (pixels).
    /// For use in <c>OS22XBITMAPHEADER.ulColorEncoding</c> field.
    /// <para>Supported since IBM OS/2 2.0.</para>
    /// <seealso href="http://www.fileformat.info/format/os2bmp/egff.htm">See this FileFormat link for more information.</seealso>
    /// <seealso href="https://en.wikipedia.org/wiki/BMP_file_format">See this Wikipedia link for additional information.</seealso>
    /// </summary>
    internal enum EnumOS2ColorEncoding : ushort
    {
        /// <summary>
        /// RGB color encoding scheme. Only RGB is allowed.
        /// Same as <c>BCE_RGB</c> macro of the OS/2 2.0 and above SDK.
        /// </summary>
        RGB = 0,

        /// <summary>
        /// Palette color encoding scheme. This value is not allowed nor supported, but is reserved.
        /// Same as <c>BCE_PALETTE</c> macro of the OS/2 2.0 and above SDK.
        /// </summary>
        Palette = 0xFFFF
    }
}
