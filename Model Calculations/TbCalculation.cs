using System;
using System.Collections.Generic;
using Framework_calsulations;

namespace ModelCalculations
{
    public class TbCalculation
    {
        private static float GetTbForImpactParameter(List<double[]> coords, float wavelength,
            List<float> trajectoryT, List<float> trajectoryNe)
        {
            var frequency = Constants.c * 10 / wavelength;

            float deg1 = 0;
            float Tb = 0;

            for (var i = 0; i < coords.Count; i++)
            {
                var h = coords[i][0];
                var t = trajectoryT[i];
                var ne = trajectoryNe[i];

                float A;
                if (t <= 2e5) A = Convert.ToSingle(17.9 + 1.5 * Math.Log(t) - Math.Log(frequency));
                else A = Convert.ToSingle(24.5 + Math.Log(t) - Math.Log(frequency));

                var k = 9.78e-3 * ne / (frequency * frequency) / (Math.Pow(t, 1.5)) * ne * A;
                var kT = Math.Sqrt(k * t / Constants.Eh);
                var ka = 2.15e-29 * ne * Math.Sqrt(k * t) / (h * frequency) / (h * frequency) //вот тут было * nhi
                         * Math.Pow(Constants.Eh, 2.25)
                         * Math.Exp(-4.862 * kT * (1 - 0.2096 * kT + 0.00170 * kT * kT - 0.00968 * kT* kT* kT));

                var kSum = Convert.ToSingle(k + ka);
                var dh = 2000000;
                var deltaT = Convert.ToSingle((1 - Math.Exp(-kSum * dh)) * t * Math.Exp(-deg1));
                Tb += deltaT;
                deg1 += kSum * dh;
            }

            return Tb;
        }

        public static float[] GetTb(List<float> impactParameters, float wavelength, BifrostModel modelT)
        {
            var modelNe = new BifrostModel(Constants.LgNe);
            var Tbs = new float[impactParameters.Count];

            for (var i = 0; i < impactParameters.Count; i++)
            {
                var impactParameter = impactParameters[i];
                var coords = Trajectory.GetCoords(impactParameter, modelT);
                var trajectoryT = Trajectory.GetTrajectory(coords, modelT);
                var trajectoryNe = Trajectory.GetTrajectory(coords, modelNe).Normalize(1000000);
                var tb = GetTbForImpactParameter(coords, wavelength, trajectoryT, trajectoryNe);
                Tbs[i] = tb;
                GC.Collect();
            }

            return Tbs;
        }
    }
}