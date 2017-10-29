using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothedSharpening
{
    class Polygon
    {
        public Vertex[] vertecies;
        public Edge[] edges;
        public int[] indexes;

        public Polygon(Vertex[] vertecies, int[] indexes)
        {
            this.indexes = indexes;
            int l = indexes.Length;
            this.vertecies = new Vertex[l];
            for (int i = 0; i < l; i++)
            {
                this.vertecies[i] = vertecies[indexes[i]];
            }
            edges = new Edge[l];
            for(int i = 0; i < l - 1; i++)
            {
                edges[i] = new Edge { startPoint = this.vertecies[i], endPoint = this.vertecies[i + 1] };
            }
            edges[l - 1] = new Edge { startPoint = this.vertecies[l - 1], endPoint = this.vertecies[0] };
        }
    }
}
