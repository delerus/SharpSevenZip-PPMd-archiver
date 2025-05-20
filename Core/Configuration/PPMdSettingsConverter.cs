using SevenZipSharpArchiver.Core.Models;
using System;
using System.Collections.Generic;

namespace SevenZipSharpArchiver.Core.Configuration
{
    public static class PPMdSettingsConverter
    {
        public static Dictionary<string, string> ToSharpSevenZipParams(PPMdSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var parameters = new Dictionary<string, string>
            {
                { "o", settings.ModelOrder.ToString() },       // Порядок модели (2-16)
                { "mem", $"{settings.MemorySizeMB}m" },        // Размер памяти в MB (1-2048)
            };

            return parameters;
        }

    }
}