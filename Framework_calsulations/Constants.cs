namespace ModelCalculations
{
    public static class Constants
    {
        public const string LgT = @"C:\Users\julie\source\repos\Sun Model\Data\3D_logT\BIFROST_ch024031_by200bz005_lgtg_249.fits";
        public const string LgNe = @"C:\Users\julie\source\repos\Sun Model\Data\3D_logne\BIFROST_ch024031_by200bz005_lgne_249.fits";

        public const double RSun = 695510;
        public const double e = 4.8032e-10;
        public const double m_e = 9.10938356e-28;
        public const double c = 2.99792458e10;
        public const double h = 6.626176e-27;
        public const double kB = 1.38067e-16;
        public const double Eh = 13.62323824 * 1.6e-12;
        public static double HMax = FitsHandler.GetMaxInHdu1(LgT) * 1000;
        public static double MaxImpactParameter = 1 + HMax / RSun;
        public static int StartIndex = FitsHandler.GetStartIndex(LgT);
    }
}