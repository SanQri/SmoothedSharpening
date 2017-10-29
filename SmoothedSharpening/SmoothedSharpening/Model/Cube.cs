using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothedSharpening
{
    class Cube
    {
        public Vertex[] vertecies { get; set; }
        public Vertex[] rotatedVertecies { get; set; }
        public Edge[] edges { get; set; }
        public Edge[] rotatedEdges { get; set; }
        public Polygon[] polygons { get; set; }
        public Polygon[] rotatedPolygons { get; set; }

        public void rotate(float deg)
        {
            rotatedPolygons = new Polygon[polygons.Length];
            rotatedVertecies = new Vertex[vertecies.Length];

            float sin = (float)Math.Sin(deg);
            float cos = (float)Math.Cos(deg);

            for (int i = 0; i < vertecies.Length; i++)
                rotatedVertecies[i] = new Vertex()
                {
                    x = cos * vertecies[i].x + sin * vertecies[i].z,
                    y = vertecies[i].y,
                    z = -sin * vertecies[i].x + cos * vertecies[i].z
                };
            for(int i = 0; i < polygons.Length; i++)
                rotatedPolygons[i] = new Polygon(rotatedVertecies, polygons[i].indexes);
        }
        
    }
}
