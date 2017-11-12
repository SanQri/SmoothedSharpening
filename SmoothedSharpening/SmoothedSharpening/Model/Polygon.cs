using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SmoothedSharpening
{
    class Polygon
    {
        public Ref<Vector3>[] vertices;
        public Edge[] edges;
        public int[] indexes;
        public Color color;
        public float[] lightAtIndex;

        public Polygon(Ref<Vector3>[] vertecies, int[] indexes)
        {
            Random o = new Random();
            color = Color.FromRgb((byte)o.Next(255), (byte)o.Next(255), (byte)o.Next(255));
            this.indexes = indexes;
            int l = indexes.Length;
            this.vertices = new Ref<Vector3>[l];
            for (int i = 0; i < l; i++)
            {
                this.vertices[i] = vertecies[indexes[i]];
            }
            edges = new Edge[l];
            for(int i = 0; i < l - 1; i++)
            {
                edges[i] = new Edge(vertices[i], vertices[i + 1]);
            }
            edges[l - 1] = new Edge(vertices[l - 1], vertices[0]);
        }

        public Vector3 GetCenter()
        {
            var c = new Vector3();
            foreach (var v in vertices)
                c += v;
            c /= vertices.Length;
            return c;
        }

        public Vector3 GetNormal()
        {
            Vector3 v1 = vertices[0].v - vertices[1].v;
            Vector3 v2 = vertices[2].v - vertices[1].v;

            if (v1.y > 0 && v2.y > 0 && v1.x < v2.x || (v1.y < 0 && v2.y < 0 && v1.x < v2.x))
                return v1.crossProduct(v2);
            else
                return v2.crossProduct(v1);
        }
    }
}
