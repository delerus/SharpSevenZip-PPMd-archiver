using SharpSevenZip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenZipSharpArchiver.Core.Configuration
{
    static public class BaseLibraryLoader
    {
        static public void InitializeLibrary()
        {
            try
            {
                string[] possiblePaths =
                {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "7-Zip", "7z.dll"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "7-Zip", "7z.dll"),
                "7z.dll"
            };

                foreach (var path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        SharpSevenZipBase.SetLibraryPath(path);
                        return;
                    }
                }

                throw new FileNotFoundException("Не удалось найти 7z.dll. Убедитесь, что 7-Zip установлен.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка инициализации библиотеки: " + ex.Message);
            }
        }
    }
}
