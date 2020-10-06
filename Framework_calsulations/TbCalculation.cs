using System;
using System.Collections.Generic;
using Framework_calsulations;

namespace ModelCalculations
{
    public class TbCalculation
    {
        private static double GetTbForImpactParameter(List<Coords> coords, double wavelength,
            List<double> trajectoryT, List<double> trajectoryNe, List<double> trajectoryNHi)
        {
            var frequency = Constants.c * 10 / wavelength;

            double deg1 = 0;
            double Tb = 0;

            for (var i = 0; i < coords.Count; i++)
            {
                var h = coords[i].h;
                var t = trajectoryT[i];
                var ne = trajectoryNe[i];
                var nhi = trajectoryNHi[i];

                double A;
                if (t <= 2e5) A = 17.9 + 1.5 * Math.Log(t) - Math.Log(frequency);
                else A = 24.5 + Math.Log(t) - Math.Log(frequency);

                var k = 9.78e-3 * ne / (frequency * frequency) / (Math.Pow(t, 1.5)) * ne * A;
                var kT = Math.Sqrt(k * t / Constants.Eh);
                var ka = 2.15e-29 * nhi * ne * Math.Sqrt(k * t) / (h * frequency) / (h * frequency)
                         * Math.Pow(Constants.Eh, 2.25)
                         * Math.Exp(-4.862 * kT * (1 - 0.2096 * kT + 0.00170 * kT * kT - 0.00968 * kT* kT* kT));

                var kSum = k + ka;
                var dh = 2000000;
                var deltaT = (1 - Math.Exp(-kSum * dh)) * t * Math.Exp(-deg1);
                Tb += deltaT;
                deg1 += kSum * dh;
            }

            return Tb;
        }

        public static double[] GetTb(List<double> impactParameters, double wavelength)
        {
            var Tbs = new double[impactParameters.Count];

            for (var i = 0; i < impactParameters.Count; i++)
            {
                var impactParameter = impactParameters[i];
                var coords = Trajectory.GetCoords(impactParameter).cords;
                var trajectoryT = Trajectory.GetTrajectory(coords, Constants.LgT);
                var trajectoryNe = Trajectory.GetTrajectory(coords, Constants.LgNe).Normalize(1000000);
                var trajectoryNHi =
                    Trajectory.GetTrajectory(coords, Constants.LgT).Normalize(1000000); //!!!!!!!!!!!!!!!!!!!!!!
                var tb = GetTbForImpactParameter(coords, wavelength, trajectoryT, trajectoryNe, trajectoryNHi);
                Tbs[i] = tb;
            }

            return Tbs;
        }
    }
}