//TODO : автоматическое построение графика

using System.Collections.Generic;
using System.IO;

namespace ModelCalculations
{
    public class Printer
    {
        public void Save(List<float> x, float[] y, string label)
        {
            using var file = new StreamWriter(Path.Combine("Results", $"{label}.csv"));
            for (var i = 0; i < x.Count; i++)
            {
                file.WriteLine($"{x[i]}, {y[i]}");
            }
        }

        public void Save(List<float> x, string label)
        {
            using var file = new StreamWriter(Path.Combine("Results", $"{label}.csv"));
            foreach (var t in x)
            {
                file.WriteLine($"{t}");
            }
        }
    }
}