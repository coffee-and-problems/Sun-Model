using System;
using System.IO;

namespace ModelCalculating
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"C:\Users\julie\source\repos\Sun Model\Data\2D_logT\BIFROST_en096014_gol_lgtg_281.fits";
            var fh = new FitsHandler();
            fh.GetData(filePath);
        }
    }
}
