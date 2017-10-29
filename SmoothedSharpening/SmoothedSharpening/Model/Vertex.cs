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
    class Vertex
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public Point toPoint()
        {
            return new Point(x, y);
        }

        public static Vertex operator *(Vertex v, float a)
            => new Vertex() { x = v.x * a, y = v.y * a, z = v.z * a };
    }
}
