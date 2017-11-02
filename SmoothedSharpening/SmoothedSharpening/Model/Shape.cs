using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothedSharpening
{
    class ShapeGeometry
    {
        public Ref<Vector3>[] localVertices { get; set; }
        public Ref<Vector3>[] worldVertices { get; set; }
        //public Edge[] edges { get; set; }
        //public Edge[] rotatedEdges { get; set; }
        public Polygon[] localPolygons { get; set; }
        public Polygon[] worldPolygons { get; set; }

        public ShapeGeometry(Ref<Vector3>[] vertecies, Polygon[] polygons)
        {
            localVertices = vertecies;
            worldVertices = new Ref<Vector3>[vertecies.Length];
            Array.Copy(vertecies, worldVertices, vertecies.Length);
            localPolygons = polygons;
            worldPolygons = new Polygon[polygons.Length];
            Array.Copy(polygons, worldPolygons, polygons.Length);

            Vector3 c = new Vector3();
            foreach (var v in worldVertices)
            {
                c += v.v;
            }
            c /= worldVertices.Length;
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
                    for (int i = 0; i < worldVertices.Length; i++)
                    {
                        worldVertices[i].v += diff;
                    }
                    _center = value;
                }
            }
        }

        Vector3 _scale = new Vector3() { x = 1, y = 1, z = 1 };
        public Vector3 Scale
        {
            set
            {
                var diff = new Vector3() { x = value.x / _scale.x, y = value.y / _scale.y, z = value.z / _scale.z };
                for (int i = 0; i < localVertices.Length; i++)
                {
                    localVertices[i].v.x *= diff.x;
                    localVertices[i].v.y *= diff.y;
                    localVertices[i].v.z *= diff.z;
                }
            }
        }

        public void rotate(float deg)
        {
            worldPolygons = new Polygon[localPolygons.Length];
            worldVertices = new Ref<Vector3>[localVertices.Length];

            float sin = (float)Math.Sin(deg);
            float cos = (float)Math.Cos(deg);

            for (int i = 0; i < localVertices.Length; i++)
                worldVertices[i] = new Ref<Vector3>()
                {
                    v = new Vector3()
                    {
                        x = cos * localVertices[i].v.x + sin * localVertices[i].v.z,
                        y = localVertices[i].v.y,
                        z = -sin * localVertices[i].v.x + cos * localVertices[i].v.z
                    }
                };
            for(int i = 0; i < localPolygons.Length; i++)
                worldPolygons[i] = new Polygon(worldVertices, localPolygons[i].indexes);
        }
        
    }
}
