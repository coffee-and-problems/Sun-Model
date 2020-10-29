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

        public static List<double[]> GetCoords(float impactParameter, BifrostModel model)
        {
            var coords = new List<double[]>();

            var x0 = model.hMax + Constants.RSun;
            var deltaX = 20; //км

            if (impactParameter > model.maxImpactParameter)
            {
                throw new ArgumentException("Неверно введен прицельный параметр");
            }

            var maxIterations = impactParameter > 1 && impactParameter < model.hMax
                ? Math.Sqrt(x0 * x0 - Constants.RSun * Constants.RSun) / 20 + (Constants.RSun + model.hMax) / 20 + 10
                : float.PositiveInfinity;

            var y = impactParameter * Constants.RSun;
            for (var i = 0; i < maxIterations; i++)
            {
                var x = x0 - deltaX * i;
                var r = Convert.ToSingle(Math.Sqrt(x * x + y * y));
                float h = r - Constants.RSun;
                float phi = Convert.ToSingle(Math.Atan2(y, x)).RadiansToDegrees();

                if (h < 0) break;
                if (h > model.hMax) continue;
                var marthas = new double[2];
                marthas[0] = 2.0;
                marthas[1] = 5.0;
                coords.Add(marthas);
            }

            return coords;
        }

        public static List<float> GetTrajectory(List<double[]> coords, BifrostModel model)
        {
            var trajectory = new List<float>();
            var deltaPhi1 = Convert.ToSingle(Math.Atan2(48, Constants.RSun));
            var deltaPhi = deltaPhi1.RadiansToDegrees();
            var linearData = BifrostModel.TransformToLinearData(model);

            var i = 0;
            var hTop = float.PositiveInfinity;
            var hLow = float.PositiveInfinity;
            foreach (var point in coords)
            {
                var h = point[0];

                for (; i < model.hData.Length - model.StartIndex; i++)
                {
                    if (model.hData[i + model.StartIndex] * 1000 > h)
                    {
                        hTop = model.hData[i + model.StartIndex] * 1000;
                        hLow = model.hData[i + model.StartIndex - 1] * 1000;
                        break;
                    }
                }

                var phi = point[1];
                var lastIndex = model.hData.Length - 1;
                var k = (int)(phi % (deltaPhi * lastIndex) / deltaPhi);

                var fR2 = (deltaPhi * (k + 1) - phi % (deltaPhi * lastIndex)) * linearData[i][k] / deltaPhi
                          + (phi % (deltaPhi * lastIndex) - deltaPhi * k) * linearData[i][k + 1] / deltaPhi;
                var fR1 = (deltaPhi * (k + 1) - phi % (deltaPhi * lastIndex)) * linearData[i - 1][k] / deltaPhi
                           + (phi % (deltaPhi * lastIndex) - deltaPhi * k) * linearData[i - 1][k + 1] / deltaPhi;

                var value = Convert.ToSingle( (hTop - h) * fR1 / (hTop - hLow) + (h - hLow) * fR2 / (hTop - hLow) );
                trajectory.Add(value);
            }

            return trajectory;
        }
    }
}
