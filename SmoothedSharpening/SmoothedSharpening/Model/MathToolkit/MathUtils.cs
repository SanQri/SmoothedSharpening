using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothedSharpening
{
    static class MathUtils
    {
        public static int min(int a, int b)
        {
            return a < b ? a : b;
        }
        public static int max(int a, int b)
        {
            return a > b ? a : b;
        }
        public static double max(double a, double b)
        {
            return a < b ? b : a;
        }
        public static double min(double a, double b)
        {
            return a > b ? b : a;
        }
        public static float min(float a, float b)
        {
            return a < b ? a : b;
        }
        public static float max(float a, float b)
        {
            return a > b ? a : b;
        }

        //public (float sin, float cos) sincos(float x)
        //{

        //}
    }
}
