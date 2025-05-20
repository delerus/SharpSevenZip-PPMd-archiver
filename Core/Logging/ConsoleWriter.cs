using System;
using System.Diagnostics;
using System.IO;

namespace SevenZipSharpArchiver.Core.Logging
{
    public static class ConsoleWriter
    {
        private static string _inputFilePath;
        private static string _outputFilePath;
        private static Stopwatch _stopwatch;

        public static void Init(string inputFilePath, string outputFilePath)
        {
            _inputFilePath = inputFilePath;
            _outputFilePath = outputFilePath;
            _stopwatch = new Stopwatch();
        }

        public static void StartTimer()
        {
            _stopwatch?.Restart();
        }

        public static void StopTimerAndWriteInfo(OperationType operationType)
        {
            _stopwatch?.Stop();
            var elapsed = _stopwatch?.Elapsed ?? TimeSpan.Zero;

            if (operationType == OperationType.Compression)
            {
                var originalSize = new FileInfo(_inputFilePath).Length;
                var processedSize = new FileInfo(_outputFilePath).Length;

                WriteInfo(originalSize, processedSize, operationType, elapsed);
            }

            if (operationType == OperationType.Decompression)
            {
                var originalSize = new FileInfo(_inputFilePath).Length;
                long processedSize = 0;

                WriteInfo(originalSize, processedSize, operationType, elapsed);
            }
        }

        public static void WriteInfo(long originalSize, long processedSize, OperationType operationType, TimeSpan elapsedTime)
        {
            var ratio = (double)processedSize / originalSize * 100;
            var operationName = operationType == OperationType.Compression ? "архивации" : "распаковки";

            Console.WriteLine($"Операция {operationName} завершена успешно");
            Console.WriteLine($"Затраченное время: {elapsedTime:mm\\:ss\\.fff}");

            if (operationType == OperationType.Compression)
            {
                Console.WriteLine($"Исходный размер: {originalSize:N0} байт");
                Console.WriteLine($"Размер архива: {processedSize:N0} байт");
                Console.WriteLine($"Степень сжатия: {ratio:F2}%");
            }
            else
            {
                
            }
        }
    }

    public enum OperationType
    {
        Compression,
        Decompression
    }
}