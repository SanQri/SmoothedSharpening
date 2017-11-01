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

        public Point WorldToScreenPoints(Vector3 v)
            => (v * (screen.z / v.z) + new Vector3(screen.bounds.width / 2, screen.bounds.height / 2, 0)).toPoint();
    }
}
