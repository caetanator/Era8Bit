
using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures
{
    /// <summary>
    /// The color space used on the Microsoft Windows BMP v4 (and later versions) image DIB or file.
    /// <seealso href="https://msdn.microsoft.com/en-us/library/dd183381(v=vs.85).aspx">See this MSDN link for more information.</seealso>
    /// </summary>
    internal enum EnumColorSpace : uint
    {
        // ** Values for Microsoft Windows BMP v4

        /// <summary>
        /// The endpoints and gamma values are given in the appropriate fields.
        /// <para>
        /// Equals 0x00000000 (in hexadecimal).
        /// </para>
        /// </summary>
        Calibrated_RGB = 0x00000000,

        /// <summary>
        /// Use destination device RGB. The endpoints and gamma values fields, are ignored.
        /// <para>
        /// Equals 0x00000001 (in hexadecimal).
        /// </para>
        /// </summary>
        Device_RGB = 0x00000001,

        /// <summary>
        /// Use destination device CMYK. The endpoints and gamma values fields, are ignored.
        /// <para>
        /// Equals 0x00000001 (in hexadecimal).
        /// </para>
        /// </summary>
        Device_CMYK = 0x00000002,

        /// <summary>
        /// The bitmap is in sRGB color space.
        /// <para>
        /// Equals 'sRGB' (in ASCII) or 0x73524742 (in hexadecimal).
        /// </para>
        /// </summary>
        SRGB = 0x73524742,

        /// <summary>
        /// The bitmap is in the system default color space: sRGB.
        /// <para>
        /// Equals 'Win ' (in ASCII) or 0x57696E20 (in hexadecimal).
        /// </para>
        /// </summary>
        WindowsColorSpace = 0x57696E20,

        // ** Values for Microsoft Windows BMP v5

        /// <summary>
        /// This value indicates that <c>ProfileOffset</c> points to the ICC color space file name (in ASCII Code Page 1252) of the profile to use (gamma and endpoints values are ignored).
        /// <para>
        /// Equals 'LINK' (in ASCII) or 0x4C494E4B (in hexadecimal).
        /// </para>
        /// </summary>
        ProfileLinked = 0x4C494E4B,

        /// <summary>
        /// Indicates that <c>ProfileOffset</c> points to a memory buffer that contains the profile to be used (gamma and endpoints values are ignored).
        /// <para>
        /// Equals 'MBED' (in ASCII) or 0x4D424544 (in hexadecimal).
        /// </para>
        /// </summary>
        ProfileEmbedded = 0x4D424544
    }
}
