
using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures
{
    /// <summary>
    /// This is the IBM OS/2 2.x (and above) BMP v2 Bitmap Color Encoding model used to describe the stored bitmap data (pixels).
    /// For use in <c>OS22XBITMAPHEADER.ulColorEncoding</c> field.
    /// <para>Supported since IBM OS/2 2.0.</para>
    /// <seealso href="http://www.fileformat.info/format/os2bmp/egff.htm">See this FileFormat link for more information.</seealso>
    /// <seealso href="https://en.wikipedia.org/wiki/BMP_file_format">See this Wikipedia link for additional information.</seealso>
    /// </summary>
    internal enum EnumOS2EnumOS2ColorEncoding : ushort
    {
        /// <summary>
        /// RGB color encoding scheme. Only RGB is allowed.
        /// Same as <c>BCE_RGB</c> macro of the OS/2 2.0 and above SDK.
        /// </summary>
        RGB = 0,

        /// <summary>
        /// Palette color encoding scheme. This value is not allowed nor supported.
        /// Same as <c>BCE_PALETTE</c> macro of the OS/2 2.0 and above SDK.
        /// </summary>
        Palette = 0xFFFF
    }
}
