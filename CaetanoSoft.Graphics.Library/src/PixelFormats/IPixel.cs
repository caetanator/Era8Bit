/**
 * IPixel.cs
 *
 * PURPOSE
 *  This interface encapsulates the properties and methods needed to represents a generic pixel color type packed 
 *  in 24-bit (3 bytes) or 32-bit (4 bytes), with the range of each pixel RGBA color component ranging from byte: 0 to 255.
 *  
 *  It also can handle the unpacked individual pixel RGBA color components in a normalized Vector4, with each component 
 *  ranging from float: 0.0 to 1.0.
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

using System;
using System.Numerics;

namespace CaetanoSoft.Graphics.PixelFormats
{
    /// <summary>
    /// This interface encapsulates the properties and methods needed to represents a generic pixel color type packed 
    /// in 24-bit (3 bytes) or 32-bit (4 bytes), with the range of each pixel RGBA color component ranging from <b>byte</b>: 0 to 255.
    /// <para>It also can handle the unpacked individual pixel RGBA color components in a normalized <see cref="Vector4"/>, with each component  
    /// ranging from <c>float</c>: 0.0 to 1.0.</para>
    /// </summary>
    /// <typeparam name="TSelf">The type of the self.</typeparam>
    public interface IPixel<TSelf> : IEquatable<TSelf> where TSelf : struct, IPixel<TSelf>
    {
        /// <summary>
        /// Packs the pixel color type from a <see cref="RGBA32"/>, 
        /// where each component ranges from 0 to 255.
        /// </summary>
        /// <param name="source">The <see cref="RGBA32"/> value to be assigned.</param>
        void PackFromRgba32(RGBA32 source);

        /// <summary>
        /// Sets the packed pixel color type <see cref="RGB24"/> from a normalized <see cref="Vector3"/>, 
        /// where each component ranges from 0.0 to 1.0.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> value to be assigned.</param>
        void PackFromVector3(Vector3 vector);

        /// <summary>
        /// Sets the packed pixel color type <see cref="RGBA32"/> from a normalized <see cref="Vector4"/>, 
        /// where each component ranges from 0.0 to 1.0.
        /// </summary>
        /// <param name="vector">The <see cref="Vector4"/> value to be assigned.</param>
        void PackFromVector4(Vector4 vector);

        /// <summary>
        /// Converts the packed pixel color type to the <see cref="RGB24"/> format.
        /// </summary>
        /// <param name="dest">The converted pixel value.</param>
        void ToRgb24(ref RGB24 dest);

        /// <summary>
        /// Converts the pixel to the <see cref="BGR24"/> format.
        /// </summary>
        /// <param name="dest">The converted pixel value.</param>
        void ToBgr24(ref BGR24 dest);

        /// <summary>
        /// Converts the packed pixel color type to the <see cref="RGBA32"/> format.
        /// </summary>
        /// <param name="dest">The converted pixel value.</param>
        void ToRgba32(ref RGBA32 dest);

        /// <summary>
        /// Converts the packed pixel color type to the <see cref="BGRA32"/> format.
        /// </summary>
        /// <param name="dest">The converted pixel value.</param>
        void ToBgra32(ref BGRA32 dest);

        /// <summary>
        /// Converts the packed pixel color type to the <see cref="ARGB32"/> format.
        /// </summary>
        /// <param name="dest">The converted pixel value.</param>
        void ToArgb32(ref ARGB32 dest);

        /// <summary>
        /// Gets the packed pixel color type <see cref="RGB24"/> value as a unpacked normalized <see cref="Vector3"/> representation, 
        /// where each RGB component ranges from 0.0 to 1.0.
        /// <para>The vector components are unpacked in least to greatest significance byte order (aka. {R, G, B}).</para>
        /// </summary>
        /// <returns>The normalized <see cref="Vector4"/> representation of the <see cref="RGBA32"/> packed value.</returns>
        Vector3 ToVector3();

        /// <summary>
        /// Gets the packed pixel color type <see cref="RGBA32"/> value as a unpacked normalized <see cref="Vector4"/> representation, 
        /// where each RGBA component ranges from 0.0 to 1.0.
        /// <para>The vector components are unpacked in least to greatest significance byte order (aka. {R, G, B, A}).</para>
        /// </summary>
        /// <returns>The normalized <see cref="Vector4"/> representation of the <see cref="RGBA32"/> packed value.</returns>
        Vector4 ToVector4();

        /// <summary>
        /// Gets the packed pixel color type <see cref="RGBA32"/> packed value as a hexadecimal string representation.
        /// <para>The value components are expanded in the #RRGGBBAA form.</para>
        /// </summary>
        /// <returns>A hexadecimal string representation of the <see cref="RGBA32"/> packed value.</returns>
        string ToHex();
    }
}
