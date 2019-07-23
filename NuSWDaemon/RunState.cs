using System;
using System.IO;

namespace NuSWDaemon
{
    class RunState
    {
        internal static bool GetRunState(string path)
        {
            try
            {
                return File.ReadAllText(path).Substring(0, 1).CompareTo("0") == 0;
            } catch (ArgumentOutOfRangeException exception)
            {
                Console.WriteLine(exception);

                return false;
            }
        }
    }
}
