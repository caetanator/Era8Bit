/**
 * Win32CieXyzTriple.cs
 *
 * PURPOSE
 *  This structure represents a Microsoft Windows BMP v5 RGB CIE XYZ coordinates of a specific color olor space.
 *  Introduced in Microsoft's Windows 2000.
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
    /// The <c>BitmapCieXyzTriple</c> structure contains the X, Y, and Z coordinates of the three colors
    /// that correspond to the Red, Green, and Blue endpoints for a specified logical color space
    /// (color representation using <see cref="Win32CieXyz"/> color components).
    /// This is the Microsoft CIEXYZTRIPLE implementation.
    /// <para>Supported since Windows 2000 and Windows 98</para>
    /// <para>Implemented on Microsoft Windows BMP v4 format.</para>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/dd371833(v=vs.85).aspx">See this MSDN link for more information.</seealso>
    /// </summary>
    /// <remarks>
    /// Make shore that <c>sizeof(BitmapCieXyzTriple)</c> returns the size of 36 bytes and is byte aligned.
    /// All structure fields are stored little-endian on the file.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 36)]
    internal struct Win32CieXyzTriple
    {
        /// <summary>
        /// A CIE XYZ 1931 color space for the Red component.
        /// </summary>
        public Win32CieXyz Red;

        /// <summary>
        /// A CIE XYZ 1931 color space for the Green component.
        /// </summary>
        public Win32CieXyz Green;

        /// <summary>
        /// A CIE XYZ 1931 color space for the Blue component.
        /// </summary>
        public Win32CieXyz Blue;
    }
}
