using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ModelCalculations
{
    class Program
    {
        static void Main(string[] args)
        {
            var impactParameters = GetImpactParameters();
            var wavelengths = new [] {4.47761f, 8.57143f, 13.5f, 17.0f};
            var printer = new Printer();
            //var modelT = new BifrostModel(Constants.LgT);

            foreach (var lambda in wavelengths)
            {
                //var Tb = TbCalculation.GetTb(impactParameters, lambda, modelT);
                printer.Save(impactParameters, $"_{lambda}");
            }
        }

        private static List<float> GetImpactParameters()
        {
            var impactParameters = new List<float>();

            for (var lambda = 0.0f; lambda < 0.05f; lambda += 0.005f)
            {
                impactParameters.Add(lambda);
            }
            for (var lambda = 0.0f; lambda < 0.95f; lambda += 0.005f)
            {
                impactParameters.Add(lambda);
            }
            for (var lambda = 0.95f; lambda < 0.9998f; lambda += 0.0002f)
            {
                impactParameters.Add(lambda);
            }
            for (var lambda = 1.0f; lambda < 1.00999f; lambda += 0.0001f)
            {
                impactParameters.Add(lambda);
            }
            for (var lambda = 1.01f; lambda < 1.02f; lambda += 0.0001f)
            {
                impactParameters.Add(lambda);
            }

            return impactParameters;
        }
    }
}
