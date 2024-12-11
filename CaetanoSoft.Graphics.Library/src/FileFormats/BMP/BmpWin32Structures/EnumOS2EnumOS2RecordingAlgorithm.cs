
using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures
{
    /// <summary>
    /// This is the IBM OS/2 2.x (and above) BMP v2 Bitmap Recording Algorithm used to store the bitmap data (pixels).
    /// For use in <c>OS22XBITMAPHEADER.usRecording</c> field.
    /// <para>Supported since IBM OS/2 2.0.</para>
    /// <seealso href="http://www.fileformat.info/format/os2bmp/egff.htm">See this FileFormat link for more information.</seealso>
    /// <seealso href="https://en.wikipedia.org/wiki/BMP_file_format">See this Wikipedia link for additional information.</seealso>
    /// </summary>
    internal enum EnumOS2EnumOS2RecordingAlgorithm : ushort
    {
        /// <summary>
        /// Bottom-up bitmap data (pixels). Only BMP Bottom-Up is allowed.
        /// Same as <c>BRA_BOTTOMUP</c> macro of the OS/2 2.0 and above SDK.
        /// </summary>
        BottomUp = 0
    }
}
