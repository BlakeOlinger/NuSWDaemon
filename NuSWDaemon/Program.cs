using NuSWDaemon;
using System;

namespace sw_part_auto_test
{
    public class Program
    {
        static void Main(string[] args)
        {
            /*
             * Assumptions this program makes:
             *  - There is only one instance of a
             *  - SLDWORKS.exe process occuring
             *      If there is more than one, this program
             *      may also create an additional instance - not verified -
             *      occasionall, this program will still run but on the
             *      background process.
             *          This program DOES NOT make an instance visible
             *          therefor if this program opporates on an additional
             *          SLDWORKS.exe process it will do so invisibily
             */

            Console.WriteLine("TOPP App SolidWorks C# Daemon Start");

            var installRoot = InstallRoot.GetInstallRoot();

            var installDirectory = installRoot + "SolidWorks Daemon\\";

            ValidateInstallDirectory(installDirectory);
            
            var databaseInstallCheckFileName = ".git";

            var localBlobDatabaseInstancePath = installDirectory + "blob\\" 
                + databaseInstallCheckFileName;

            ValidateLocalBlobDatabase(localBlobDatabaseInstancePath, installDirectory);
            
            var programStateFileName = "SWmicroservice.config";

            var programStatePath =  installDirectory + programStateFileName;

            ValidateSWconfigFile(programStatePath, programStateFileName);

            InitializeFile(programStatePath, programStateFileName, "01!");
            
            var GUIconfigFileName = "GUI.config";

            var GUIconfigPath = installDirectory + GUIconfigFileName;

            ValidateGUIconfigFile(GUIconfigPath, GUIconfigFileName);

            InitializeFile(GUIconfigPath, GUIconfigFileName, "00");

            Daemon.Start(
                installDirectory,
                programStatePath,
                GUIconfigPath
            );  

            Console.WriteLine("TOPP App SolidWorks C# Daemon Exit");
        }

        private static void InitializeFile(string path, string fileName,
            string state)
        {
            if (FileWrite.WriteStringToFileFalseOnFail(path, state))
            {
                Console.WriteLine(fileName + " Initialized");
            } else
                Console.WriteLine(fileName + " Failed to Initialize");
        }

        private static void ValidateGUIconfigFile(string path, string fileName)
        {
            if (!ToppFiles.ValidateFile(path, fileName))
            {
                Console.WriteLine("Could not Create " + fileName);

                return;
            }

            Console.WriteLine(fileName + " found");
        }

        private static void ValidateSWconfigFile(string path, string fileName)
        {
            if (!ToppFiles.ValidateFile(path, fileName))
            {
                Console.WriteLine("Could not Create " + fileName);

                return;
            }

            Console.WriteLine(fileName + " found");
        }

        private static void ValidateLocalBlobDatabase(string blobDBpath,
            string installDirectory)
        {
            if (!ToppFiles.ValidateBlobLocalDatabase(blobDBpath, installDirectory))
            {
                Console.WriteLine("Could not get valid blob local database instance");

                return;
            }

            Console.WriteLine("Valid local blob local database instance found");
        }

        private static void ValidateInstallDirectory(string path)
        {
            if (!ToppFiles.ValidateDirectory(path, "SolidWorks Daemon"))
            {
                Console.WriteLine("Could not create install directory");

                return;
            }

            Console.WriteLine("Valid Install Directory Found");
        }
    }
}
