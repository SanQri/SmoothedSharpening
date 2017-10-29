using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothedSharpening
{
    class Rect2D
    {
        public float x { get; set; }
        public float y { get; set; }
        public float x2 { get; set; }
        public float y2 { get; set; }
        public float width { get; set; }
        public float height { get; set; }

        public bool intersectsWith(Rect2D rect)
        {
            bool haveSharedX = (rect.x > x && rect.x < x + width) || (x > rect.x && x < rect.x + rect.width);
            bool haveSharedY = (rect.y > y && rect.y < y + height) || (y > rect.y && y < rect.y + rect.height);
            return haveSharedX && haveSharedY;
        }

        //public Rect2D intersectedWith(Rect2D rect)
        //{
        //    float newX = MathUtils.max(x, rect.x);

        //    return new Rect2D () { x = , y = MathUtils.max(y, rect.y), width = MathUtils.min(x + width, rect.x + rect.width), width = }
        //}
    }
}
