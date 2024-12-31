/**
 * RGB24.cs
 *
 * PURPOSE
 *  This structure encapsulates the properties and methods needed to represents a packed 24-bit (3 unsigned bytes) 
 *  pixel with three individual 8-bit (1 unsigned byte) values ranging from 0 to 255.
 *  
 *  The color components are stored in Red, Green, Blue order.
 *
 * CONTACTS
 *  For any question or bug report, regarding any portion of the "CaetanoSoft.Graphics.PixelFormats" project:
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
    /// This structure encapsulates the properties and methods needed to represents a packed 24-bit (3 unsigned bytes) 
    /// pixel with three individual 8-bit(1 unsigned byte) values ranging from 0 to 255.
    /// <para>The color components are stored in RGB (Red, Green, Blue) order.</para>
    /// <para>When possible, it uses the <c>Unsafe</c> class to optimize the methods for speed and low memory usage.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 3)]
    public struct RGB24 : IPixel<RGB24>
    {
        // ** Properties

        /// <summary>
        /// The red component property.
        /// </summary>
        public byte R;

        /// <summary>
        /// The green component property.
        /// </summary>
        public byte G;

        /// <summary>
        /// The blue component property.
        /// </summary>
        public byte B;

        // ** Fields

        /// <summary>
        /// A <see cref="Vector3"/> with the maximum integer value that a <see cref="byte"/> can contain (aka. 255), 
        /// used to convert the RGB color pixel components from unsigned byte [0, 255] to float [0.0, 1.0], and vice versa.
        /// </summary>
        private static readonly Vector3 vector3MaxByteValue = new (255.0F);

        /// <summary>
        /// A <see cref="Vector3"/> with the delta error, used round an RGB color pixel components from float [0.0, 1.0] to 
        /// unsigned byte [0, 255], and vice versa.
        /// </summary>
        private static readonly Vector3 vector3DeltaError = new (0.5F);

        // ** Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RGB24"/> struct.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RGB24(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        // ** Override methods of Object

        /// <summary>
        /// Gets a string representation of the packed vector.
        /// <para>The output RGB format is: <c>(n.ff, n.ff, n.ff)</c>, where <c>n</c> is in the interval [0, 1] and 
        /// <c>ff</c> is in the interval [00, 99].</para>
        /// </summary>
        /// <returns>A string representation of the packed vector.</returns>
        public override string ToString()
        {
            return this.ToVector3().ToString();
        }

        /// <summary>Generates a hash code for this instance, from the RGB color components, to insure that 2 RGB24 
        /// objects that represent the same color are report <c>true</c> by <c>IEquatable&lt;RGB24&gt;.Equals(object)</c>.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures 
        /// like a hash table.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.R;
                hashCode = (hashCode * 397) ^ this.G;
                hashCode = (hashCode * 397) ^ this.B;
                return hashCode;
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(RGB24 left, RGB24 right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(RGB24 left, RGB24 right)
        {
            return !(left == right);
        }

        // ** Override methods of IEquatable<RGB24>

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(RGB24 other)
        {
            return this.R == other.R && this.G == other.G && this.B == other.B;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj?.GetType() == typeof(RGB24) && this.Equals((RGB24)obj);
        }

        // ** Override methods of IPixel<RGB24>

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromRgba32(RGBA32 source)
        {
            this = Unsafe.As<RGBA32, RGB24>(ref source);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromVector3(Vector3 vector)
        {
            // Scale the vector
            vector *= vector3MaxByteValue;
            // Add rounding error
            vector += vector3DeltaError;
            // Clamp to [0.0, 255.0]
            vector = Vector3.Clamp(vector, Vector3.Zero, vector3MaxByteValue);

            this.R = (byte)vector.X;
            this.G = (byte)vector.Y;
            this.B = (byte)vector.Z;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromVector4(Vector4 vector)
        {
            var vector3 = new Vector3(vector.X, vector.Y, vector.Z);
            this.PackFromVector3(vector3);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToRgb24(ref RGB24 dest)
        {
            dest = this;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToBgr24(ref BGR24 dest)
        {
            dest.R = this.R;
            dest.G = this.G;
            dest.B = this.B;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToRgba32(ref RGBA32 dest)
        {
            dest.Rgb = this;
            dest.A = 255;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToBgra32(ref BGRA32 dest)
        {
            dest.B = this.B;
            dest.G = this.G;
            dest.R = this.R;
            dest.A = 255;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToArgb32(ref ARGB32 dest)
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
            // Not clamped because float type as enough resolution to guaranty that all the components
            // in the vector are in the range 0.0 to 1.0
            return new Vector3(this.R, this.G, this.B) / vector3MaxByteValue;
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
            uint hex = (uint)(this.B | (uint)(this.G << 8) | (uint)(this.R << 16));
            return "#" + hex.ToString("X6");
        }

        // ** Methods

        /// <summary>
        /// Gets the value of this struct as <see cref="BGR24"/>.
        /// </summary>
        /// <returns>A <see cref="BGR24"/> value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BGR24 ToBgr24()
        {
            return new BGR24(this.R, this.G, this.B);
        }
    }
}
