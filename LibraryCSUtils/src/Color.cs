/**
 * Color.cs
 *
 * PURPOSE
 *  Implements the functionality necessary to handle colors.
 *
 * CONTACTS
 *  E-mail regarding any portion of the "MonkeyRT" project:
 *      caetanator@hotmail.com
 *
 * COPYRIGHT
 *  This file is distributed under the terms of the GNU General Public
 *  License (GPL) v3. Copies of the GPL can be obtained from:
 *      ftp://prep.ai.mit.edu/pub/gnu/GPL
 *  Each contributing author retains all rights to their own work.
 *
 *  (C) 2006-2007   José Caetano Silva
 *
 * HISTORY
 *  2006-02-01: Created.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace CaetanoSof.Utils.Graphics
{
    /// <summary>
    /// Represents color by RGBA (Red, Green, Blue and Alpha) values.
    /// </summary>
    public class Color
    {
        // Private members
        private double m_red = 0.0;
        private double m_green = 0.0;
        private double m_blue = 0.0;
        private double m_alpha = 1.0;

        // Class constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <remarks>
        /// Default values are 1.0 for Alpha and 0.0 for the RGB color channels.
        /// </remarks>
        public Color()
        {
            m_red = 0.0;
            m_green = 0.0;
            m_blue = 0.0;
            m_alpha = 1.0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="r">Red channel value.</param>
        /// <param name="g">Green channel value.</param>
        /// <param name="b">Blue channel value.</param>
        /// <remarks>
        /// RGB color channels values must be a real between [0.0; 1.0].
        /// The Alpha channel value is set to 1.0.
        /// </remarks>
        public Color(double r, double g, double b)
        {
            Red = r;
            Green = g;
            Blue = b;
            m_alpha = 1.0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="r">Red channel value.</param>
        /// <param name="g">Green channel value.</param>
        /// <param name="b">Blue channel value.</param>
        /// <param name="a">Alpha channel value.</param>
        /// <remarks>
        /// The Alpha channel and RGB color channels values must be a real between [0.0; 1.0].
        /// </remarks>
        public Color(double r, double g, double b, double a)
        {
            Red = r;
            Green = g;
            Blue = b;
            Alpha = a;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="r">Red channel value.</param>
        /// <param name="g">Green channel value.</param>
        /// <param name="b">Blue channel value.</param>
        /// <remarks>
        /// RGB color channels values must be an integer between [0; 255].
        /// The Alpha channel value is set to 255.
        /// </remarks>
        public Color(int r, int g, int b)
        {
            intRed = (int)((double)r / 255.0);
            intGreen = (int)((double)g / 255.0);
            intBlue = (int)((double)b / 255.0);
            m_alpha = 1.0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="r">Red channel value.</param>
        /// <param name="g">Green channel value.</param>
        /// <param name="b">Blue channel value.</param>
        /// <param name="a">Alpha channel value.</param>
        /// <remarks>
        /// The Alpha channel and RGB color channels values must be an integer between [0; 255].
        /// </remarks>
        public Color(int r, int g, int b, int a)
        {
            intRed = (int)((double)r / 255.0);
            intGreen = (int)((double)g / 255.0);
            intBlue = (int)((double)b / 255.0);
            intAlpha = (int)((double)a / 255.0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> class.
        /// </summary>
        /// <param name="color">The Color class RGBA to be copyed.</param>
        public Color(Color color)
        {
            m_red = color.Red;
            m_green = color.Green;
            m_blue = color.Blue;
            m_alpha = color.Alpha;
        }

        // Public properties

        /// <summary>
        /// The <b>Red</b> property represents the <i>red</i> component of the RGBA color.
        /// </summary>
        /// <remarks>
        /// <b>Red</b> must be between [0.0; 1.0].
        /// </remarks>
        /// <value>
        /// The <b>Red</b> property gets/sets the <i>red</i> component of the RGBA color.
        /// </value>
        /// <exception cref="System.ArgumentException">
        /// Throws <b>ArgumentException</b> when the value is not valid.
        /// </exception>
        public double Red
        {
            get { return m_red; }
            set { 
                    if (value < 0.0 || value > 1.0)
                        throw new ArgumentException("Red value must be between 0.0 and 1.0!");
                    m_red = value;
                }
        }

        /// <summary>
        /// The <b>Green</b> property represents the <i>green</i> component of the RGBA color.
        /// </summary>
        /// <remarks>
        /// <b>Green</b> must be between [0.0; 1.0].
        /// </remarks>
        /// <value>
        /// The <b>Green</b> property gets/sets the <i>green</i> component of the RGBA color.
        /// </value>
        /// <exception cref="System.ArgumentException">
        /// Throws <b>ArgumentException</b> when the value is not valid.
        /// </exception>
        public double Green
        {
            get { return m_green; }
            set {
                    if (value < 0.0 || value > 1.0)
                        throw new ArgumentException("Green value must be between 0.0 and 1.0!");
                    m_green = value;
                }
        }

        /// <summary>
        /// The <b>Blue</b> property represents the <i>blue</i> component of the RGBA color.
        /// </summary>
        /// <remarks>
        /// <b>Blue</b> must be between [0.0; 1.0].
        /// </remarks>
        /// <value>
        /// The <b>Blue</b> property gets/sets the <i>blue</i> component of the RGBA color.
        /// </value>
        /// <exception cref="System.ArgumentException">
        /// Throws <b>ArgumentException</b> when the value is not valid.
        /// </exception>
        public double Blue
        {
            get { return m_blue; }
            set {
                    if (value < 0.0 || value > 1.0)
                        throw new ArgumentException("Blue value must be between 0.0 and 1.0!");
                    m_blue = value;
                }
        }

        /// <summary>
        /// The <b>Alpha</b> property represents the <i>alpha</i> component of the RGBA color.
        /// </summary>
        /// <remarks>
        /// <b>Alpha</b> must be between [0.0; 1.0] (0.0 => 100% transparent; 1.0 => 100% opaque).
        /// </remarks>
        /// <value>
        /// The <b>Alpha</b> property gets/sets the <i>alpha</i> component of the RGBA color.
        /// </value>
        /// <exception cref="System.ArgumentException">
        /// Throws <b>ArgumentException</b> when the value is not valid.
        /// </exception>
        public double Alpha
        {
            get { return m_alpha; }
            set
            {
                if (value < 0.0 || value > 1.0)
                    throw new ArgumentException("Alpha value must be between 0.0 and 1.0!");
                m_alpha = value;
            }
        }

        /// <summary>
        /// The <b>intRed</b> property represents the <i>red</i> component of the RGBA color, as an integer.
        /// </summary>
        /// <remarks>
        /// <b>intRed</b> must be between [0; 255].
        /// </remarks>
        /// <value>
        /// The <b>intRed</b> property gets/sets the <i>red</i> component of the RGBA color.
        /// </value>
        /// <exception cref="System.ArgumentException">
        /// Throws <b>ArgumentException</b> when the value is not valid.
        /// </exception>
        public int intRed
        {
            get { return (int)(255.0 * m_red); }
            set {
                    if (value < 0 || value > 255)
                        throw new ArgumentException("Red value must be between 0 and 255!");
                    m_red = (double)value / 255.0;
                }
        }

        /// <summary>
        /// The <b>intGreen</b> property represents the <i>green</i> component of the RGBA color, as an integer.
        /// </summary>
        /// <remarks>
        /// <b>intGreen</b> must be between [0; 255].
        /// </remarks>
        /// <value>
        /// The <b>intGreen</b> property gets/sets the <i>green</i> component of the RGBA color.
        /// </value>
        /// <exception cref="System.ArgumentException">
        /// Throws <b>ArgumentException</b> when the value is not valid.
        /// </exception>
        public int intGreen
        {
            get { return (int)(255.0 * m_green); }
            set {
                    if (value < 0 || value > 255)
                        throw new ArgumentException("Green value must be between 0 and 255!");
                    m_green = (double)value / 255.0;
                }
        }

        /// <summary>
        /// The <b>intBlue</b> property represents the <i>blue</i> component of the RGBA color, as an integer.
        /// </summary>
        /// <remarks>
        /// <b>intBlue</b> must be between [0; 255].
        /// </remarks>
        /// <value>
        /// The <b>intBlue</b> property gets/sets the <i>blue</i> component of the RGBA color.
        /// </value>
        /// <exception cref="System.ArgumentException">
        /// Throws <b>ArgumentException</b> when the value is not valid.
        /// </exception>
        public int intBlue
        {
            get { return (int)(255.0 * m_blue); }
            set {
                    if (value < 0 || value > 255)
                        throw new ArgumentException("Blue value must be between 0 and 255!");
                    m_blue = (double)value / 255.0;
                }
        }

        /// <summary>
        /// The <b>intAlpha</b> property represents the <i>blue</i> component of the RGBA color, as an integer.
        /// </summary>
        /// <remarks>
        /// <b>intAlpha</b> must be between [0; 255] (0 => 100% transparent; 255 => 100% opaque).
        /// </remarks>
        /// <value>
        /// The <b>intAlpha</b> property gets/sets the <i>blue</i> component of the RGBA color.
        /// </value>
        /// <exception cref="System.ArgumentException">
        /// Throws <b>ArgumentException</b> when the value is not valid.
        /// </exception>
        public int intAlpha
        {
            get { return (int)(255.0 * m_alpha); }
            set
            {
                if (value < 0 || value > 255)
                    throw new ArgumentException("Alpha value must be between 0 and 255!");
                m_alpha = (double)value / 255.0;
            }
        }

        // Public Methods


        /// <summary>
        /// Clamp's the RGBA values to the [0.0; 1.0] range.
        /// </summary>
        /// <remarks>
        /// Values above 1.0 are clamped to 1.0.
        /// Values below 0.0 are clamped to 0.0.
        /// </remarks>
        public void Clamp()
        {
            if (m_red < 0.0)
                m_red = 0.0;
            if (m_green < 0.0)
                m_green = 0.0;
            if (m_blue < 0.0)
                m_blue = 0.0;

            if (m_red > 1.0)
                m_red = 1.0;
            if (m_green > 1.0)
                m_green = 1.0;
            if (m_blue > 1.0)
                m_blue = 1.0;
        }

        /// <summary>
        /// Gets the color's intensity.
        /// </summary>
        /// <remarks>
        /// Intensity = (R + G + B) / 3
        /// </remarks>
        public double Intensity()
        {
            return (double)(m_red + m_green + m_blue) / 3.0;
        }

        /// <summary>
        /// Converts the color's RGB values to HSV (Hue, Saturation and Value).
        /// </summary>
        /// <param name="h">The Hue value (0..360) degrees.</param>
        /// <param name="s">The Saturation value.</param>
        /// <param name="v">The Value value.</param>
        public void ToHSV(out double h, out double s, out double v)
        {
            Clamp();

            // Finds the minimum value of the 3 RGB colors
            double min = m_red;
            if (min < m_green)
                min = m_green;
            if (min < m_blue)
                min = m_blue;
            // Finds the maximum value of the 3 RGB colors
            double max = m_red;
            if (m_green > max)
                max = m_green;
            if (m_blue > max)
                max = m_blue;

            // Set the Value
            h = 0.0;
            s = 0.0;
            v = max;

            // Calculates the Saturation
            double delta = max - min;
            if (max != 0.0)
            {
                s = delta / max;
            }
            else
            {
                // R = G = B = 0 => S = 0, H is undefined
                s = 0.0;
                h = Double.MaxValue;
                return;
            }

            // Calculates the Hue
            double rDelta = (max - m_red) / delta;
            double gDelta = (max - m_green) / delta;
            double bDelta = (max - m_blue) / delta;

            if (m_red == max)
            {
                h = bDelta - gDelta;  // Must be between yellow & magenta
            }
            else
            {
                if (m_green == max)
                {
                    h = 2.0 + (rDelta - bDelta);  // Must be between cyan & yellow
                }
                else
                {
                    if (m_blue == max)
                    {
                        h = 4.0 + (gDelta - rDelta);	// Must be between magenta & cyan
                    }
                }
            }
            // Converts the Hue to degrees
            h *= 60.0;
            if (h < 0.0)
                h += 360.0;
        }

        /// <summary>
        /// Converts the color's HSV (Hue, Saturation and Value) values to RGB.
        /// </summary>
        /// <param name="h">The Hue value.</param>
        /// <param name="s">The Saturation value.</param>
        /// <param name="v">The Value value.</param>
        /// <exception cref="System.ArgumentException">
        /// Throws <b>ArgumentException</b> when the HSV values are not valid.
        /// </exception>
        public void FromHSV(double h, double s, double v)
        {
            if (s == 0.0)
            {
                Red = v;
                Green = v;
                Blue = v;
            }
            else
            {
                if (h == 360.0)
                    h = 0.0;
                h /= 60.0;
                
                int i = (int)Math.Truncate(h);

                double f = h - (double)i;
                double p = v * (1.0 - s);
                double q = v * (1.0 - (s * f));
                double t = v * (1.0 - (1.0 - f));

                switch (i)
                {
                    case 0:
                        Red = v;
                        Green = t;
                        Blue = p;
                        break;
                    case 1:
                        Red = q;
                        Green = v;
                        Blue = p;
                        break;
                    case 2:
                        Red = p;
                        Green = v;
                        Blue = t;
                        break;
                    case 3:
                        Red = p;
                        Green = q;
                        Blue = v;
                        break;
                    case 4:
                        Red = t;
                        Green = p;
                        Blue = v;
                        break;
                    case 5:
                        Red = v;
                        Green = p;
                        Blue = q;
                        break;
                    default:
                        throw new ArgumentException("HSV color values not valid!");
                        //break;
                }
            }

            Clamp();
        }

        // Operators overloading

        /* C = C1 + C2 */
        public static Color operator +(Color C1, Color C2)
        { 
            Color C = new Color(C1.m_red + C2.m_red, C1.m_green + C2.m_green, C1.m_blue + C2.m_blue);
            return C;
        }

        /* C = C1 - C2 */
        public static Color operator -(Color C1, Color C2)
        {
            Color C = new Color(C1.m_red - C2.m_red, C1.m_green - C2.m_green, C1.m_blue - C2.m_blue);
            return C;
        }

        /* C = C1 * C2 */
        public static Color operator *(Color C1, Color C2)
        {
            Color C = new Color(C1.m_red * C2.m_red, C1.m_green * C2.m_green, C1.m_blue * C2.m_blue);
            return C;
        }

        // Object methods overrides

        /// <summary>
        /// Returns the hashcode for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return m_red.GetHashCode() ^ m_green.GetHashCode() ^ m_blue.GetHashCode() ^ m_alpha.GetHashCode();
        }

        /// <summary>
		/// Returns a value indicating whether this instance is equal to
		/// the specified object.
		/// </summary>
		/// <param name="obj">An object to compare to this instance.</param>
		/// <returns><see langword="true"/> if <paramref name="obj"/> is a <see cref="ColorF"/> and has the same values as this instance; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			Color color = obj as Color;
			if (color != null)
				return (m_red == color.Red) && (m_green == color.Green) && (m_blue == color.Blue) && (m_alpha == color.Alpha);
			else 
				return false;
		}

		/// <summary>
		/// Returns a string representation of this object.
		/// </summary>
		/// <returns>A string representation of this object.</returns>
		public override string ToString()
		{
			return string.Format("Color({0,8:F4}, {1,8:F4}, {2,8:F4}, {3,8:F4})", m_red, m_green, m_blue, m_alpha);
		}
    }
}
