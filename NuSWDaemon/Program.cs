using SolidWorks.Interop.sldworks;
using System;

namespace sw_part_auto_test
{
    public class Program
    {
        static void Main(string[] args)
        {
            var errorMessage = " ERROR - Program Exit Prematurely - ";

            Console.WriteLine("TOPP App SolidWorks C# Daemon Start");

            var PROG_ID = "SldWorks.Application.24";
            
            var swType = Type.GetTypeFromProgID(PROG_ID);

            if (swType == null)
            {
                Console.WriteLine(errorMessage + "Could not Get SW Type");

                return;
            }

            Console.WriteLine(" - SolidWorks Type Retrieved");
            
            var swApp = (ISldWorks) Activator.CreateInstance(swType);

            if (swApp == null)
            {
                Console.WriteLine(errorMessage + "Could not Instantiate SW Instance");

                return;
            }

            Console.WriteLine(" - SolidWorks App Instance Created");
            
            var path = "C:\\Users\\bolinger\\Desktop\\test install\\C-HSSX.blob.SLDPRT";

            DocumentSpecification documentSpecification =
                SWDocSpecification.GetDocumentSpecification(swApp, path);

            if (documentSpecification == null)
            {
                Console.WriteLine(errorMessage + "Could not Get Document Specification for - " +
                    path);

                return;
            }

            Console.WriteLine(" - Obtained Document Specification for - " + path);
            
            var model = (ModelDoc2) swApp.OpenDoc7(
                documentSpecification);

            if (model == null)
            {
                Console.WriteLine(" - ERROR - Could not Open Document - " + path);

                return;
            }

            Console.WriteLine(" - Opened SolidWorks Document - " + path);
            /*
            Config.model = model;

            EquationMgr equationManager = model.GetEquationMgr();

            if (equationManager == null)
            {
                return;
            }

            Config.equationManager = equationManager;

            Daemon.Start();
            */

            Console.Read();

            Console.WriteLine(" - Closing All Open SolidWorks Documents");

            swApp.CloseAllDocuments(true);

            Console.WriteLine("TOPP App SolidWorks C# Daemon Exit");
        }
    }
}
