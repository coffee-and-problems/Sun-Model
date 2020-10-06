using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework_calsulations
{
    public static class Extentions
    {
        public static double RadiansToDegrees(this double radians)
        {
            return (radians > 0 ? radians : (2 * Math.PI + radians)) * 360 / (2 * Math.PI);
        }

        public static List<double> Normalize(this List<double> list, double value)
        {
            return list.Select(x => x / value).ToList();
        }
    }
}
