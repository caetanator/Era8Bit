/**
 * EnumOS2Huffman1DHalftoning.cs
 *
 * PURPOSE
 *  This enumeration contains all possible Huffman 1D Halftoning values for IBM OS/2 BMP v2 bitmaps.
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
    /// This is the IBM OS/2 2.x (and above) BMP v2 Huffman 1D HalftoningMethod Halftoning used to compress the stored 1-bpp bitmap data (pixels).
    /// For use in <c>OS22XBITMAPHEADER.usRendering</c> field..
    /// <para>Supported since IBM OS/2 2.0.</para>
    /// <seealso href="http://www.fileformat.info/format/os2bmp/egff.htm">See this FileFormat link for more information.</seealso>
    /// <seealso href="https://en.wikipedia.org/wiki/BMP_file_format">See this Wikipedia link for more information.</seealso>
    /// </summary>
    internal enum EnumOS2Huffman1DHalftoning : ushort
    {
        /// <summary>
        /// No halftoning. Same as <c>BRH_NOTHALFTONED</c> macro of the OS/2 2.0 and above SDK.
        /// </summary>
        NoHalftoning = 0,

        /// <summary>
        /// Error-diffusion halftoning. Same as <c>BRH_ERRORDIFFUSION</c> macro of the OS/2 2.0 and above SDK.
        /// </summary>
        ErrorDiffusion = 1,

        /// <summary>
        /// Processing Algorithm for Non-coded Document Acquisition (PANDA) halftoning. 
        /// Same as <c>BRH_PANDA</c> macro of the OS/2 2.0 and aboveSDK.
        /// </summary>
        PANDA = 2,

        /// <summary>
        /// Super-circle halftoning. Same as <c>BRH_SUPERCIRCLE</c> macro of the OS/2 2.0 and above SDK.
        /// </summary>
        SuperCircle = 3
    }
}
