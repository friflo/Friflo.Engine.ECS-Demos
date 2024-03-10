// Copyright (c) Ullrich Praetz - https://github.com/friflo. All rights reserved.
// See LICENSE file in the project root for full license information.

using System.Numerics;
using Godot;

namespace Example;

public static class MathUtils
{
    public static Transform3D AsTransform3D(this Matrix4x4 m)
    {
        return new Transform3D(
            m.M11, m.M12, m.M13,
            m.M21, m.M22, m.M23,
            m.M31, m.M32, m.M33,
            m.M41, m.M42, m.M43);
    }
}