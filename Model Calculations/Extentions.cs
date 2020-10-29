using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework_calsulations
{
    public static class Extentions
    {
        public static float RadiansToDegrees(this float radians)
        {
            return Convert.ToSingle(radians * 180 / Math.PI);
        }

        public static List<float> Normalize(this List<float> list, float value)
        {
            return list.Select(x => x / value).ToList();
        }
    }
}
