
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
