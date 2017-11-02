using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothedSharpening
{
    class Polygon
    {
        public Ref<Vector3>[] vertices;
        public Edge[] edges;
        public int[] indexes;

        public Polygon(Ref<Vector3>[] vertecies, int[] indexes)
        {
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
                edges[i] = new Edge { startPoint = this.vertices[i].v, endPoint = this.vertices[i + 1].v };
            }
            edges[l - 1] = new Edge { startPoint = this.vertices[l - 1].v, endPoint = this.vertices[0].v };
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
