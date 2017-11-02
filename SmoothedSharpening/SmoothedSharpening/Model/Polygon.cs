using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothedSharpening
{
    class Polygon
    {
        public Ref<Vector3>[] vertecies;
        public Edge[] edges;
        public int[] indexes;

        public Polygon(Ref<Vector3>[] vertecies, int[] indexes)
        {
            this.indexes = indexes;
            int l = indexes.Length;
            this.vertecies = new Ref<Vector3>[l];
            for (int i = 0; i < l; i++)
            {
                this.vertecies[i] = vertecies[indexes[i]];
            }
            edges = new Edge[l];
            for(int i = 0; i < l - 1; i++)
            {
                edges[i] = new Edge { startPoint = this.vertecies[i].Value, endPoint = this.vertecies[i + 1].Value };
            }
            edges[l - 1] = new Edge { startPoint = this.vertecies[l - 1].Value, endPoint = this.vertecies[0].Value };
        }
    }
}
