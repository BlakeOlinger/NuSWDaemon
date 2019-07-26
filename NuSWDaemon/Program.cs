using NuSWDaemon;
using SolidWorks.Interop.sldworks;
using System;
using System.Diagnostics;

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

            var errorMessage = " ERROR - Program Exit Prematurely - ";

            Console.WriteLine("TOPP App SolidWorks C# Daemon Start");

            var installRoot = InstallRoot.GetInstallRoot();

            var installDirectory = installRoot + "SolidWorks Daemon\\";

            validateInstallDirectory(installDirectory);

            var PROG_ID = "SldWorks.Application.24";
            
            var swType = Type.GetTypeFromProgID(PROG_ID);

            validateSWtype(swType, errorMessage);
            
            var swApp = (ISldWorks) Activator.CreateInstance(swType);

            createSWAppInstance(swApp, errorMessage);

            // FIXME - wouldn't make an assumption about a specific .blob file 
            // - only look for the .git that would be there no matter what

            var databaseInstallFile = ".git";

            var blobLocalDatabasePath = installDirectory + "blob\\" 
                + databaseInstallFile;

            if(!ToppFiles.validateBlobLocalDatabase(blobLocalDatabasePath, installDirectory))
            {
                Console.WriteLine("Could not get valid blob local database instance");

                return;
            }

            Console.WriteLine("Valid local blob local database instance found");

            // FIXME - set by reading .blob file names in blob directory
            // then have GUI pass a specific file request through the DDTO
            // and have this verify it exists
            // likely have to move this function inside the daemon call to action loop
            // as well as everything else related to the equation manager and model
            // related to it
            var blobName = "C-HSSX.blob.SLDPRT";

            var blobPath = installDirectory + "blob\\" + blobName;

            DocumentSpecification documentSpecification =
                SWDocSpecification.GetDocumentSpecification(swApp, blobPath);

            if (documentSpecification == null)
            {
                Console.WriteLine(errorMessage + "Could not Get Document Specification for - " +
                    blobPath);

                return;
            }

            Console.WriteLine(" - Obtained Document Specification for - " + blobPath);

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var model = (ModelDoc2)swApp.OpenDoc7(
                documentSpecification);

            stopwatch.Stop();

            if (model == null)
            {
                Console.WriteLine(" - ERROR - Could not Open Document - " + blobPath);

                Console.WriteLine(" - File Open Operation - Elapsed Time: " +
                    stopwatch.ElapsedMilliseconds + "ms");

                return;
            }

            Console.WriteLine(" - Opened SolidWorks Document - " + blobPath);

            Console.WriteLine(" - File Open Operation - Elapsed Time: " +
                    stopwatch.ElapsedMilliseconds + "ms");

            EquationMgr equationManager = model.GetEquationMgr();

            if (equationManager == null)
            {
                Console.WriteLine(errorMessage +
                    "Could Not Get Equation Manager Instance");

                Console.WriteLine(" - Closing All Open SolidWorks Documents");
                swApp.CloseAllDocuments(true);

                return;
            }

            Console.WriteLine(" - Created Equation Manager Instance");

            // this will remain in the high level Program area
            // will likely remove FileValidate class
            /*
            var programStatePath = "C:\\Users\\bolinger\\Desktop\\test install\\SWmicroservice.config";

            var validatedPathProgramState = FileValidate.CheckAndReturnString(programStatePath);

            
            ConfirmExistingOrCreateNewFile(
                swApp,
                validatedPathProgramState,
                programStatePath,
                " - Progam State Config File Found"
                );

            FileWriteAndConfirm(
                swApp,
                validatedPathProgramState, 
                "01",
                errorMessage + "Could Not Initialize Program State Config File"
                );

            Console.WriteLine(" - Program State Config File - Successfully Initialized");

            var blempDDOpath = "C:\\Users\\bolinger\\Desktop\\test install\\DDTO.blemp";

            var validatedPathBlempDDO = FileValidate.CheckAndReturnString(blempDDOpath);

            ConfirmExistingOrCreateNewFile(
                swApp,
                validatedPathBlempDDO,
                blempDDOpath,
                " - Blemp DDO File Found"
                );
                */
            // most daemon arguments will move inside the call to action loop
            // the call to action transfering specific .blob file info will mean
            // the daemon will have to close all documents before opening a new one
            // if the .blob is different than the currently opened .blob
            /*
            Daemon.Start(
                model,
                equationManager,
                validatedPathBlempDDO,
                validatedPathProgramState,
                "C:\\Users\\bolinger\\Desktop\\test install\\GUI.config"
            );
            */

            Console.WriteLine(" - Closing All Open SolidWorks Documents");
            swApp.CloseAllDocuments(true);

            Console.WriteLine("TOPP App SolidWorks C# Daemon Exit");
        }

        private static void createSWAppInstance(ISldWorks sldWorks, string message)
        {
            if (sldWorks == null)
            {
                Console.WriteLine(message + "Could not Instantiate SW Instance");

                return;
            }

            Console.WriteLine(" - SolidWorks App Instance Created");
        }

        private static void validateSWtype(Type type, string message)
        {
            if (type == null)
            {
                Console.WriteLine(message + "Could not Get SW Type");

                return;
            }

            Console.WriteLine(" - SolidWorks Type Retrieved");
        }

        private static void validateInstallDirectory(string path)
        {
            if (!ToppFiles.validateDirectory(path, "SolidWorks Daemon"))
            {
                Console.WriteLine("Could not create install directory");

                return;
            }

            Console.WriteLine("Valid Install Directory Found");
        }

        // likely replace with ToppFiles class
        /*
        private static void ConfirmExistingOrCreateNewFile(ISldWorks swApp,
            string validateFile, string expectedPath, string message)
        {
            if (validateFile == null)
            {
                try
                {
                    File.Create(expectedPath);

                    Console.WriteLine(" - Created File - " + expectedPath);
                }
                catch (IOException exception)
                {
                    Console.WriteLine(exception);

                    Console.WriteLine(" - Closing All Open SolidWorks Documents");
                    swApp.CloseAllDocuments(true);

                    return;
                }

            }
            else
                Console.WriteLine(message);
        }

        private static void FileWriteAndConfirm(ISldWorks swApp, string validatedPath,
            string writeMessage, string errorMessage)
        {
            if (!FileWrite.WriteStringToFileFalseOnFail(validatedPath, writeMessage))
            {
                Console.WriteLine(errorMessage);

                Console.WriteLine(" - Closing All Open SolidWorks Documents");
                swApp.CloseAllDocuments(true);

                return;
            }
        }
        */
    }
}
