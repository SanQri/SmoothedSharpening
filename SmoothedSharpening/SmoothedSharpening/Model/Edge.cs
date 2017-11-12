using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothedSharpening
{
    class Edge
    {

        public Ref<Vector3> startPoint;
        // need to set endPoint last!!!
        Ref<Vector3> _endPoint;
        public Ref<Vector3> endPoint {
            get
            {
                return _endPoint;
            }
            set
            {
                _endPoint = value;
                if (endPoint.v.x - startPoint.v.x == 0)
                    k = 99999;
                else
                    k = (endPoint.v.y - startPoint.v.y) / (endPoint.v.x - startPoint.v.x);
                b = startPoint.v.y - startPoint.v.x * k;
            }
        }

        // y = kx + b
        public float k;
        public float b;

        public Edge(Ref<Vector3> startPoint, Ref<Vector3> endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public float IntersectionXAtY(float iy)
        {
            if (k == 0)
                return startPoint.v.x;
            return (iy - b) / k;
        }
    }
}
