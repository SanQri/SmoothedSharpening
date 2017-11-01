using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothedSharpening
{
    class ShapeGeometry
    {
        public Vector3[] localVerticies { get; set; }
        public Vector3[] worldVerticies { get; set; }
        //public Edge[] edges { get; set; }
        //public Edge[] rotatedEdges { get; set; }
        public Polygon[] localPolygons { get; set; }
        public Polygon[] worldPolygons { get; set; }

        public ShapeGeometry(Vector3[] vertecies, Polygon[] polygons)
        {
            localVerticies = vertecies;
            worldVerticies = new Vector3[vertecies.Length];
            Array.Copy(vertecies, worldVerticies, vertecies.Length);
            localPolygons = polygons;
            worldPolygons = new Polygon[polygons.Length];
            Array.Copy(polygons, worldPolygons, polygons.Length);

            Vector3 c = new Vector3();
            foreach (var v in worldVerticies)
            {
                c += v;
            }
            c /= worldVerticies.Length;
            Center = c;
        }

        Vector3 _center;
        public Vector3 Center
        {
            set
            {
                if (_center == null) {
                    _center = value;
                }
                else
                {
                    var diff = value - _center;
                    for (int i = 0; i < worldVerticies.Length; i++)
                    {
                        worldVerticies[i] += diff;
                    }
                }
            }
        }

        Vector3 _scale = new Vector3() { x = 1, y = 1, z = 1 };
        public Vector3 Scale
        {
            set
            {
                var diff = new Vector3() { x = value.x / _scale.x, y = value.y / _scale.y, z = value.z / _scale.z };
                for (int i = 0; i < localVerticies.Length; i++)
                    localVerticies[i] = localVerticies[i].dotProfuct(diff);
            }
        }

        public void rotate(float deg)
        {
            worldPolygons = new Polygon[localPolygons.Length];
            worldVerticies = new Vector3[localVerticies.Length];

            float sin = (float)Math.Sin(deg);
            float cos = (float)Math.Cos(deg);

            for (int i = 0; i < localVerticies.Length; i++)
                worldVerticies[i] = new Vector3()
                { 
                    x = cos * localVerticies[i].x + sin * localVerticies[i].z,
                    y = localVerticies[i].y,
                    z = -sin * localVerticies[i].x + cos * localVerticies[i].z
                };
            for(int i = 0; i < localPolygons.Length; i++)
                worldPolygons[i] = new Polygon(worldVerticies, localPolygons[i].indexes);
        }
        
    }
}
