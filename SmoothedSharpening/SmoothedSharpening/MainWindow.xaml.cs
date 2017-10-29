using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmoothedSharpening
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var c = new Cube();
            var v = new Vertex[] {
                new Vertex { x = 1, y = 1, z = 1 },
                new Vertex { x = 1, y = 1, z = -1 },
                new Vertex { x = 1, y = -1, z = 1 },
                new Vertex { x = 1, y = -1, z = -1 },
                new Vertex { x = -1, y = 1, z = 1 },
                new Vertex { x = -1, y = 1, z = -1 },
                new Vertex { x = -1, y = -1, z = 1 },
                new Vertex { x = -1, y = -1, z = -1 }
            };

            var polygons = new Polygon[] {
                new Polygon(v, new int[] { 0, 2, 6, 4 }),
                new Polygon(v, new int[] { 1, 5, 4, 0 }),
                new Polygon(v, new int[] { 1, 3, 2, 0 }),
                new Polygon(v, new int[] { 5, 7, 3, 1 }),
                new Polygon(v, new int[] { 4, 6, 7, 5 }),
                new Polygon(v, new int[] { 3, 7, 6, 2 }),
            };
            c.polygons = polygons;
            c.vertecies = v;
            c.rotate(2);
            for (var i = 0; i < c.vertecies.Length; i++)
            {
                c.rotatedVertecies[i].z = c.rotatedVertecies[i].z + 50;
                c.rotatedVertecies[i].y = c.rotatedVertecies[i].y + 105;
                c.rotatedVertecies[i].x = c.rotatedVertecies[i].x + 10;
            }
            var canvas = new DrawingCanvas();
            canvas.Height = 720;
            canvas.Width = 1280;
            canvas.Margin = new Thickness(200, 10, 0, 0);
            canvas.toRender = c;
            CanvasPanel.Children.Add(canvas);
            canvas.InvalidateVisual();

            zSlider.ValueChanged += (s, e) =>
            {
                canvas.camera.screen.z = (float)zSlider.Value * 100;
                canvas.InvalidateVisual();
            };
            rSlider.ValueChanged += (s, e) =>
            {
                c.rotate((float)rSlider.Value);
                //canvas.camera.screen.z = (float)zSlider.Value * 100;
                for (var i = 0; i < c.vertecies.Length; i++)
                {
                    c.rotatedVertecies[i].z = c.rotatedVertecies[i].z + 50;
                    c.rotatedVertecies[i].y = c.rotatedVertecies[i].y + 15;
                    c.rotatedVertecies[i].x = c.rotatedVertecies[i].x + 10;
                }
                canvas.InvalidateVisual();
            };
        }
    }
}
