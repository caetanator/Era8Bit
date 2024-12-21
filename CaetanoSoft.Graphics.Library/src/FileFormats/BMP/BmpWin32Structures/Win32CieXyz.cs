
using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures
{
    /// <summary>
    /// The <c>Win32CieXyz</c> structure contains the X, Y, and Z coordinates of a specific color in a
    /// specified color space (CIE XYZ 1931). This is the Microsoft CIEXYZ implementation.
    /// <para>
    /// X, Y and Z are extrapolations of RGB created mathematically to avoid negative numbers
    /// (In 1931 there weren't any computers) and are called Tristimulus values.
    /// Y means luminance, Z is somewhat equal to blue, and X is a mix of cone response curves
    /// chosen to be orthogonal to luminance and non-negative.
    /// <seealso href="http://wolfcrow.com/blog/what-is-the-difference-between-cie-lab-cie-rgb-cie-xyy-and-cie-xyz">See this Wolfcrow link for more information.</seealso>
    /// </para>
    /// <para>Supported since Windows 2000 and Windows 98</para>
    /// <para>Implemented on Microsoft Windows BMP v4 format.</para>
    /// </summary>
    /// <remarks>
    /// Make shore that <c>sizeof(Win32CieXyz)</c> returns the size of 12 bytes and is byte aligned.
    /// All structure fields are stored little-endian on the file.
    /// <para>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/dd371828(v=vs.85).aspx">See this MSDN link for more information.</seealso>
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 12)]
    internal struct Win32CieXyz
    {
        /// <summary>
        /// The X coordinate (mix (a linear combination) of cone response curves chosen to be nonnegative).
        /// <remarks>Specified in fixed point 2.30 format. The upper 2 bits are the unsigned integer value. The lower 30 bits are the fractional part.</remarks>
        /// </summary>
        /// <seealso href="https://en.wikipedia.org/wiki/CIE_1931_color_space">See this Wikipedia link for more information.</seealso>
        public uint X;

        /// <summary>
        /// The Y coordinate (luminance).
        /// <remarks>Specified in fixed point 2.30 format. The upper 2 bits are the unsigned integer value. The lower 30 bits are the fractional part.</remarks>
        /// </summary>
        /// <seealso href="https://en.wikipedia.org/wiki/CIE_1931_color_space">See this Wikipedia link for more information.</seealso>
        public uint Y;

        /// <summary>
        /// The Z coordinate (quasi-equal to blue stimulation/S cone response).
        /// <remarks>Specified in fixed point 2.30 format. The upper 2 bits are the unsigned integer value. The lower 30 bits are the fractional part.</remarks>
        /// </summary>
        /// <seealso href="https://en.wikipedia.org/wiki/CIE_1931_color_space">See this Wikipedia link for more information.</seealso>
        public uint Z;
    }
}
