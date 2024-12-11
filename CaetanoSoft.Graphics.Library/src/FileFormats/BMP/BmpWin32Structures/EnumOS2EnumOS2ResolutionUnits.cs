
using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures
{
    /// <summary>
    /// This is the IBM OS/2 2.x (and above) BMP v2 Bitmap Resolution Metric Unit used to specify the image resolution 
    /// in <c>OS22XBITMAPHEADER.cxResolution</c> and <c>OS22XBITMAPHEADER.cyResolution</c> fields.
    /// For use in <c>OS22XBITMAPHEADER.usUnits</c> field.
    /// <para>Supported since IBM OS/2 2.0.</para>
    /// <seealso href="http://www.fileformat.info/format/os2bmp/egff.htm">See this FileFormat link for more information.</seealso>
    /// <seealso href="https://en.wikipedia.org/wiki/BMP_file_format">See this Wikipedia link for additional information.</seealso>
    /// </summary>
    internal enum EnumOS2EnumOS2ResolutionUnits : ushort
    {
        /// <summary>
        /// Pixels-per-meter (ppm), metric unit. Only ppm is allowed. 
        /// Same as <c>BRU_METRIC</c> macro of the OS/2 2.0 and above SDK.
        /// </summary>
        PPM = 0
    }
}
