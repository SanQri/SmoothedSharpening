using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;

namespace SmoothedSharpening
{
    class DrawingBitmapManager
    {
        public WriteableBitmap bitmap { get; set; } = new WriteableBitmap(1280, 720, 96, 96, PixelFormats.Bgr24, null);

        public byte[] imageByteData;
        int bitmapWidth;
        int bitmapHeight;

        public Camera camera { get; set; }

        public List<ShapeGeometry> shapesToRender { get; set; } = new List<ShapeGeometry>();
        public PointLight[] lights;

        struct PointF
        {
            public float x;
            public float y;
        }

        float cameraZ;
        float invertionCameraZ;

        public DrawingBitmapManager()
        {
            bitmapWidth = (int)bitmap.Width;
            bitmapHeight = (int)bitmap.Height;
            imageByteData = new byte[bitmapWidth * bitmapHeight * 3];
            ClearData();

            camera = new Camera();
            camera.screen = new Screen();
            camera.screen.z = 100;
            camera.screen.bounds = new Rect2D() { x = 0, y = 0, width = 1280, height = 720 };
            cameraZ = camera.screen.z;
            invertionCameraZ = 1 / camera.screen.z;
        }

        void ClearData()
        {
            int t = bitmapHeight * bitmapWidth * 3;
            for (int i = 0; i < t; i++)
                imageByteData[i] = 170;
        }

        public void Draw()
        {
            ClearData();
            var t = shapesToRender;
            for (int i = 0; i < shapesToRender.Count; i++)
                DrawShape(shapesToRender[i]);
            Int32Rect rect = new Int32Rect(0, 0, bitmapWidth, bitmapHeight);
            int stride = 3 * bitmapWidth;
            bitmap.WritePixels(rect, imageByteData, stride, 0);
        }

        void DrawShape(ShapeGeometry shape)
        {
            var t = shape.TransformedPolygons();
            for (int i = 0; i < t.Length; i++)
                DrawPolygon(t[i]);
        }

        byte B;
        byte G;
        byte R;

        void DrawPolygon(Polygon p)
        {
            var normal = p.GetNormal();
            if (normal.dotProfuct(p.vertices[0]) > 0)
                return;

            Rect2D bounds = new Rect2D() { x = 9999, x2 = -9999, y = 9999, y2 = -9999, height = 0, width = 0 };
            PointF[] points = new PointF[p.vertices.Length];
            int vCount = p.vertices.Length;
            float[] py = new float[vCount];
            float[] px = new float[vCount];

            for (var i = 0; i < p.vertices.Length; i++)
            {
                var t = camera.WorldToScreenPoints(p.vertices[i]);
                px[i] = (float)t.X;
                py[i] = (float)t.Y;
                if (bounds.x > px[i])
                    bounds.x = px[i];
                if (bounds.y > py[i])
                    bounds.y = py[i];
                if (bounds.x2 < px[i])
                    bounds.x2 = px[i];
                if (bounds.y2 < py[i])
                    bounds.y2 = py[i];
            }
            bounds.width = bounds.x2 - bounds.x;
            bounds.height = bounds.y2 - bounds.y;
            float D = normal.dotProfuct(p.vertices[0]);

            B = p.color.B;
            G = p.color.G;
            R = p.color.R;

            var bx2 = Math.Min((int)bounds.x2, bitmapWidth);
            var by2 = Math.Min((int)bounds.y2, bitmapHeight);
            for (int x = Math.Max((int)bounds.x, 0); x < bx2; x++)
            {
                for (int y = Math.Max((int)bounds.y, 0); y < by2; y++)
                {
                    if (CountIntersections(px, py, x, y, vCount) % 2 == 0)
                        continue;
                    DrawPixel(x, y, normal, D);
                }
            }
        }

        void DrawPixel(int x, int y, Vector3 normal, float D)
        {
            float lightC = GetLightC(x, y, normal, D);
            var t = (y * bitmapWidth + x) * 3;
            if (t >= 0 && t < bitmapWidth * bitmapHeight * 3)
            {
                imageByteData[t] = (byte)(B * lightC);
                imageByteData[t + 1] = (byte)(G * lightC);
                imageByteData[t + 2] = (byte)(R * lightC);
                //imageByteData[t] = p.color.B;
                //imageByteData[t + 1] = p.color.R;
                //imageByteData[t + 2] = p.color.G;
            }
        }

        int CountIntersections(float[] px, float[] py, int x, int y, int vCount)
        {
            int c = 0;
            int t = vCount - 1;
            for (int i = 0, j = t; i < vCount; j = i++)
                if (((py[i] > y && py[j] < y) ||
                    (py[i] < y && py[j] > y)) &&
                    (x > (px[j] - px[i]) * (y - py[i]) / (py[j] - py[i]) + px[i]))
                    c++;
            return c;
        }

        float GetLightC(int x, int y, Vector3 normal, float D)
        {
            float lightC = 0;
            for(int i = 0; i < lights.Length; i++)
            {
                var tVector = new Vector3(x * invertionCameraZ, y * invertionCameraZ, cameraZ);
                var alpha = tVector.dotProfuct(normal) / D;
                //var worldPoint = tVector * alpha;
                tVector.x *= alpha;
                tVector.y *= alpha;
                tVector.z *= alpha;
                float d = lights[i].position.SqrMagnitude(tVector);
                float r = lights[i].range;
                if (r > d)
                    lightC += lights[i].intensity * (r - d) / r;
            }
            return lightC;
        }

    }
}
