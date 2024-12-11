/**
 * Win32CoreHeader.cs
 *
 * PURPOSE
 *   This structure represents a Microsoft Windows BMP v2 DIB (Device Independent Bitmap) header structure (<c>BITMAPCOREHEADER</c>) of a BMP bitmap.
 *   Introduced in Microsoft's Windows 2.0, Windows NT 3.1, Windows CE 2.0 and OS/2 1.0.
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
    /// This structure represents a Microsoft Windows BMP v2 DIB (Device Independent Bitmap) header structure (<c>BITMAPCOREHEADER</c>) of a BMP bitmap.
    /// <para>Introduced in Microsoft's Windows 2.0, Windows NT 3.1, Windows CE 2.0 and OS/2 1.0.</para>
    /// <para>Supported by the Windows OS API since Windows 2.0, Windows NT 3.1, Windows CE 2.0 and OS/2 1.0.</para>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/dd183372(v=vs.85).aspx">See this MSDN link for more information.</seealso>
    /// </summary>
    /// <remarks>
    /// The <c>sizeof(Win32CoreHeader)</c> returns 12 bytes and is sequential and byte aligned.
    /// All structure fields are stored little-endian on the file.
    /// <para>The color table (if present) must immediately follow this structure and stored in an vector array of 
    /// <seealso cref="Win32TriplePalette"/> with most important colors at top, up to the maximum palette size 
    /// dictated by the <seealso cref="BitsPerPixel"/> field.</para>
    /// <para>Each bitmap scan line of the pixels data, must be zero-padded to end on a DWORD (4 bytes) boundary.</para>
    /// <para>
    /// When the bitmap array immediately follows the <c>Win32CoreHeader</c> structure plus the palette array (if needed), it is a packed bitmap.
    /// Packed bitmaps are referenced by a single pointer.
    /// Packed bitmaps require that the <seealso cref="PaletteSize"/> field must 
    /// be either 0 or the actual size of the color table.
    /// </para>
    /// </remarks>
    /// 
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12)]
    internal struct Win32CoreHeader
    {
        // ** Fields added for Microsoft Windows BMP v2 and IBM OS/2 BMP v1 DIB header or upgraded from Microsoft Windows BMP v1

        /// <summary>
        /// The size required to store this structure, in bytes. Always 124.
        /// <para>
        /// Also used to determine the version of the BMP DIB, as:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Version</description>
        /// </listheader>
        /// <item>
        /// <term>12</term>
        /// <description>Windows BMP DIB v2 (<seealso cref="Win32CoreHeader"><c>BITMAPCOREHEADER</c></seealso>) or OS/2 BMP DIB v1 (<seealso cref="Win32CoreHeader"><c>OS21XBITMAPHEADER</c></seealso>)</description>
        /// </item>
        /// <item>
        /// <term>40</term>
        /// <description>Windows BMP DIB v3 (<seealso cref="Win32InfoHeaderV5"><c>BITMAPINFOHEADER</c></seealso>)</description>
        /// </item>
        /// <item>
        /// <term>52</term>
        /// <description>Windows BMP DIB v3.2 [only Windows NT 3.5 and above] (<seealso cref="Win32InfoHeaderV5"><c>BITMAPV2INFOHEADER</c></seealso>)</description>
        /// </item>
        /// <item>
        /// <term>56</term>
        /// <description>Windows BMP DIB v3.3 [only Windows CE 5 and above] (<seealso cref="Win32InfoHeaderV5"><c>BITMAPV3INFOHEADER</c></seealso>)</description>
        /// </item>
        /// <item>
        /// <term>108</term>
        /// <description>Windows BMP DIB v4 (<seealso cref="Win32InfoHeaderV5"><c>BITMAPV4HEADER</c></seealso>)</description>
        /// </item>
        /// <item>
        /// <term>124</term>
        /// <description>Windows BMP DIB v5 (<seealso cref="Win32InfoHeaderV5"><c>BITMAPV5HEADER</c></seealso>)</description>
        /// </item>
        /// <item>
        /// <term>16 to 64</term>
        /// <description>OS/2 BMP DIB v2 [only OS/2 2.0 and above] (<seealso cref="OS2InfoHeaderV2_ExtraFields"><c>OS22XBITMAPHEADER</c></seealso>)</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        public uint Size;

        /// <summary>
        /// Specifies the width of the bitmap image, in pixels.
        /// <para>Must be bigger that zero (<c>Width &gt; 0</c>).</para>
        /// </summary>
        public ushort Width;

        /// <summary>
        /// Specifies the height of the bitmap image, in pixels.
        /// <para>Must be bigger that zero (<c>Height &gt; 0</c>).</para>
        /// </summary>
        public ushort Height;

        /// <summary>
        /// The number of color planes for the target device: Always 1.
        /// </summary>
        public ushort ColorPlanes;

        /// <summary>
        /// The number of bits-per-pixel (bpp) for each pixel in the image data. This value must be one of: 1, 4, 8 or 24.
        /// </summary>
        public ushort BitsPerPixel;
    }
}
