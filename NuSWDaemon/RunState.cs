using System;
using System.IO;

namespace NuSWDaemon
{
    class RunState
    {
        internal static bool TrueForStringCharacterZero(string path, int start, int end)
        {
            try
            {
                return File.ReadAllText(path).Substring(start, end).CompareTo("0") == 0;
            } catch (ArgumentOutOfRangeException exception)
            {
                Console.WriteLine(exception);

                Console.WriteLine(" -- Offending String - " + File.ReadAllText(path));

                Console.WriteLine(" ... Press Any Key to Continue");
                Console.ReadLine();

                return false;
            }
        }
    }
}
