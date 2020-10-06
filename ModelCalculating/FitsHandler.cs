using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using TA.ObjectOrientedAstronomy.FlexibleImageTransportSystem;  //я просто молюсь на создателей этой библиотеки

namespace ModelCalculating
{
    public class FitsHandler
    {
        /// <summary>
        ///     Выводит в консоль хэдер файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        public void PrintHeader(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException($"Указанный путь {filePath} должен являться путем к файлу");
            var header = GetHeader(filePath);
            foreach (var headerRecord in header.HeaderRecords)
            {
                Console.WriteLine($"{headerRecord.Keyword}\t{headerRecord.Value}\t{headerRecord.Comment}");
            }
            Console.WriteLine("end of a header------------------------------------------------------");
        }

        //to do: превратить этот метод в асинхронный
        /// <summary>
        ///     (Не) Асинхронно сохраняет на диск хэдеры файлов в формате .csv
        /// </summary>
        /// <param name="sourcePath">Если указанный путь является директорией, то будут сохранены
        /// хэдеры всех файлов, содержащихся в ней</param>
        /// /// <param name="headerPath">Путь, куда должны быть сохранены получанные файлы</param>
        public void SaveHeaderAsync(string sourcePath, string headerPath)
        {
            var files = GetFitsFiles(sourcePath);

            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                using var destinationStream = File.CreateText(Path.Combine(headerPath, $"{fileName}_header.csv"));
                var header = GetHeader(file);

                foreach (var headerRecord in header.HeaderRecords)
                {
                    destinationStream.Write($"{headerRecord.Keyword},{headerRecord.Value},{headerRecord.Comment}\n");
                }
            }
        }

        private FitsHeader GetHeader(string filePath)
        {
            var reader = new FitsReader(File.Open(filePath, FileMode.Open));
            var headerTask = reader.ReadPrimaryHeader();
            return headerTask.Result;
        }

        private string[] GetFitsFiles(string path)
        {
            var isDirectory = Directory.Exists(path);
            if (!isDirectory && !File.Exists(path))
                throw new ArgumentException($"Не найдено: {path}"); 

            var fileNames = isDirectory ? Directory.GetFiles(path) : new[] {path};
            if (fileNames.Length < 1) 
                throw new WarningException($"Не найдены файлы в {path}");
            return fileNames;
        }

        public void GetData(string path)
        {
            var reader = new FitsReader(File.Open(path, FileMode.Open));

            var a = reader.ReadPrimaryHeaderDataUnit().Result;
            var b = a.;
            Console.WriteLine(b);
        }
    }
}