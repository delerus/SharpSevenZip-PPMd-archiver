using System;
using SharpSevenZip;

namespace SevenZipSharpArchiver.Core.Models
{
    public class PPMdSettings
    {
        // Порядок контекста (2-16).
        public int ModelOrder { get; set; } = 16;

        // Лимит памяти (в МБ)
        public int MemorySizeMB { get; set; } = 2048;

        public TextComplexity Complexity { get; set; } = TextComplexity.Medium;
    }
    public enum TextComplexity
    {
        Low,       // Логи, JSON
        Medium,    // Обычный текст
        High       // Исходный код
    }

}
