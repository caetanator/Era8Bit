using System;
using System.Collections.Generic;
using System.Text;

using ColorEntryByteRGB = CaetanoSof.Utils.Drawing.ColorEntryRGB<byte>;
using ColorEntryFloatRGB = CaetanoSof.Utils.Drawing.ColorEntryRGB<float>;
using ColorEntryByteRGBA = CaetanoSof.Utils.Drawing.ColorEntryRGBA<byte>;
using ColorEntryFloatRGBA = CaetanoSof.Utils.Drawing.ColorEntryRGBA<float>;

namespace CaetanoSoft.Graphics.FileFormats
{
    /// <summary>
    /// A template that defines a color entry for a palette in the form of RGBA.
    /// </summary>
    /// <typeparam name="T">
    /// A numeric type like byte, float, etc.
    /// </typeparam>
    public struct TColorEntryRGBA<T>
    {
        /// <summary>
        /// The red value
        /// </summary>
        public T Red;
        /// <summary>
        /// The green value
        /// </summary>
        public T Green;
        /// <summary>
        /// The blue value
        /// </summary>
        public T Blue;
        /// <summary>
        /// The alpha/transparency value
        /// </summary>
        public T Alpha;
    }
}