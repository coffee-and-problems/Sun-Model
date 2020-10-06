using System.Collections.Generic;

namespace ModelCalculations
{
    class Program
    {
        static void Main(string[] args)
        {
            var impactParameters = GetImpactParameters();
            var wavelengths = new [] {4.47761, 8.57143, 13.5, 17.0};
            var printer = new Printer();

            foreach (var lambda in wavelengths)
            {
                var Tb = TbCalculation.GetTb(impactParameters, lambda);
                printer.Print(impactParameters, Tb, lambda.ToString());
            }
        }

        private static List<double> GetImpactParameters()
        {
            var impactParameters = new List<double>();

            for (var lambda = 0.0; lambda < 0.95; lambda += 0.005)
            {
                impactParameters.Add(lambda);
            }
            for (var lambda = 0.95; lambda < 0.9998; lambda += 0.0002)
            {
                impactParameters.Add(lambda);
            }
            for (var lambda = 1.0; lambda < 1.00999; lambda += 0.0001)
            {
                impactParameters.Add(lambda);
            }
            for (var lambda = 1.01; lambda < 1.02; lambda += 0.0001)
            {
                impactParameters.Add(lambda);
            }

            return impactParameters;
        }
    }
}
