using System;
using System.Collections.Generic;
using System.IO;
using SharpSevenZip;
using SevenZipSharpArchiver.Core;

namespace SevenZipSharpArchiver
{
    class PPMdArchiver
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SharpSevenZip Archiver");
            Console.WriteLine("-------------------");

            if (args.Length != 2)
            {
                Console.WriteLine("Использование: SevenZipSharpArchiver.exe 'inputFile' 'outputFile'");
                Console.ReadKey();
                return;
            }

            string inputFile = args[0];
            string outputFile = args[1];

            try
            {
                var archiver = new ArchiverManager(inputFile, outputFile);
                archiver.Init();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Console.WriteLine("-------------------");
            Console.WriteLine("Нажмите любую кнопку для выхода..");
            Console.ReadKey();
        }
    }
}