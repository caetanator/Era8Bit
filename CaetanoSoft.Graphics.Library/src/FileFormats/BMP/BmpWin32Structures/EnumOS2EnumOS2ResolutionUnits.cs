
using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures
{
    /// <summary>
    /// This is the IBM OS/2 BMP v2 (and above) metric unit used for Resolution.
    /// <para>Supported since IBM OS/2 2.0.</para>
    /// <seealso href="http://www.fileformat.info/format/os2bmp/egff.htm">See this FileFormat link for more information.</seealso>
    /// <seealso href="https://en.wikipedia.org/wiki/BMP_file_format">See this Wikipedia link for more information.</seealso>
    /// </summary>
    internal enum EnumOS2EnumOS2ResolutionUnits : ushort
    {
        /// <summary>
        /// OS/2 BMP_DIB_HeaderV2.usUnits, metric unit used in BMP_DIB_HeaderV2.cxResolution 
        /// and BMP_DIB_HeaderV2.cyResolution, only ppm (Pixels-per-Meter) is allowed. 
        /// Same as <c>BRU_METRIC</c> macro of the OS/2 2.0 and above SDK.
        /// </summary>
        PPM = 0
    }
}
