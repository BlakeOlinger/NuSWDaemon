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
            Console.WriteLine(" -- Daemon - Start --");
            
            string current = null;
            string compare = null;
            
            do {
                /*
            var rawBlempString = Blemp.LoadDDO(blempDDOpath);
                
                if (rawBlempString == null)
                {
                    return;
                }

                if (string.Compare(rawBlempString, "") != 0)
                {
                }

                string blempString = "";

                try
                {
                    blempString = File.ReadAllText(blempDDOpath);
                } catch (IOException)
                {
                }

                if (string.Compare(
                    blempString == null ? "" : blempString ,
                    "") != 0)
                {
                    Blemp.PopulateDDO(rawBlempString);

                    try
                    {
                        compare = Config.DDO[1];

                        if (string.Compare(current, compare) != 0)
                        {
                            
                            current = compare;
                            
                            string equation = Config.DDO[0] + Config.DDO[1] +
                                Config.DDO[2];
                            
                            SWEquation.AddEquation(
                                equationManager,
                                equation
                                );
                            
                            SWEquation.Build(
                                model
                                );

                            SWEquation.DeleteEquation(
                                equationManager
                                , 0);
                                
                        }
                        
                        
                    } catch(ArgumentOutOfRangeException){ }
                    
                }
                */

                Thread.Sleep(300);
                
           } while (RunState.GetRunState(programStatePath));
           
            Console.WriteLine(" -- Daemon - Exit --");
        }
    }
}
