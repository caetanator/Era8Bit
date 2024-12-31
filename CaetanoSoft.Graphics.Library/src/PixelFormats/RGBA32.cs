/**
 * RGBA32.cs
 *
 * PURPOSE
 *  This structure encapsulates the properties and methods needed to represents a packed 32-bit (4 unsigned bytes) pixel 
 *  with four individual 8-bit (1 unsigned byte) values ranging from 0 to 255.
 *  
 *  The color components are stored in Red, Green, Blue, Alpha order.
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
    /// This structure encapsulates the properties and methods needed to represents a packed 32-bit (4 unsigned bytes) pixel with  
    /// three individual 8-bit (1 unsigned byte) values ranging from 0 to 255.
    /// <para>The color components are stored in RGBA (Red, Green, Blue, Alpha) order.</para>
    /// <para>When possible, it uses the <c>Unsafe</c> class to optimize the methods for speed and low memory usage.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct RGBA32 : IPixel<RGBA32>
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

        /// <summary>
        /// The alpha component property.
        /// </summary>
        public byte A;

        // ** Fields

        /// <summary>
        /// A <see cref="Vector4"/> with the maximum integer value that a <see cref="byte"/> can contain (aka. 255), 
        /// used to convert the RGB color pixel components from unsigned byte [0, 255] to float [0.0, 1.0], and vice versa.
        /// </summary>
        private static readonly Vector4 vector4MaxByteValue = new (255.0F);

        /// <summary>
        /// A <see cref="Vector4"/> with the delta error, used round an RGB color pixel components from float [0.0, 1.0] to 
        /// unsigned byte [0, 255], and vice versa.
        /// </summary>
        private static readonly Vector4 vector4DeltaError = new (0.5F);

        /// <summary>
        /// The shift count for the red component
        /// </summary>
        private const int RedShift = 0;

        /// <summary>
        /// The shift count for the green component
        /// </summary>
        private const int GreenShift = 8;

        /// <summary>
        /// The shift count for the blue component
        /// </summary>
        private const int BlueShift = 16;

        /// <summary>
        /// The shift count for the alpha component
        /// </summary>
        private const int AlphaShift = 24;

        // ** Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBA32"/> struct.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RGBA32(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = 255;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBA32"/> struct.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RGBA32(byte r, byte g, byte b, byte a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBA32"/> struct.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RGBA32(float r, float g, float b, float a = 1) : this()
        {
            this.Pack(r, g, b, a);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBA32"/> struct.
        /// </summary>
        /// <param name="vector">
        /// The vector containing the components for the packed vector.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RGBA32(Vector3 vector) : this()
        {
            this.Pack(ref vector);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBA32"/> struct.
        /// </summary>
        /// <param name="vector">
        /// The vector containing the components for the packed vector.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RGBA32(Vector4 vector) : this()
        {
            this = RGBA32.PackToRGBA32(vector);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBA32"/> struct.
        /// </summary>
        /// <param name="packed">
        /// The packed value.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RGBA32(uint packed) : this()
        {
            this.Rgba = packed;
        }

        /// <summary>
        /// Gets or sets the packed representation of the RGBA32 struct.
        /// </summary>
        public uint Rgba
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Unsafe.As<RGBA32, uint>(ref this);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Unsafe.As<RGBA32, uint>(ref this) = value;
            }
        }

        /// <summary>
        /// Gets or sets the RGB components of this struct as <see cref="RGB24"/>
        /// </summary>
        public RGB24 Rgb
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Unsafe.As<RGBA32, RGB24>(ref this);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Unsafe.As<RGBA32, RGB24>(ref this) = value;
            }
        }

        /// <summary>
        /// Gets or sets the RGB components of this struct as <see cref="BGR24"/> reverting the component order.
        /// </summary>
        public BGR24 Bgr
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new BGR24(this.R, this.G, this.B);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                this.R = value.R;
                this.G = value.G;
                this.B = value.B;
            }
        }

        // ** Override methods of Object

        /// <summary>
        /// Gets a string representation of the packed vector.
        /// <para>The output RGBA format is: <c>(n.ff, n.ff, n.ff, n.ff)</c>, where <c>n</c> is in the interval [0, 1] and 
        /// <c>ff</c> is in the interval [00, 99].</para>
        /// </summary>
        /// <returns>A string representation of the packed vector.</returns>
        public override string ToString()
        {
            return this.ToVector4().ToString();
        }

        /// <summary>Generates a hash code for this instance, from the RGBA color components, to insure that 2 RGBA32 
        /// objects that represent the same color are report <c>true</c> by <c>IEquatable&lt;RGBA32&gt;.Equals(object)</c>.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures 
        /// like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.R;
                hashCode = (hashCode * 397) ^ this.G;
                hashCode = (hashCode * 397) ^ this.B;
                hashCode = (hashCode * 397) ^ this.A;
                return hashCode;
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(RGBA32 left, RGBA32 right)
        {
            return left.Rgba == right.Rgba;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(RGBA32 left, RGBA32 right)
        {
            return left.Rgba != right.Rgba;
        }

        // ** Override methods of IEquatable<RGBA32>

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(RGBA32 other)
        {
            return this.Rgba == other.Rgba;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj?.GetType() == typeof(RGBA32) && this.Equals((RGBA32)obj);
        }

        // ** Override methods of IPixel<RGBA32>

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromRgba32(RGBA32 source)
        {
            this = source;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromVector3(Vector3 vector)
        {
            this.Pack(ref vector);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromVector4(Vector4 vector)
        {
            this.Pack(ref vector);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToRgb24(ref RGB24 dest)
        {
            dest = Unsafe.As<RGBA32, RGB24>(ref this);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToBgr24(ref BGR24 dest)
        {
            dest.R = this.R;
            dest.G = this.G;
            dest.B = this.B;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToRgba32(ref RGBA32 dest)
        {
            dest = this;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToBgra32(ref BGRA32 dest)
        {
            dest.B = this.B;
            dest.G = this.G;
            dest.R = this.R;
            dest.A = this.A;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToArgb32(ref ARGB32 dest)
        {
            dest.A = this.A;
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
            // Not clamped because float type as enough resolution to guaranty that all the components
            // in the vector are in the range 0.0 to 1.0
            return new Vector4(this.R, this.G, this.B, this.A) / vector4MaxByteValue;
        }

        /// <inheritdoc/>
        public string ToHex()
        {
            uint hexOrder = PackToUInt(this.A, this.B, this.G, this.R);
            return hexOrder.ToString("X8");
        }

        // ** Methods

        /// <summary>
        /// Gets the value of this struct as <see cref="BGRA32"/>.
        /// </summary>
        /// <returns>A <see cref="BGRA32"/> value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BGRA32 ToBgra32()
        {
            return new BGRA32(this.R, this.G, this.B, this.A);
        }

        /// <summary>
        /// Gets the <see cref="Vector4"/> representation without normalizing to 
        /// the interval [0.00, 1.00].
        /// </summary>
        /// <returns>A <see cref="Vector4"/> of values in the interval [0, 255].</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Vector4 ToUnscaledVector4()
        {
            return new Vector4((float)this.R, (float)this.G, (float)this.B, (float)this.A);
        }

        /// <summary>
        /// Packs four normalized floats ([0.00, 1.00]) into this instance.
        /// </summary>
        /// <param name="x">The x-component.</param>
        /// <param name="y">The y-component.</param>
        /// <param name="z">The z-component.</param>
        /// <param name="w">The w-component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Pack(float x, float y, float z, float w)
        {
            var value = new Vector4(x, y, z, w);
            this.Pack(ref value);
        }

        /// <summary>
        /// Packs a normalized ([0.00, 1.00]) <see cref="Vector3"/> into this instance.
        /// </summary>
        /// <param name="vector">The vector containing the values to pack.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Pack(ref Vector3 vector)
        {
            var value = new Vector4(vector, 1.0f);
            this.Pack(ref value);
        }

        /// <summary>
        /// Packs a normalized ([0.00, 1.00]) <see cref="Vector4"/> into this instance.
        /// </summary>
        /// <param name="vector">The vector containing the values to pack.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Pack(ref Vector4 vector)
        {
            vector *= vector4MaxByteValue;
            vector += vector4DeltaError;
            vector = Vector4.Clamp(vector, Vector4.Zero, vector4MaxByteValue);

            this.R = (byte)vector.X;
            this.G = (byte)vector.Y;
            this.B = (byte)vector.Z;
            this.A = (byte)vector.W;
        }

        /// <summary>
        /// Packs four bytes ([0, 255]) into a new instance of <c>uint</c>.
        /// </summary>
        /// <param name="x">The x-component.</param>
        /// <param name="y">The y-component.</param>
        /// <param name="z">The z-component.</param>
        /// <param name="w">The w-component.</param>
        /// <returns>The packet color value in an <see cref="uint"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint PackToUInt(byte x, byte y, byte z, byte w)
        {
            return (uint)(x << RedShift | y << GreenShift | z << BlueShift | w << AlphaShift);
        }

        /// <summary>
        /// Packs a normalized ([0.00, 1.00]) <see cref="Vector4"/> into a new instance of <c>RGBA32</c>.
        /// </summary>
        /// <param name="vector">The vector containing the values to pack.</param>
        /// <returns>The <see cref="RGBA32"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static RGBA32 PackToRGBA32(Vector4 vector)
        {
            vector *= vector4MaxByteValue;
            vector += vector4DeltaError;
            vector = Vector4.Clamp(vector, Vector4.Zero, vector4MaxByteValue);

            return new RGBA32((byte)vector.X, (byte)vector.Y, (byte)vector.Z, (byte)vector.W);
        }
    }
}
