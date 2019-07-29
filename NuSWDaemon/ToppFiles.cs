using System;
using System.Diagnostics;
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
        
        internal static bool ValidateBlobLocalDatabase(string path, string installDirectory)
        {
            Console.WriteLine("Checking for Install - " + path);

            if(!Directory.Exists(path))
            {
                Console.WriteLine("Checking Client Internet Connection");

                if (!Internet.CheckClientConnection())
                {
                    Console.WriteLine("No Internet Connection Found");

                    return false;
                }

                Console.WriteLine("Installing local blob database instance");

                using (var process = new Process())
                {
                    process.StartInfo.UseShellExecute = true;

                    process.StartInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
                    var argument = "/c cd " + installDirectory 
                        + " && git clone https://github.com/BlakeOlinger/blob " ;
                   
                    process.StartInfo.Arguments = argument;

                    process.Start();

                    process.WaitForExit();

                    return Directory.Exists(path);
                }

            }

            return true;
        }
    }
}
