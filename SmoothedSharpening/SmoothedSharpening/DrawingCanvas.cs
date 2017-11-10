using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Drawing.Drawing2D;
using System.Drawing;
using ColorF = System.Drawing.Color;
using ColorM = System.Windows.Media.Color;

namespace SmoothedSharpening
{
    class DrawingCanvas : Canvas
    {
        public Camera camera { get; set; }

        public List<ShapeGeometry> shapesToRender { get; set; } = new List<ShapeGeometry>();
        public PointLight[] lights;

        public DrawingCanvas()
        {
            camera = new Camera();
            camera.screen = new Screen();
            camera.screen.z = 100;
            camera.screen.bounds = new Rect2D() { x = 0, y = 0, width = 1280, height = 720 };
        }

        static ColorM color = ColorM.FromRgb(200, 160, 50);

        bool needToStrokeEdges = true;

        protected override void OnRender(DrawingContext dc)
        {
            int k = 0;
            var r = new Random();
            foreach (var shape in shapesToRender)
            {
                foreach (var p in shape.TransformedPolygons() ?? shape.localPolygons)
                {
                    if (p.GetNormal().dotProfuct(p.vertices[0]) > 0)
                        continue;
                    var path = new PathFigure();
                    path.StartPoint = camera.WorldToScreenPoints(p.vertices[0].v);
                    PointF[] pointFs = new PointF[p.vertices.Length];
                    p.lightAtIndex = new float[p.vertices.Length];
                    for (var i = 1; i < p.vertices.Length; i++)
                    {
                        var tP = camera.WorldToScreenPoints(p.vertices[i].v);
                        pointFs[i] = new PointF((float)tP.X, (float)tP.Y);
                        path.Segments.Add(new LineSegment(tP, needToStrokeEdges));
                        p.lightAtIndex[i] = 0;
                        foreach (var l in lights) {
                            float d = l.position.SqrMagnitude(p.vertices[i]);
                            if (l.range < d)
                                p.lightAtIndex[i] += l.intensity * ((l.range - d) / l.range);
                        }
                    }
                    path.IsClosed = true;
                    var geometry = new PathGeometry();
                    geometry.Figures.Add(path);
                    var c = ColorM.FromRgb((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255));
                    var c2 = ColorM.FromRgb((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255));
                    ColorF[] colors = new ColorF[p.vertices.Length];
                    var brush = new PathGradientBrush(pointFs);
                    for (int i = 0; i < p.vertices.Length; i++)
                    {
                        colors[i] = ColorF.FromArgb((byte)(p.color.R * p.lightAtIndex[i]), (byte)(p.color.G * p.lightAtIndex[i]), (byte)(p.color.B * p.lightAtIndex[i]));
                    }
                    brush.SurroundColors = colors;
                    //dc.DrawGeometry(brush, new System.Windows.Media.Pen(new SolidColorBrush(ColorM.FromRgb(40,40,40)), 1), geometry);  
                }
            }
        }
    }
}
