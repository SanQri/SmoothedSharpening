using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Runtime.CompilerServices;

namespace SmoothedSharpening
{
    class Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3() { }
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point toPoint() => new Point(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator /(Vector3 v, float a)
            => new Vector3() { x = v.x / a, y = v.y / a, z = v.z / a };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator *(Vector3 v, float a)
            => new Vector3() { x = v.x * a, y = v.y * a, z = v.z * a };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator +(Vector3 v, Vector3 v2)
            => new Vector3() { x = v.x + v2.x, y = v.y + v2.y, z = v.z + v2.z };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator -(Vector3 v, Vector3 v2)
            => new Vector3() { x = v.x - v2.x, y = v.y - v2.y, z = v.z - v2.z };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float dotProfuct(Vector3 v)
            => x * v.x + y * v.y + z * v.z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 crossProduct(Vector3 v)
            => new Vector3() { x = y * v.z - z * v.y, y = x * v.z - z * v.x, z = x * v.y - y * v.x };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Abs()
            => (float)Math.Sqrt(x * x + y * y + z * z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float SqrAbs()
            => x * x + y * y + z * z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float SqrMagnitude(Vector3 v)
            => (x - v.x) * (x - v.x) + (y - v.y) * (y - v.y) + (z - v.z) * (z - v.z);

        // <summary>
        // Return a new vector equal to normalized this
        // </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 Normalized()
            => new Vector3(this) / Abs();

        // <summary>
        // Normalizes this vector and returns it
        // </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 Normalize()
            => this / Abs();

    }
}
