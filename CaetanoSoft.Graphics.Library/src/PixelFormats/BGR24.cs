/**
 * BGR24.cs
 *
 * PURPOSE
 *  This structure encapsulates the properties and methods needed to represents a packed 24-bit (3 unsigned bytes) pixel with 
 *  three individual 8-bit (1 unsigned byte) values ranging from 0 to 255.
 *  
 *  The color components are stored in Blue, Green, Red order.
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
 *  (C)2019-2024 José Caetano Silva
 *
 * HISTORY
 *  2019-09-16: Created.
 *  2024-12-10: Renamed and updated.
 */

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CaetanoSoft.Graphics.PixelFormats
{

    /// <summary>
    /// This structure encapsulates the properties and methods needed to represents a packed 24-bit (3 unsigned bytes) pixel with  
    /// three individual 8-bit (1 unsigned byte) values ranging from 0 to 255.
    /// <para>The color components are stored in BGR (Blue, Green, Red) order.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 12)]
    public struct BGR24 : IPixel<BGR24>
    {
        // ** Properties

        /// <summary>
        /// The blue component property.
        /// </summary>
        public byte B;

        /// <summary>
        /// The green component property.
        /// </summary>
        public byte G;

        /// <summary>
        /// The red component property.
        /// </summary>
        public byte R;

        // ** Constructors

        /// <summary>Initializes a new instance of the <see cref="BGR24" /> struct.</summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BGR24(byte r, byte g, byte b)
        {
            this.G = g;
            this.B = b;
            this.R = r;
        }

        // ** Override methods of Object

        /// <inheritdoc/>
        public override string ToString()
        {
            return new RGB24(this.R, this.G, this.B).ToString();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            // Generates the hash code from the BGR color components, to insure that 2 BGR24 objects 
            // that represent the same color are report true by IEquatable<BGR24>.Equals(object)
            unchecked
            {
                int hashCode = this.B;
                hashCode = (hashCode * 397) ^ this.G;
                hashCode = (hashCode * 397) ^ this.R;
                return hashCode;
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BGR24 left, BGR24 right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BGR24 left, BGR24 right)
        {
            return !(left == right);
        }

        // ** Override methods of IEquatable<BGR24>

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BGR24 other)
        {
            return this.R == other.R && this.G == other.G && this.B == other.B;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj?.GetType() == typeof(BGR24) && this.Equals((BGR24)obj);
        }

        // ** Override methods of IPixel<RGB24>

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromRgba32(RGBA32 source)
        {
            this = Unsafe.As<RGBA32, BGR24>(ref source);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromVector3(Vector3 vector)
        {
            var rgb = default(RGB24);
            rgb.PackFromVector3(vector);
            rgb.ToBgr24(ref this);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromVector4(Vector4 vector)
        {
            var rgb = default(RGB24);
            rgb.PackFromVector4(vector);
            rgb.ToBgr24(ref this);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToRgb24(ref RGB24 dest)
        {
            dest.R = this.R;
            dest.G = this.G;
            dest.B = this.B;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToBgr24(ref BGR24 dest)
        {
            dest = this;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToRgba32(ref RGBA32 dest)
        {
            dest.R = this.R;
            dest.B = this.B;
            dest.G = this.G;
            dest.A = 255;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToBgra32(ref BGRA32 dest)
        {
            dest.Bgr = this;
            dest.A = 255;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToArgba32(ref ARGB32 dest)
        {
            dest.A = 255;
            dest.R = this.R;
            dest.G = this.G;
            dest.B = this.B;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 ToVector3()
        {
            return new RGB24(this.R, this.G, this.B).ToVector3();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4 ToVector4()
        {
            return new RGBA32(this.R, this.G, this.B, 255).ToVector4();
        }

        /// <inheritdoc/>
        public string ToHex()
        {
            return new RGB24(this.R, this.G, this.B).ToHex();
        }
    }
}
