using System;
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
                
                var rawEquationSegments = DDTOdata.Split("$");

                Console.WriteLine(" - DDTO Raw Equation Segments: ");

                foreach(string segment in rawEquationSegments) {
                    Console.WriteLine(segment);
                }

                var equationSegments = new string[rawEquationSegments.Length - 1];

                for(var i = 0; i < equationSegments.Length; ++i)
                {
                    equationSegments[i] = rawEquationSegments[i];
                }

                Console.WriteLine(" - DDTO Trimmed Equation Segments: ");

                foreach(string segment in equationSegments)
                {
                    Console.WriteLine(segment);
                }

                return equationSegments;
            }
            else
            {
                Console.WriteLine(" - WARNING - DDTO Data - Invalid Format");

                return null;
            }
        }
    }
}
