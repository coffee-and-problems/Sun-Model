using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using nom.tam.fits;

namespace ModelCalculations
{
    public static class FitsHandler
    {
        /// <summary>
        ///     Выводит в консоль хэдер файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        public static void PrintHeader(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException($"Указанный путь {filePath} должен являться путем к файлу");
            var header = GetHeader(filePath);

            for (var i = 0; i < header.NumberOfCards; i++)
            {
                var headerRecord = header.GetCard(i);
                Console.WriteLine(headerRecord);
            }

            Console.WriteLine("end of a header------------------------------------------------------");
        }

        private static Header GetHeader(string filePath)
        {
            var fits = new Fits(File.Open(filePath, FileMode.Open));
            var hdu = fits.GetHDU(0);

            return hdu.Header;
        }

        /// <summary>
        ///     Возвращает массив fits файлов, лежащих по переданному пути
        /// </summary>
        /// <param name="filePath">Путь (дериктория или файл)</param>
        public static string[] GetFitsFiles(string path)
        {
            var isDirectory = Directory.Exists(path);
            if (!isDirectory && !File.Exists(path))
                throw new ArgumentException($"Не найдено: {path}"); 

            var fileNames = isDirectory ? Directory.GetFiles(path) : new[] {path};
            if (fileNames.Length < 1) 
                throw new WarningException($"Не найдены файлы в {path}");
            return fileNames.Where(file => Path.GetExtension(file) == "fits").ToArray();
        }

        /// <summary>
        ///     Возвращает массив данных
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="hduNumber">Номер hdu</param>
        public static object GetData(string filePath, int hduNumber)
        {
            var fits = new Fits(File.Open(filePath, FileMode.Open));
            var hdu = fits.GetHDU(hduNumber);
            return hdu.Data.DataArray; //double[][][] если hduNumber = 0, иначе double[]
        }

        /// <summary>
        ///     Переводит данные из логарифмического масштаба в линейный
        /// </summary>
        /// <param name="logData">Исходный трехмерный массив</param>
        /// <param name="logBase">Основание логарифма (по умолчанию 10)</param>
        /// <param name="sliceNumber">Количество срезов исходных данных (по умолчанию 1)</param>
        public static double[][] TransformToLinearData(double[][][] logData, double logBase = 10, int sliceNumber = 1)
        {
            var linearData = new double[logData[1].Length][];

            for (var h = 0; h < logData[1].Length; h++)
            {
                var linearSlice = new double[logData[0].Length * sliceNumber];

                for (var z = 0; z < sliceNumber; z++)
                {
                    for (var i = Constants.StartIndex; i < logData[0].Length; i++)
                    {
                        linearSlice[i + logData[0].Length * z] = Math.Pow(logBase, logData[h][475 + z][i]);
                    }
                }

                linearData[h] = linearSlice;
            }

            return linearData;
        }

        /// <summary>
        ///     Возвращает максимальный элемент массива, содержащегося в hdu
        /// </summary>
        /// <param name="filePath">Путь к fits-файлу</param>
        public static double GetMaxInHdu1(string filePath)
        {
            var fits = new Fits(File.Open(filePath, FileMode.Open));
            var data = fits.GetHDU(1).Data.DataArray as double[];
            return data.Max();
        }

        /// <summary>
        ///     Возвращает максимальный элемент массива, содержащегося в hdu
        /// </summary>
        /// <param name="filePath">Путь к fits-файлу</param>
        public static int GetStartIndex(string filePath)
        {
            var fits = new Fits(File.Open(filePath, FileMode.Open));
            var data = fits.GetHDU(1).Data.DataArray as double[];
            var index = 0;

            for (; data[index] < 0; index++) {} //да, я знаю о бинарном поиске, но тут и так сойдет

            return index;
        }
    }
}