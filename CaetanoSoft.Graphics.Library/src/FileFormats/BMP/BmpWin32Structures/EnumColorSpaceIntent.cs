/**
 * EnumColorSpaceIntent.cs
 *
 * PURPOSE
 *  This enumeration contains all possible Color Space Intent values for Microsoft Windows BMP v5 bitmaps.
 *  Introduced in Microsoft's Windows 2000.
 *
 * CONTACTS
 *  For any question or bug report, regarding any portion of the "CaetanoSoft.Graphics.FileFormats.BMP.BmpWin32Structures" project:
 *      https://github.com/caetanator/Era8Bit
 *
 * COPYRIGHT
 *  This file is distributed under the terms of the GNU General Public
 *  License (GPL) v3. Copies of the GPL can be obtained from:
 *      ftp://prep.ai.mit.edu/pub/gnu/GPL
 *  Each contributing author retains all rights to their own work.
 *
 *  (C)2009-2024 José Caetano Silva
 *
 * HISTORY
 *  2009-09-15: Created.
 *  2017-04-13: Major rewrite.
 *  2023-09-16: Renamed and updated.
 *  2024-12-10: More documentation updates.
 */

using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.FileFormats.BMP.Win32Structures
{
    /// <summary>
    /// The rendering intent used on the Microsoft Windows BMP v5 (and later versions) image DIB or file.
    /// <seealso href="https://msdn.microsoft.com/en-us/library/dd183381(v=vs.85).aspx">See this MSDN link for more information.</seealso>
    /// </summary>
    internal enum EnumColorSpaceIntent : uint
    {
        // ** Values for Microsoft Windows BMP v5

        /// <summary>
        /// Saturation. Maintains saturation. Same as <c>LCS_GM_BUSINESS</c> in 
        /// Windows BMP v5 SDK.
        /// Used for business charts and other situations in which undithered colors are required.
        /// </summary>
        Business = 1,

        /// <summary>
        /// Relative Colorimetric. Maintains colorimetric match.
        /// Used for graphic designs and named colors. Same as <c>LCS_GM_GRAPHICS</c> in 
        /// Windows BMP v5 SDK.
        /// </summary>
        Graphics = 2,

        /// <summary>
        /// Perceptual. Maintains contrast.
        /// Used for photographs and natural images. Same as <c>LCS_GM_IMAGES</c> in 
        /// Windows BMP v5 SDK.
        /// </summary>
        Images = 4,

        /// <summary>
        /// Absolute Colorimetric. Maintains the white point.
        /// Matches the colors to their nearest color in the destination gamut. 
        /// Same as <c>LCS_GM_ABS_COLORIMETRIC</c> in Windows BMP v5 SDK.
        /// </summary>
        AbsoluteColoriMetric = 8
    }
}
