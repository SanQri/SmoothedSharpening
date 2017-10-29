using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmoothedSharpening
{
    class Camera
    {
        public Screen screen { get; set; }

        //public Vertex ScreenToWorldPoints(Point p)
        //{
        //    new Vertex() { x = }
        //}

        public Point WorldToScreenPoints(Vertex v)
            => (v * (screen.z / v.z)).toPoint();
    }
}
