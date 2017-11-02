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

namespace SmoothedSharpening
{
    class Vector3
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

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

        public Point toPoint() => new Point(x, y);

        public static Vector3 operator /(Vector3 v, float a)
            => new Vector3() { x = v.x / a, y = v.y / a, z = v.z / a };
        public static Vector3 operator *(Vector3 v, float a)
            => new Vector3() { x = v.x * a, y = v.y * a, z = v.z * a };
        public static Vector3 operator +(Vector3 v, Vector3 v2)
            => new Vector3() { x = v.x + v2.x, y = v.y + v2.y, z = v.z + v2.z };
        public static Vector3 operator -(Vector3 v, Vector3 v2)
            => new Vector3() { x = v.x - v2.x, y = v.y - v2.y, z = v.z - v2.z };

        public float dotProfuct(Vector3 v)
            => x * v.x + y * v.y + z * v.z;

        public Vector3 crossProduct(Vector3 v)
            => new Vector3() { x = y * v.z - z * v.y, y = x * v.z - z * v.x, z = x * v.y - y * v.x };

        public float Abs()
            => (float)Math.Sqrt(x * x + y * y + z * z);

        public float SqrAbs()
            => x * x + y * y + z * z;

        // <summary>
        // Return a new vector equal to normalized this
        // </summary>
        public Vector3 Normalized()
            => new Vector3(this) / Abs();

        // <summary>
        // Normalizes this vector and returns it
        // </summary>
        public Vector3 Normalize()
            => this / Abs();

    }
}
