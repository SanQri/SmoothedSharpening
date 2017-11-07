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

            var v = new Ref<Vector3>[] {
                new Ref<Vector3> () { v = new Vector3 { x = 1, y = 1, z = 1 } },
                new Ref<Vector3> () { v = new Vector3 { x = 1, y = 1, z = -1 } },
                new Ref<Vector3> () { v = new Vector3 { x = 1, y = -1, z = 1 } },
                new Ref<Vector3> () { v = new Vector3 { x = 1, y = -1, z = -1 } },
                new Ref<Vector3> () { v = new Vector3 { x = -1, y = 1, z = 1 } },
                new Ref<Vector3> () { v = new Vector3 { x = -1, y = 1, z = -1 } },
                new Ref<Vector3> () { v = new Vector3 { x = -1, y = -1, z = 1 } },
                new Ref<Vector3> () { v = new Vector3 { x = -1, y = -1, z = -1 } }
            };

            var v1 = new Ref<Vector3>[] {
                new Ref<Vector3> () { v = new Vector3 { x = 0, y = 2, z = 0 } },
                new Ref<Vector3> () { v = new Vector3 { x = -1, y = 0, z = 0.5f } },
                new Ref<Vector3> () { v = new Vector3 { x = 1, y = 0, z = 0.5f } },
                new Ref<Vector3> () { v = new Vector3 { x = 0, y = 0, z = -1 } }
            };
            var polygons1 = new Polygon[] {
                new Polygon(v1, new int[] { 0, 1, 2 }),
                new Polygon(v1, new int[] { 0, 2, 3 }),
                new Polygon(v1, new int[] { 0, 1, 3 }),
                new Polygon(v1, new int[] { 1, 2, 3 })
            };

            var v2 = new Ref<Vector3>[] {
                new Ref<Vector3> () { v = new Vector3 { x = -1, y = 0, z = 1 } },
                new Ref<Vector3> () { v = new Vector3 { x = -1, y = 3, z = 1 } },
                new Ref<Vector3> () { v = new Vector3 { x = -1, y = 3, z = -1 } },
                new Ref<Vector3> () { v = new Vector3 { x = -1, y = 0, z = -1 } },
                new Ref<Vector3> () { v = new Vector3 { x = 2, y = 0, z = -1 } },
                new Ref<Vector3> () { v = new Vector3 { x = 2, y = 0, z = 1 } },
                new Ref<Vector3> () { v = new Vector3 { x = 2, y = 1.5f, z = 1 } },
                new Ref<Vector3> () { v = new Vector3 { x = 1, y = 1.5f, z = 1 } },
                new Ref<Vector3> () { v = new Vector3 { x = 1, y = 3, z = 1 } },
                new Ref<Vector3> () { v = new Vector3 { x = 1, y = 3, z = -1 } },
                new Ref<Vector3> () { v = new Vector3 { x = 1, y = 1.5f, z = -1 } },
                new Ref<Vector3> () { v = new Vector3 { x = 2, y = 1.5f, z = -1 } },
            };
            var polygons2 = new Polygon[] {
                new Polygon(v2, (new int[] { 8, 9, 2, 1 }).Reverse().ToArray()),
                new Polygon(v2, (new int[] { 10, 7, 6, 11 }).Reverse().ToArray()),
                new Polygon(v2, (new int[] { 9, 8, 7, 10 })),
                new Polygon(v2, (new int[] { 0, 1, 2, 3 })),
                new Polygon(v2, new int[] { 2, 9, 10, 11, 4, 3 }),
                new Polygon(v2, new int[] { 0, 5, 4, 3 }),
                new Polygon(v2, new int[] { 5, 6, 7, 8, 1, 0 }),
                new Polygon(v2, (new int[] { 4, 5, 6, 11 }).Reverse().ToArray()),
            };


            var polygons = new Polygon[] {
                new Polygon(v, new int[] { 0, 2, 6, 4 }),
                new Polygon(v, new int[] { 1, 5, 4, 0 }),
                new Polygon(v, new int[] { 1, 3, 2, 0 }),
                new Polygon(v, new int[] { 5, 7, 3, 1 }),
                new Polygon(v, new int[] { 4, 6, 7, 5 }),
                new Polygon(v, new int[] { 3, 7, 6, 2 }),
            };
            var c = new ShapeGeometry(v, polygons);
            var c1 = new ShapeGeometry(v1, polygons1);
            var c2 = new ShapeGeometry(v2, polygons2);
            //for (var i = 0; i < c.localVerticies.Length; i++)
            //{
            //    c.worldVerticies[i].Value.z = c.worldVerticies[i].Value.z + 50;
            //    c.worldVerticies[i].Value.y = c.worldVerticies[i].Value.y + 105;
            //    c.worldVerticies[i].Value.x = c.worldVerticies[i].Value.x + 10;
            //}
            c2.Scale = new Vector3(1, 1, 0.3f);
            var canvas = new DrawingCanvas();
            canvas.Height = 720;
            canvas.Width = 1280;
            canvas.Margin = new Thickness(200, 10, 0, 0);
            //canvas.shapesToRender.Add(c);
            //canvas.shapesToRender.Add(c1);
            canvas.shapesToRender.Add(c2);
            CanvasPanel.Children.Add(canvas);
            canvas.InvalidateVisual();

            zSlider.ValueChanged += (s, e) =>
            {
                canvas.camera.screen.z = (float)zSlider.Value * 100;
                canvas.InvalidateVisual();
            };
            rSlider.ValueChanged += (s, e) =>
            {
                c2.rotate((float)rSlider.Value);
                //canvas.camera.screen.z = (float)zSlider.Value * 100;
                canvas.InvalidateVisual();
            };
            var mult = 10;
            xSlider.ValueChanged += (s, e) =>
            {
                c2.Center = new Vector3((float)xSlider.Value * mult-20, (float)ySlider.Value * mult-20, 5);
                canvas.InvalidateVisual();
            };
            ySlider.ValueChanged += (s, e) =>
            {
                c2.Center = new Vector3((float)xSlider.Value * mult - 20, (float)ySlider.Value * mult - 20, 5);
                canvas.InvalidateVisual();
            };
            sSlider.ValueChanged += (s, e) =>
            {
                c2.Scale = new Vector3((float)sSlider.Value + 0.1f, (float)sSlider.Value + 0.1f, (float)sSlider.Value + 0.1f);
                canvas.InvalidateVisual();
            };
        }
    }

    public class Ref<T>
    {
        public T v { get; set; }

        public static implicit operator T(Ref<T> r) => r.v;

        public static implicit operator Ref<T>(T v) => new Ref<T>() { v = v }; 
                
    }
}
