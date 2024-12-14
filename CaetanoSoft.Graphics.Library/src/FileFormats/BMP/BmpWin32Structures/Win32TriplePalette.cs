/**
 * Win32TriplePalette.cs
 *
 * PURPOSE
 *  This represents a BGR (Blue, Green and Blue) color entry on a color table/palette of a Microsoft Windows BMP v2
 *  (RGBTRIPLE structure) and IBM OS/2 BMP v1 (OS21XPALETTEELEMENT structure) bitmap.
 *
 * CONTACTS
 *  E-mail regarding any portion of the "CaetanoSoft.Graphics.FileFormats.BMP.BmpWin32Structures" project:
 *      José Caetano Silva, jcaetano@users.sourceforge.net
 *
 * COPYRIGHT
 *  This file is distributed under the terms of the GNU General Public
 *  License (GPL) v3. Copies of the GPL can be obtained from:
 *      ftp://prep.ai.mit.edu/pub/gnu/GPL
 *  Each contributing author retains all rights to their own work.
 *
 *  (C)2009-2023 José Caetano Silva
 *
 * HISTORY
 *  2009-09-15: Created.
 *  2017-04-13: Major rewrite.
 *  2023-09-16: Renamed and updated.
 */

using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures
{
    /// <summary>
    /// This represents a BGR (Blue, Green and Blue) color entry on a color table/palette of a Microsoft Windows BMP v2
    /// (RGBTRIPLE structure) and IBM OS/2 BMP v1 (OS21XPALETTEELEMENT structure) bitmap.
    /// <para>Supported since Windows 2.0, Windows NT 3.1, Windows CE 2.0 and OS/2 1.0.</para>
    /// <para>Implemented on Microsoft Windows BMP v2 and IBM OS/2 BMP v1 format.</para>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/dd162939(v=vs.85).aspx">See this MSDN link for more information.</seealso>
    /// </summary>
    /// <remarks>
    /// Make shore that <c>sizeof(Win32TriplePalette)</c> returns the size of 3 bytes and is byte aligned.
    /// All structure fields are stored little-endian on the file.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 3)]
    internal struct Win32TriplePalette
    {
        /// <summary>
        /// Specifies the intensity of blue in the color in the range 0 to 255.
        /// </summary>
        public byte Blue;

        /// <summary>
        /// Specifies the intensity of green in the color in the range 0 to 255.
        /// </summary>
        public byte Green;

        /// <summary>
        /// Specifies the intensity of red in the color in the range 0 to 255.
        /// </summary>
        public byte Red;
    }
}
