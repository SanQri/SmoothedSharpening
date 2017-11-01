using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothedSharpening.Model
{
    class Quaternion
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float w { get; set; }

        public Quaternion() { }
        public Quaternion(Quaternion q)
        {
            x = q.x;
            y = q.y;
            z = q.z;
            w = q.w;
        }

        public static Quaternion operator *(Quaternion q, Vector3 v)
            => new Quaternion()
            {
                x =  q.w * v.x + q.y * v.z - q.z * v.y,
                y =  q.w * v.y - q.x * v.z + q.z * v.x,
                z =  q.w * v.z + q.x * v.y - q.y * v.x,
                w = -q.x * v.x - q.y * v.y - q.z * v.z
            };

        public static Quaternion operator *(Quaternion q, float a)
            => new Quaternion()
            {
                x = q.x * a,
                y = q.y * a,
                z = q.z * a,
                w = q.w * a
            };

        public static Quaternion operator *(Quaternion q1, Quaternion q2)
            => new Quaternion()
            {
                x = q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y,
                y = q1.w * q2.y - q1.x * q2.z + q1.y * q2.w + q1.z * q2.x,
                z = q1.w * q2.z + q1.x * q2.y - q1.y * q2.x + q1.z * q2.w,
                w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z
            };

        public float Abs()
            => (float)Math.Sqrt(w * w + x * x + y * y + z * z);

        public Quaternion Normalized()
            => new Quaternion(this) * (1 / this.Abs());

        public Quaternion Normalize()
            => this * (1 / this.Abs());

        public Quaternion Inverted()
            => new Quaternion()
            {
                w = w,
                x = -x,
                y = -y,
                z = -z
            }.Normalize();
    }
}
