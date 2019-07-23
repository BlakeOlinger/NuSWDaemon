using NuSWDaemon;
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
            Console.WriteLine(" -- Daemon - Start --");
            
            do {

                // RunStateCalltoActionDebugPrompt(programStatePath);

                if (RunState.TrueForStringCharacterZero(programStatePath, 1, 1)) {
                    var rawBlempString = Blemp.LoadDDTO(blempDDOpath);

                    // BlempDDTOdebugPrompt(rawBlempString);

                    if (rawBlempString != null)
                    {
                        Blemp.PopulateDDO(rawBlempString);
                        /*
                        SWEquation.AddEquation(
                            equationManager,
                            equation
                            );

                        SWEquation.Build(
                            model
                            );

                        SWEquation.DeleteEquation(
                            equationManager,
                            0);
                            */
                    }
                }

                Thread.Sleep(300);
                
           } while (RunState.TrueForStringCharacterZero(programStatePath, 0, 1));
           
            Console.WriteLine(" -- Daemon - Exit --");
        }

        private static void BlempDDTOdebugPrompt(string rawBlempString)
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
            if (RunState.TrueForStringCharacterZero(programStatePath, 1, 1))
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
