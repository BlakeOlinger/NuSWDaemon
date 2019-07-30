using System;
using System.Diagnostics;
using System.IO;

namespace NuSWDaemon
{
    class BlobDatabase
    {

        internal static bool ValidateBlobLocalDatabase(string path, string installDirectory)
        {
            Console.WriteLine("Checking for Install - " + path);

            if (!Directory.Exists(path))
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
                        + " && git clone https://github.com/BlakeOlinger/blob ";

                    process.StartInfo.Arguments = argument;

                    process.Start();

                    process.WaitForExit();

                    return Directory.Exists(path);
                }

            } else
            {
                SyncLocalDatabase(installDirectory);
            }

            return true;
        }

        private static void SyncLocalDatabase(string installDirectory)
        { 
            if (!File.Exists(installDirectory + "\\toppAppDBdaemon.jar")) {

                Console.WriteLine("Syncing local database instance with remote");

                if (Internet.CheckClientConnection())
            {
                    using (var process = new Process())
                    {
                        process.StartInfo.UseShellExecute = true;

                        process.StartInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
                        var argument = "/c cd " + installDirectory + "\\blob\\"
                            + " && git pull ";

                        process.StartInfo.Arguments = argument;

                        process.Start();

                        process.WaitForExit();

                        Console.WriteLine("Local database instance updated");
                    }
                } else
                {
                    Console.WriteLine("Could not sync local database instance with remote - no internent connection found");
                }
            }
        }
    }
}
