using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NuSWDaemon
{
    class ToppFiles
    {
        internal static bool validateDirectory(string path, string name)
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
        
        internal static bool validateBlobFile(string path)
        {
            Console.WriteLine("Checking for Install - " + path);

            if(!File.Exists(path))
            {
                Console.WriteLine("Checking Client Internet Connection");

                if (!Internet.CheckClientConnection())
                {
                    Console.WriteLine("No Internet Connection Found");

                    return false;
                }
                Console.WriteLine("Installing local blob database instance");

                // how to make the git process -
                // https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process.start?view=netframework-4.8

                using (var process = new Process())
                {
                    process.StartInfo.UseShellExecute = true;

                    process.StartInfo.FileName = "C:\\Windows\\System32\\cmd.exe";

                    // this is how you pas arguments - git didn't work
                    process.StartInfo.Arguments = "/c cd " + path;
                    // https://github.com/BlakeOlinger/blob
                    // - this creates a folder 'blob' - remove the autocreate from program
                    // and add that functionality to the this

                    process.Start();
                }

                return false;
            }

            return false;
        }
    }
}
