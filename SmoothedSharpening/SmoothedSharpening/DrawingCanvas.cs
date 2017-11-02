using System;
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

        public List<ShapeGeometry> shapesToRender { get; set; } = new List<ShapeGeometry>();

        public DrawingCanvas() {
            camera = new Camera();
            camera.screen = new Screen();
            camera.screen.z = 100;
            camera.screen.bounds = new Rect2D() { x = 0, y = 0, width = 1280, height = 720 };
        }

        static Color color = Color.FromRgb(200, 160, 50);

        bool needToStrokeEdges = false;

        protected override void OnRender(DrawingContext dc)
        {
            int k = 0;
            var r = new Random();
            foreach (var shape in shapesToRender)
            {
                foreach (var p in shape.worldPolygons ?? shape.localPolygons)
                {
                    k++;
                    var path = new PathFigure();
                    path.StartPoint = camera.WorldToScreenPoints(p.vertecies[0].Value);
                    for (var i = 1; i < p.vertecies.Length; i++)
                        path.Segments.Add(new LineSegment(camera.WorldToScreenPoints(p.vertecies[i].Value), needToStrokeEdges));
                    path.IsClosed = true;
                    var geometry = new PathGeometry();
                    geometry.Figures.Add(path);
                    var c = Color.FromRgb((byte)r.Next(255), (byte)r.Next(255), (byte)r.Next(255));
                    dc.DrawGeometry(new SolidColorBrush(c), new Pen(), geometry);
                }
            }
        }
    }
}
