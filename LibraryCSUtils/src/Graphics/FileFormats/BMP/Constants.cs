
using System.Collections.Generic;

namespace CaetanoSoft.Graphics.FileFormats.BMP
{
    /// <summary>
    /// Defines constants relating to BMPs
    /// </summary>
    internal static class Constants
    {
		/// <summary>
        /// For Windows Mobile version 5.0 and later, you can OR any of the values BI_RGB,
        /// BI_BITFIELDS and BI_ALPHABITFIELDS with <c>SourcePreRotateMask</c> to specify that the source
        /// DIB section has the same rotation angle as the destination.
        /// Otherwise, the image can be rotated 90 degrees anti-clockwise (Landscape/Portrait).
        /// https://msdn.microsoft.com/en-us/library/aa452495.aspx
        /// </summary>
        public static readonly uint SourceIsPreRotatedMask = 0x8000;

		/// <summary>
        /// For Windows Mobile version 5.0 and later, you can OR any of the values BI_RGB,
        /// BI_BITFIELDS and BI_ALPHABITFIELDS with <c>SourcePreRotateMask</c> to specify that the source
        /// DIB section has the same rotation angle as the destination.
        /// Otherwise, the image can be rotated 90 degrees anti-clockwise (Landscape/Portrait).
        /// https://msdn.microsoft.com/en-us/library/aa452495.aspx
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
