﻿
using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP
{
    /// <summary>
    /// This is the Microsoft Windows BMP v2 and IBM OS/2 BMP v1 (and later) DIB (Device Independent Bitmap)
    /// information header: BITMAPCOREHEADER.
    /// <para>Supported since Windows 2.0, Windows CE 2.0 and OS/2 1.0.</para>
    /// <para>Implemented on Microsoft Windows BMP v2 and IBM OS/2 BMP v1 format.</para>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/dd183372(v=vs.85).aspx">See this MSDN link for more information.</seealso>
    /// </summary>
    /// <remarks>
    /// Make shore that <c>sizeof(WinCoreHeader)</c> returns the size of 12 bytes and is byte aligned.
    /// All structure fields are stored little-endian on the file.
    /// <para>The colors (<seealso cref="WinRgbTriple"/>) in the palette table should appear in order of importance
    /// and must follow this structure.</para>
    /// <para>Each scan line must be zero-padded to end on a DWORD (4 bytes) boundary.</para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12)]
    internal struct WinCoreHeader
    {
        // ** Fields for Microsoft Windows BMP v2 and IBM OS/2 BMP v1 DIB header

        /// <summary>
        /// The size in bytes required to store this structure: Always 12.
        /// </summary>
        public uint Size;

        /// <summary>
        /// The width of the bitmap, in pixels.
        /// </summary>
        public ushort Width;

        /// <summary>
        /// The height of the bitmap, in pixels.
        /// </summary>
        public ushort Height;

        /// <summary>
        /// The number of planes for the target device: Always 1.
        /// </summary>
        public ushort Planes;

        /// <summary>
        /// The number of bits-per-pixel (bpp). This value must be one of: 1, 2, 4, 8, or 24.
        /// <para>If <c>WinCoreHeader.BitsPerPixel</c> is 2, the bitmap is Windows CE 2.0 and above specific.</para>
        /// <see cref="WinRgbTriple"/> structure vector (most important colors at top), up to the maximum palette size dictated by the bpp.
        /// </summary>
        /// <remarks>
        /// The color table (if present) must follow the <c>WinCoreHeader</c> structure, and consist of
        /// </remarks>
        public ushort BitsPerPixel;
    }
}
