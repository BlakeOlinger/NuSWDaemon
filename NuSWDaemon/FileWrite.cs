using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NuSWDaemon
{
    class FileWrite
    {
        internal static bool WriteStringToFileFalseOnFail(string path, string message)
        {
            try
            {
                File.WriteAllText(path, message);

                return File.ReadAllText(path).CompareTo(message) == 0;
            } catch (Exception exception)
            {
                Console.WriteLine(exception);

                return false;
            }
        }
    }
}
