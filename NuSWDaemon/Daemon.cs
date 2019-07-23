﻿using System;
using System.IO;
using System.Threading;

namespace sw_part_auto_test
{
    class Daemon
    {
        public static void Start()
        {
            
            var blempDDOpath = "C:\\Users\\bolinger\\Desktop\\test install\\programFiles\\blemp\\DDO.blemp";
            var programStatePath = "C:\\Users\\bolinger\\Desktop\\test install\\programFiles\\config\\SWmicroservice.config";
            var programState = "0";

            string current = null;
           string compare = null;
            
            do {
                
            var rawBlempString = Blemp.LoadDDO(blempDDOpath);
                
                if (rawBlempString == null)
                {
                    return;
                }

                if (string.Compare(rawBlempString, "") != 0)
                {
                }

                // Console.Read();

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
                                Config.equationManager,
                                equation
                                );
                            
                            SWEquation.Build(
                                Config.model
                                );

                            SWEquation.DeleteEquation(
                                Config.equationManager
                                , 0);
                                
                        }
                        
                        
                    } catch(ArgumentOutOfRangeException){ }
                    
                }

                Thread.Sleep(300);

               programState = GetProgramState(programStatePath);

                if(programState == null)
                {
                    return;
                }

           } while (string.Equals(programState, "0"));

        }

        private static string GetProgramState(string path)
        {
            try
            {
                var programState = File.ReadAllText(path).Substring(0, 1);

                return programState;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
