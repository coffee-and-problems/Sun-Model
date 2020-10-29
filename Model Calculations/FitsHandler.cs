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
            using var file = File.Open(filePath, FileMode.Open);
            var fits = new Fits(file);
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
            using var file = File.Open(filePath, FileMode.Open);
            var fits = new Fits(file);
            var hdu = fits.GetHDU(hduNumber);
            return hdu.Data.DataArray; //float[][][] если hduNumber = 0, иначе float[]
        }
    }
}