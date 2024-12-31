/**
 * BGRA32.cs
 *
 * PURPOSE
 *  This structure encapsulates the properties and methods needed to represents a packed 32-bit (4 unsigned bytes) pixel 
 *  with four individual 8-bit (1 unsigned byte) values ranging from 0 to 255.
 *  
 *  The color components are stored in Blue, Green, Red, Alpha order.
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
    /// <para>The color components are stored in BGRA (Red, Green, Blue, Alpha) order.</para>
    /// <para>When possible, it uses the <c>Unsafe</c> class to optimize the methods for speed and low memory usage.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct BGRA32 : IPixel<BGRA32>
    {
        // ** Properties

        /// <summary>
        /// Gets or sets the blue component.
        /// </summary>
        public byte B;

        /// <summary>
        /// Gets or sets the green component.
        /// </summary>
        public byte G;

        /// <summary>
        /// Gets or sets the red component.
        /// </summary>
        public byte R;

        /// <summary>
        /// Gets or sets the alpha component.
        /// </summary>
        public byte A;

        // ** Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BGRA32"/> struct.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BGRA32(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = 255;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BGRA32"/> struct.
        /// </summary>
        /// <param name="r">The red component.</param>
        /// <param name="g">The green component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="a">The alpha component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BGRA32(byte r, byte g, byte b, byte a)
        {
            this.B = b;
            this.G = g;
            this.R = r;
            this.A = a;
        }

        /// <summary>
        /// Gets or sets the packed representation of the BGRA32 struct.
        /// </summary>
        public uint Bgra
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Unsafe.As<BGRA32, uint>(ref this);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            { 
                Unsafe.As<BGRA32, uint>(ref this) = value;
            }
        }

        /// <summary>
        /// Gets or sets the RGB components of this struct as <see cref="BGR24"/>
        /// </summary>
        public BGR24 Bgr
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Unsafe.As<BGRA32, BGR24>(ref this);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Unsafe.As<BGRA32, BGR24>(ref this) = value;
            }
        }

        /// <inheritdoc/>
        public uint PackedValue
        {
            get => this.Bgra;
            set => this.Bgra = value;
        }

        /// <inheritdoc/>
        public bool Equals(BGRA32 other)
        {
            return this.R == other.R && this.G == other.G && this.B == other.B && this.A == other.A;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj?.GetType() == typeof(BGRA32) && this.Equals((BGRA32)obj);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.B;
                hashCode = (hashCode * 397) ^ this.G;
                hashCode = (hashCode * 397) ^ this.R;
                hashCode = (hashCode * 397) ^ this.A;
                return hashCode;
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromVector4(Vector4 vector)
        {
            var rgba = default(RGBA32);
            rgba.PackFromVector4(vector);
            this.PackFromRgba32(rgba);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4 ToVector4()
        {
            return this.ToRgba32().ToVector4();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PackFromRgba32(RGBA32 source)
        {
            this.R = source.R;
            this.G = source.G;
            this.B = source.B;
            this.A = source.A;
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
        public void ToRgba32(ref RGBA32 dest)
        {
            dest.R = this.R;
            dest.G = this.G;
            dest.B = this.B;
            dest.A = this.A;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToBgr24(ref BGR24 dest)
        {
            dest = Unsafe.As<BGRA32, BGR24>(ref this);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToBgra32(ref BGRA32 dest)
        {
            dest = this;
        }

        /// <summary>
        /// Converts the pixel to <see cref="RGBA32"/> format.
        /// </summary>
        /// <returns>The RGBA value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RGBA32 ToRgba32() => new RGBA32(this.R, this.G, this.B, this.A);
    }
}
