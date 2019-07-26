using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NuSWDaemon
{
    class InstallRoot
    {
        internal static string GetInstallRoot()
        {
            var fullPath = Path.GetFullPath(".");

            var pathChunks = fullPath.Split("\\");

            return "C:\\Users\\" + pathChunks[2] + "\\Desktop\\";
        }
    }
}
