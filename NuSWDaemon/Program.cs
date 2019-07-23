using NuSWDaemon;
using SolidWorks.Interop.sldworks;
using System;
using System.Diagnostics;
using System.IO;

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

            var PROG_ID = "SldWorks.Application.24";
            
            var swType = Type.GetTypeFromProgID(PROG_ID);

            if (swType == null)
            {
                Console.WriteLine(errorMessage + "Could not Get SW Type");

                return;
            }

            Console.WriteLine(" - SolidWorks Type Retrieved");
            
            var swApp = (ISldWorks) Activator.CreateInstance(swType);

            if (swApp == null)
            {
                Console.WriteLine(errorMessage + "Could not Instantiate SW Instance");

                return;
            }

            Console.WriteLine(" - SolidWorks App Instance Created");
            
            var blobPath = "C:\\Users\\bolinger\\Desktop\\test install\\C-HSSX.blob.SLDPRT";

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

            Daemon.Start(
                model,
                equationManager,
                validatedPathBlempDDO,
                validatedPathProgramState
            );

            Console.WriteLine(" ... Press Any Key to End Program");
            Console.Read();

            Console.WriteLine(" - Closing All Open SolidWorks Documents");
            swApp.CloseAllDocuments(true);

            Console.WriteLine("TOPP App SolidWorks C# Daemon Exit");
        }

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
    }
}
