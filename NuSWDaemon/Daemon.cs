using NuSWDaemon;
using SolidWorks.Interop.sldworks;
using System;
using System.IO;
using System.Threading;

namespace sw_part_auto_test
{
    class Daemon
    {
        private static ISldWorks swApp = null;

        public static void Start(string installDirectory, string programStatePath,
            string GUIconfigPath, string DDTOpath)
        {
            /*
             * This program listens for a 'call to action bool'
             *  - the second character in a string from the
             *  - run state config file for this MS
             * On TRUE it reads the DDTO, which the GUI MS
             * will have sent the current set of expressions to
             * evaluate, then set the GUI's call to action state
             * to false - meaning it cannot write to the DDTO,
             * though it will store the command and do it asap,
             * then this daemon will read the DDTO and implement
             * the expressions in it, after it resets its own
             * and the GUI's call to action, turning itself off
             * and the GUI's write ability on
             */
            RunState programState;

            Console.WriteLine(" -- Daemon - Start --");
            try { 

            do {
                programState = RunState.GetProgramState(programStatePath);

                    if (programState.GetActionState())
                    {
                        CloseGUIsemaphore(GUIconfigPath);

                        var PROG_ID = "SldWorks.Application.24";

                        var swType = Type.GetTypeFromProgID(PROG_ID);

                        ValidateSWtype(swType);

                        swApp = (ISldWorks)Activator.CreateInstance(swType);

                        CreateSWAppInstance(swApp);

                        var blobName = programState.GetBlobName();

                        var blobPath = installDirectory + "blob\\" + blobName;

                        DocumentSpecification documentSpecification =
                            SWDocSpecification.GetDocumentSpecification(swApp, blobPath);

                        if (documentSpecification == null)
                        {
                            Console.WriteLine("Could not Get Document Specification for - " +
                                blobPath);

                            return;
                        }

                        Console.WriteLine(" - Obtained Document Specification for - " + blobPath);

                        var model = swApp.OpenDoc7(
                            documentSpecification);

                        if (model == null)
                        {
                            Console.WriteLine(" - ERROR - Could not Open Document - " + blobPath);

                            return;
                        }

                        Console.WriteLine(" - Opened SolidWorks Document - " + blobPath);

                        EquationMgr equationManager = model.GetEquationMgr();

                        if (equationManager == null)
                        {
                            Console.WriteLine("Could Not Get Equation Manager Instance");

                            Console.WriteLine(" - Closing All Open SolidWorks Documents");
                            swApp.CloseAllDocuments(true);

                            return;
                        }

                        Console.WriteLine(" - Created Equation Manager Instance");

                        var rawBlempString = Blemp.LoadDDTO(DDTOpath);
                        
                        var writeSuccess = false;
                        var timeOut = 5;

                        if (rawBlempString != null)
                        {
                            var equationSegments = Blemp.GetDDTOequationSegments(rawBlempString);

                            if (equationSegments != null)
                            {
                                Console.WriteLine(" - Valid Equations Found - Processing");

                                try
                                {
                                    for (var i = 0; i < equationSegments.Length; ++i)
                                    {
                                        SWEquation.AddEquation(
                                            equationManager,
                                            equationSegments[i]
                                            );

                                        SWEquation.Build(
                                            model
                                            );

                                        SWEquation.DeleteEquation(
                                            equationManager,
                                            0);
                                    }
                                }
                                catch (ArgumentOutOfRangeException exception)
                                {
                                    Console.WriteLine(exception);
                                }

                            }
                            else
                            {
                                Console.WriteLine(" - WARNING - No Valid Equations for Processing Found");
                            }

                        } else
                        {
                            Console.WriteLine("No Equations Found in DDTO");
                        }

                        do
                        {
                            Console.WriteLine(" - Closing Call to Action Semaphore");

                            writeSuccess = FileWrite.WriteStringToFileFalseOnFail(
                                programStatePath, "011!"
                                );

                            FileWrite.WriteStringToFileFalseOnFail(DDTOpath, "");

                            Thread.Sleep(300);
                        } while (!writeSuccess && timeOut-- > 0);

                        if (timeOut > 0)
                        {
                            Console.WriteLine(" - Call to Action Semaphore - Successfuly Closed");
                        }
                        else
                        {
                            Console.WriteLine(" - ERROR - Could Not Write Call to Action Close Command");

                            Console.WriteLine(" - Exiting Daemon -");

                            Console.WriteLine(" -- Daemon - Exit --");

                            return;
                        }

                        timeOut = 5;

                        do
                        {
                            Console.WriteLine(" - Opening GUI Call to Action Semaphore");

                            writeSuccess = FileWrite.WriteStringToFileFalseOnFail(
                               GUIconfigPath, "00"
                               );

                            Thread.Sleep(300);
                        } while (!writeSuccess && timeOut-- > 0);
                    }

                    if (programState.GetCloseBlob())
                    {
                        CloseGUIsemaphore(GUIconfigPath);

                        swApp.CloseAllDocuments(true);

                        FileWrite.WriteStringToFileFalseOnFail(programStatePath, "011!");

                        Console.WriteLine(" - Opening GUI Call to Action Semaphore");

                        FileWrite.WriteStringToFileFalseOnFail(
                               GUIconfigPath, "00");
                    }

                    Thread.Sleep(300);
                
           } while (programState.GetRunState());

        } catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }

            if (swApp != null)
                swApp.CloseAllDocuments(true);

    Console.WriteLine(" -- Daemon - Exit --");
        }

        private static void CloseGUIsemaphore(string path)
        {
            Console.WriteLine("Closing GUI Semaphore");

            if (File.ReadAllText(path).Substring(1, 1).CompareTo("1") != 0)
            {
                File.WriteAllText(path, "01");
            }
        }

        private static void CreateSWAppInstance(ISldWorks sldWorks)
        {
            if (sldWorks == null)
            {
                Console.WriteLine(" - ERROR - Could not Instantiate SW Instance");

                return;
            }

            Console.WriteLine(" - SolidWorks App Instance Created");
        }

        private static void ValidateSWtype(Type type)
        {
            if (type == null)
            {
                Console.WriteLine(" - ERROR - Could not Get SW Type");

                return;
            }

            Console.WriteLine(" - SolidWorks Type Retrieved");
        }
    }
}
