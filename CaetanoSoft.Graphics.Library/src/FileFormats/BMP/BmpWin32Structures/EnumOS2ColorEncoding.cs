
using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures
{
    /// <summary>
    /// This is the IBM OS/2 BMP v2 (and above) color model used to describe the bitmap data.
    /// <para>Supported since IBM OS/2 2.0.</para>
    /// <seealso href="http://www.fileformat.info/format/os2bmp/egff.htm">See this FileFormat link for more information.</seealso>
    /// <seealso href="https://en.wikipedia.org/wiki/BMP_file_format">See this Wikipedia link for more information.</seealso>
    /// </summary>
    internal enum EnumOS2EnumOS2ColorEncoding : ushort
    {
        /// <summary>
        /// OS/2 BMP_DIB_HeaderV2.ulColorEncoding, RGB color encoding scheme, only RGB is allowed.
        /// Same as <c>BCE_RGB</c> macro of the OS/2 2.0 and above SDK.
        /// </summary>
        RGB = 0,

        /// <summary>
        /// OS/2 BMP_DIB_HeaderV2.ulColorEncoding, Palette color encoding scheme is not allowed.
        /// Same as <c>BCE_PALETTE</c> macro of the OS/2 2.0 and above SDK.
        /// </summary>
        Palette = 0xFFFF
    }
}
