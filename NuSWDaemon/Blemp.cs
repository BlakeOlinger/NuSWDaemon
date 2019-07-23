using System;
using System.IO;

namespace sw_part_auto_test
{
    class Blemp
    {
        public static string LoadDDO(string path)
        {
            try
            {
               var rawBlempString = File.ReadAllText(path);

                return rawBlempString;
            } catch(ArgumentNullException exception)
            {
                return null;
            } catch (ArgumentException exception)
            {
                return null;
            } catch (FileNotFoundException exception)
            {
                return null;
            } catch (Exception exception)
            {
                return null;
            }
        }

        public static void PopulateDDO(string DDOdata)
        {
           
            string[] equationSegments = DDOdata.Split("$");

            if (equationSegments.Length > 1)
            {
                Config.DDO.Clear();

                for (var i = 0; i < equationSegments.Length; ++i)
                {
                    Config.DDO.Add(equationSegments[i]);
                }
            }
        }
    }
}
