using System;
using System.Collections.Generic;
using Framework_calsulations;

namespace ModelCalculations
{
    public static class Trajectory
    {
        public static int HowManyCubesForTrajectory(int hCube, int xCube)
        {
            var alpha = Math.Acos(1 + Constants.RSun / hCube);
            var beta = Math.Acos(1 - xCube * xCube / (2 * Constants.RSun * Constants.RSun));
            return Convert.ToInt32(Math.Round(alpha / beta, 0, MidpointRounding.AwayFromZero));
        }

        public static (List<Coords> cords, List<double> hs) GetCoords(double impactParameter)
        {
            var coords = new List<Coords>();
            var hs = new List<double>();

            var x0 = Constants.HMax + Constants.RSun;
            var deltaX = 20; //км

            if (impactParameter > Constants.MaxImpactParameter)
            {
                throw new ArgumentException("Неверно введен прицельный параметр");
            }

            var maxIterations = impactParameter > 1 && impactParameter < Constants.HMax
                ? Math.Sqrt(x0 * x0 - Constants.RSun * Constants.RSun) / 20 + (Constants.RSun + Constants.HMax) / 20 + 10
                : double.PositiveInfinity;

            var y = impactParameter * Constants.RSun;
            for (var i = 0; i < maxIterations; i++)
            {
                var x = x0 - deltaX * i;
                var r = Math.Sqrt(x * x + y * y);
                var h = r - Constants.RSun;
                var phi = Math.Atan2(y, x).RadiansToDegrees();

                if (h < 0) break;
                if (h > Constants.HMax) continue;

                coords.Add(new Coords(h, phi));
                hs.Add(h);
            }

            return (coords, hs);
        }

        public static List<double> GetTrajectory(List<Coords> coords, string file)
        {
            var trajectory = new List<double>();
            var deltaPhi = Math.Atan2(48, Constants.RSun).RadiansToDegrees();
            var data = FitsHandler.GetData(file, 0) as double[][][];
            var linearData = FitsHandler.TransformToLinearData(data);

            var coord = 0;
            var hTop = double.PositiveInfinity;
            var hLow = double.PositiveInfinity;
            for (var i = 0; i < coords.Count; i++)
            {
                var h = coords[i].h;
                var hdu1Data = FitsHandler.GetData(file, 1) as double[];

                for (var j = Constants.StartIndex; j < hdu1Data.Length * 1000; j++)
                {
                    if (hdu1Data[i] * 1000 > h)
                    {
                        hTop = hdu1Data[i] * 1000;
                        hLow = hdu1Data[i - 1] * 1000;
                        coord = i;
                        break;
                    }
                }

                var phi = coords[i].phi;
                var k = Convert.ToInt32((phi % (deltaPhi * hdu1Data.Length - 1)) / deltaPhi);

                var fR2 = (deltaPhi * (k + 1) - phi % (deltaPhi * 503)) * linearData[coord][k] / deltaPhi
                          + (phi % (deltaPhi * 503) - deltaPhi * k) * linearData[coord][k + 1] / deltaPhi;
                var fR1 = (deltaPhi * (k + 1) - phi % (deltaPhi * 503)) * linearData[coord - 1][k] / deltaPhi
                           + (phi % (deltaPhi * 503) - deltaPhi * k) * linearData[coord - 1][k + 1] / deltaPhi;

                var value = (hTop - h) * fR1 / (hTop - hLow) + (h - hLow) * fR2 / (hTop - hLow);
                trajectory.Add(value);
            }

            return trajectory;
        }
    }
}
