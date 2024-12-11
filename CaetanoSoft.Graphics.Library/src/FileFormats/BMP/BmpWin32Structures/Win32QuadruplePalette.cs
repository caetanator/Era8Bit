/**
 * Win32RgbQuadruple.cs
 *
 * PURPOSE
 *   This represents a BGRA (Blue, Green, Blue and Alpha) color entry on a table/palette of a Microsoft Windows BMP v3
 *   (RGBQUAD structure) and IBM OS/2 BMP v2 (OS22XPALETTEELEMENT structure) bitmap.
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
    /// This represents a BGRA (Blue, Green, Blue and Alpha) color entry on a table/palette of a Microsoft Windows BMP v3
    /// (RGBQUAD structure) and IBM OS/2 BMP v2 (OS22XPALETTEELEMENT structure) bitmap.<c>RGBQUAD</c>
    /// <para>Supported since Windows 3.0, Windows NT 3.1, Windows CE 1.0 and OS/2 2.0.</para>
    /// <para>Implemented on Microsoft Windows BMP v3 and IBM OS/2 BMP v2 format.</para>
    /// </summary>
    /// <remarks>
    /// Make shore that <c>sizeof(Win32RgbQuadruple)</c> returns the size of 4 bytes and is byte aligned.
    /// All structure fields are stored little-endian on the file.
    /// <para>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/dd162938(v=vs.85).aspx">See this MSDN link for more information.</seealso>
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    internal struct Win32RgbQuadruple
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

        /// <summary>
        /// Not used on Microsoft Windows BMP v3 and IBM OS/2 BMP v2 DIB (reserved and must be 0).
        /// </summary>
        /// <remarks>Optional Alpha color channel on Microsoft Windows BMP v4. Default is set to 0.</remarks>
        public byte Alpha;
    }
}
