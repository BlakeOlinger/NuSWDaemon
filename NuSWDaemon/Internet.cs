using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NuSWDaemon
{
    class Internet
    {
        internal static bool CheckClientConnection()
        {
            try
            {
                using (var webClient = new WebClient())

                using (webClient.OpenRead("https://www.google.com"))
                {
                    return true;
                }

            } catch
            {
                return false;
            }
        }
    }
}
