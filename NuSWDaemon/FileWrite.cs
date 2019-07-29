using System;
using System.IO;
using System.Threading;

namespace NuSWDaemon
{
    class FileWrite
    {
        internal static bool WriteStringToFileFalseOnFail(string path, string message)
        {
            var writeSuccess = false;

            var flag = false;

            var timeout = 10;

            do
            {
                try
                {
                    File.WriteAllText(path, message);

                    flag = true;

                    writeSuccess = File.ReadAllText(path).CompareTo(message) == 0;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);

                    Thread.Sleep(300);
                }
            } while (timeout-- > 0 && !flag);

            return writeSuccess;
        }
    }
}
