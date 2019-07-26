using System;
using System.IO;

namespace NuSWDaemon
{
    class RunState
    {
        internal static bool[] GetProgramStates(string path)
        {
            try
            {
                var rawString = File.ReadAllText(path);
                var programStates = new bool[] { false, false };

                programStates[0] = rawString.Substring(0, 1).CompareTo("0") == 0;
                programStates[1] = rawString.Substring(1, 1).CompareTo("0") == 0;

                return programStates;
            } catch (Exception exception)
            {
                Console.WriteLine(exception);

                Console.WriteLine(" ... Press Any Key to Continue");
                Console.ReadLine();

                return new bool[] { };
            }
        }
    }
}
