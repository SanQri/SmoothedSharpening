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
            int l = shapesToRender.Count;
            for (int i = 0; i < l; i++)
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
            Edge[] edges = new Edge[vCount];

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
            var minX = Math.Max((int)bounds.x, 0);
            int l = p.vertices.Length;
            for (int y = Math.Max((int)bounds.y, 0); y < by2; y++)
            {
                int c = 0;
                float[] intersectionX = new float[l];
                //float[] intersectionY = new float[l];
                int t = vCount - 1;
                for (int i = 0, j = t; i < vCount; j = i++)
                {
                    float k, b;
                    float xi;
                    if (!((y >= py[i] && y <= py[j]) || (y >= py[j] && y <= py[i])))
                        continue;
                    if (px[i] == px[j])
                        xi = px[i];
                    else
                    {
                        k = (py[i] - py[j]) / (px[i] - px[j]);
                        b = py[i] - k * px[i];
                        if (k == 0)
                            continue;
                        xi = (y - b) / k;
                    }
                    if(xi >= minX)
                        intersectionX[c++] = xi;
                }
                
                for (int i = 1; i < c; i++)
                {
                    var j = i;
                    while (j > 0 && intersectionX[j - 1] > intersectionX[j])
                    {
                        var tmp = intersectionX[j];
                        intersectionX[j] = intersectionX[j - 1];
                        intersectionX[j-- - 1] = tmp;
                    }
                }
                var sdff = 2 + 2;
                //for (int i = 0; i < vCount; i++)
                //{

                //    var xI = edges[i].IntersectionXAtY(y);
                //    if (xI >= edges[i].startPoint.v.x && xI <= edges[i].endPoint.v.x)
                //        intersectionX[c++] = xI;
                //}
                for (int i = 0; i < c; i += 2)
                    for(int x = (int)intersectionX[i]; x < (int)intersectionX[i + 1]; x++)
                        DrawPixel(x, y, normal, D);
                //for (int x = Math.Max((int)bounds.x, 0); x < bx2; x++)
                //{
                //    if (CountIntersections(px, py, x, y, vCount) % 2 == 0)
                //        continue;
                //    DrawPixel(x, y, normal, D);
                //}
            }
        }

        void DrawPixel(int x, int y, Vector3 normal, float D)
        {
            float lightC = GetLightC(x, y, normal, D);
            var t = (y * bitmapWidth + x) * 3;
            if (t >= 0 && t < bitmapWidth * bitmapHeight * 3)
            {
                imageByteData[t] = (byte)Math.Min(B * lightC, 255);
                imageByteData[t + 1] = (byte)Math.Min(G * lightC, 255);
                imageByteData[t + 2] = (byte)Math.Min(R * lightC, 255);
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
            int l = lights.Length;
            for(int i = 0; i < l; i++)
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
                    lightC += (float)(lights[i].intensity * (Math.Pow(r, 0.25) - Math.Pow(d, 0.25)) / Math.Pow(r, 0.25));
            }
            return lightC;
        }

    }
}
