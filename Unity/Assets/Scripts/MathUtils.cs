// Copyright (c) Ullrich Praetz - https://github.com/friflo. All rights reserved.
// See LICENSE file in the project root for full license information.


public static class MathExtensions
{
    public static UnityEngine.Matrix4x4 AsUnityMatrix4x4(in this System.Numerics.Matrix4x4 m)
    {
        return new UnityEngine.Matrix4x4 
        {
            m00 = m.M11,    m01 = m.M21,    m02 = m.M31,    m03 = m.M41,
            m10 = m.M12,    m11 = m.M22,    m12 = m.M32,    m13 = m.M42,
            m20 = m.M13,    m21 = m.M23,    m22 = m.M33,    m23 = m.M43,
            m30 = m.M14,    m31 = m.M24,    m32 = m.M34,    m33 = m.M44,
        };
        
        /*
        return new UnityEngine.Matrix4x4(
            new Vector4(m.M11, m.M12, m.M13, m.M14),
            new Vector4(m.M21, m.M22, m.M23, m.M24),
            new Vector4(m.M31, m.M32, m.M33, m.M34),
            new Vector4(m.M41, m.M42, m.M43, m.M44));
        */
    }
}