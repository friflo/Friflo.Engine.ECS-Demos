// Copyright (c) Ullrich Praetz - https://github.com/friflo. All rights reserved.
// See LICENSE file in the project root for full license information.

using System.Numerics;
using Microsoft.Xna.Framework;

// ReSharper disable InconsistentNaming
namespace MonoGame;

public static class MathExtensions
{
    public static Matrix4x4 AsMatrix4x4(this Matrix m)
    {
        return new Matrix4x4(
            m.M11, m.M12, m.M13, m.M14,
            m.M21, m.M22, m.M23, m.M24,
            m.M31, m.M32, m.M33, m.M34,
            m.M41, m.M42, m.M43, m.M44);
    }
}