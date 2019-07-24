﻿using System;
using System.IO;

namespace sw_part_auto_test
{
    class Blemp
    {
        public static string LoadDDTO(string path)
        {
           
            try
            {
               var rawBlempString = File.ReadAllText(path);

                return rawBlempString.CompareTo("") == 0 ? null : rawBlempString;
            } catch (Exception exception)
            {
                Console.WriteLine(exception);

                return null;
            }
            
        }

        public static string[] GetDDTOequationSegments(string DDTOdata)
        {
            if (DDTOdata.Contains("$"))
            {
                Console.WriteLine(" - DDTO Data Valid - Begin Processing");
                /*
                string[] equationSegments = DDTOdata.Split("$");

                if (equationSegments.Length > 1)
                {
                    Config.DDO.Clear();

                    for (var i = 0; i < equationSegments.Length; ++i)
                    {
                        Config.DDO.Add(equationSegments[i]);
                    }
                }
                */

                return new string[] { };
            }
            else
            {
                Console.WriteLine(" - ERROR - DDTO Data - Invalid Format");

                return null;
            }
        }
    }
}
