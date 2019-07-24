﻿using NuSWDaemon;
using SolidWorks.Interop.sldworks;
using System;
using System.IO;
using System.Threading;

namespace sw_part_auto_test
{
    class Daemon
    {
        public static void Start(ModelDoc2 model,
            EquationMgr equationManager, String blempDDOpath,
            String programStatePath)
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
            var programState = new bool[] { false, false };

            Console.WriteLine(" -- Daemon - Start --");

            do {
                programState = RunState.GetProgramStates(programStatePath);
                
                // RunStateCalltoActionDebugPrompt(programStatePath);
                
                if (programState[1]) {
                    var rawBlempString = Blemp.LoadDDTO(blempDDOpath);

                    // BlempLoadDDTOdebugPrompt(rawBlempString);
                    
                    if (rawBlempString != null)
                    {
                        var equationSegments = Blemp.GetDDTOequationSegments(rawBlempString);
                        
                        if (equationSegments != null)
                        {
                            Console.WriteLine(" - Valid Equations Found - Processing");
                            
                            SWEquation.AddEquation(
                                equationManager,
                                equationSegments[0]
                                );
                            
                            SWEquation.Build(
                                model
                                );
                            Console.WriteLine(" ... Press Any Key to Continue");
                            Console.ReadLine();
                            /*
                            SWEquation.DeleteEquation(
                                equationManager,
                                0);
                                */
                        } else
                        {
                            Console.WriteLine(" - WARNING - No Valid Equations for Processing Found");
                        }
                        
                        var writeSuccess = false;
                        var timeOut = 5;

                        do {
                            Console.WriteLine(" - Closing Call to Action Semaphore");

                            writeSuccess = FileWrite.WriteStringToFileFalseOnFail(
                                programStatePath, "01"
                                );

                            Thread.Sleep(300);
                                } while (!writeSuccess && timeOut-- > 0);
                        
                        if (timeOut > 0)
                        {
                            Console.WriteLine(" - Call to Action Semaphore - Successfuly Closed");
                        } else
                        {
                            Console.WriteLine(" - ERROR - Could Not Write Call to Action Close Command");

                            Console.WriteLine(" - Exiting Daemon -");

                            Console.WriteLine(" -- Daemon - Exit --");

                            return;
                        }
                    }
                    
                }
                
                Thread.Sleep(300);
                
           } while (programState[0]);
           
            Console.WriteLine(" -- Daemon - Exit --");
        }

        private static void BlempLoadDDTOdebugPrompt(string rawBlempString)
        {
            if (rawBlempString == null)
            {
                Console.WriteLine(" - Blemp DDTO - Empty");

                Console.WriteLine(" ... Press Any Key to Continue");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine(" - Blemp DDTO - Contents:");

                Console.WriteLine(rawBlempString);

                Console.WriteLine(" ... Press Any Key to Continue");
                Console.ReadLine();
            }
        }

        private static void RunStateCalltoActionDebugPrompt(string programStatePath)
        {
            if (RunState.GetProgramStates(programStatePath)[1])
            {
                Console.WriteLine(" - Call to Action State - True");

                Console.WriteLine(" ... Press Any Key to Continue");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine(" - Call to Action State - False");

                Console.WriteLine(" ... Press Any Key to Continue");
                Console.ReadLine();
            }
        }
    }
}
