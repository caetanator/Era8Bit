
using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures
{
    /// <summary>
    /// This is the IBM OS/2 BMP v2 (and above) Bitmap Recording Algorithm used.
    /// <para>Supported since IBM OS/2 2.0.</para>
    /// <seealso href="http://www.fileformat.info/format/os2bmp/egff.htm">See this FileFormat link for more information.</seealso>
    /// <seealso href="https://en.wikipedia.org/wiki/BMP_file_format">See this Wikipedia link for more information.</seealso>
    /// </summary>
    internal enum EnumOS2EnumOS2RecordingAlgorithm : ushort
    {
        /// <summary>
        /// OS/2 BMP_DIB_HeaderV2.usRecording, Bitmap Recording Algorithm, only BMP Bottom-Up is allowed.
        /// Same as <c>BRA_BOTTOMUP</c> macro of the OS/2 2.0 and above SDK.
        /// </summary>
        BottomUp = 0
    }
}
