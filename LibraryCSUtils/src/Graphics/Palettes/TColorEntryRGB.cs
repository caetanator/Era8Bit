using System;
using System.Collections.Generic;
using System.Text;

namespace CaetanoSoft.Graphics.FileFormats
{
    /// <summary>
    /// A template that defines a color entry for a palette in the form of RGB.
    /// </summary>
    /// <copyright>(c) 2006-2017 by José Caetano Silva</copyright>
	/// <license type="GPL-3">See LICENSE for full terms</license>
    /// <typeparam name="T">
    /// A numeric type like byte, float, etc.
    /// </typeparam>
    public struct TColorEntryRGB<T>
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
    }
}