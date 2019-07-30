using System;
using System.IO;

namespace NuSWDaemon
{
    class ToppFiles
    {
        internal static bool ValidateDirectory(string path, string name)
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine(name + " Directory Not Found - Attempting to Create");

                Directory.CreateDirectory(path);

                return Directory.Exists(path);
            }
            else
                return true;
        }

        internal static bool ValidateFile(string path, string name)
        {
            if (!File.Exists(path))
            {
                
                Console.WriteLine(name + " File Not Found - Attempting to Create");

                File.Create(path).Close();

                return File.Exists(path);
            }
            else
            {
                return true;
            }
        }
    }
}
