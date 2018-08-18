
using System.Collections.Generic;

namespace CaetanoSoft.Graphics.FileFormats.BMP
{
    /// <summary>
    /// Defines constants relating to BMPs
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// For Windows Mobile version 5.0 and later, you can OR BITMAPINFOHEADER.biCompression/BITMAPV4HEADER.bV4Compression/BITMAPV5HEADER.bV5Compression
        /// with <c>SourcePreRotateMask</c> to specify that the source DIB section has the same rotation angle as the destination.
        /// Otherwise, the image can be rotated 90 degrees anti-clockwise (Landscape/Portrait).
        /// 
        /// This is the same value as BI_SRCPREROTATE in the Windows CE Mobile SDK.
        /// 
        /// BITMAPV5HEADER.bV5Compression (or equivalent) must be either BI_RGB, BI_ALPHABITFIELDS or BI_BITFIELDS. Pre-roteted DIBs cannot be compressed.
        /// https://msdn.microsoft.com/en-us/library/aa452495.aspx
        /// </summary>
        public static readonly uint SourceIsPreRotatedMask = 0x8000;

        /// <summary>
        /// For Windows 95/NT and later, you can OR BITMAPINFOHEADER.biHeight/BITMAPV4HEADER.bV4Height/BITMAPV5HEADER.bV5Height
        /// with this value to see if this bitmap is a top-down DIB and its origin is the upper-left corner.
        /// 
        /// BITMAPV5HEADER.bV5Compression (or equivalent) must be either BI_RGB, BI_ALPHABITFIELDS or BI_BITFIELDS. Top-down DIBs cannot be compressed.
        /// https://docs.microsoft.com/en-us/windows/desktop/api/Wingdi/ns-wingdi-bitmapv5header
        /// </summary>
        public static readonly uint SourceIsTopDownMask = 0x80000000;
		
        /// <summary>
        /// The list of MIME types that equate to a BMP.
        /// </summary>
        public static readonly IEnumerable<string> MimeTypes = new[] { "image/bmp", "image/x-windows-bmp" };

        /// <summary>
        /// The list of file extensions that equate to a BMP.
        /// </summary>
        public static readonly IEnumerable<string> FileExtensions = new[] { "bmp", "dib" };
    }
}
