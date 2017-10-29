﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace SmoothedSharpening
{
    class DrawingCanvas : Canvas
    {
        public Camera camera { get; set; }

        public Cube toRender { get; set; }

        public DrawingCanvas() {
            camera = new Camera();
            camera.screen = new Screen();
            camera.screen.z = 100;
        }

        static Color color = Color.FromRgb(200, 160, 50);

        bool needToStrokeEdges = false;

        protected override void OnRender(DrawingContext dc)
        {
            int k = 0;
            var r = new Random();
            foreach(var p in toRender.rotatedPolygons ?? toRender.polygons)
            {
                k++;
                var path = new PathFigure();
                path.StartPoint = camera.WorldToScreenPoints(p.vertecies[0]);
                for(var i = 1; i < p.vertecies.Length; i++)
                    path.Segments.Add(new LineSegment(camera.WorldToScreenPoints(p.vertecies[i]), needToStrokeEdges));
                path.IsClosed = true;
                var geometry = new PathGeometry();
                geometry.Figures.Add(path);
                var c = Color.FromRgb((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255));
                dc.DrawGeometry(new SolidColorBrush(c), new Pen(), geometry);
            }
        }
    }
}
