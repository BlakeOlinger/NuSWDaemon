using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NuSWDaemon
{
    class FileValidate
    {
        internal static string CheckAndReturnString(string path) 
            => File.Exists(path) ? path : null ;
    }
}
