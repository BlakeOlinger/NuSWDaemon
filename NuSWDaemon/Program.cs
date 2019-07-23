using System;
using SolidWorks.Interop.sldworks;

namespace sw_part_auto_test
{
    public class Program
    {
        public static readonly string PROG_ID = "SolidWorks.Application.24";

        static void Main(string[] args)
        {

            // LiveUpdateTest();

            var swType = SWType.GetFromProgID(PROG_ID);

            if (swType == null)
            {
                return;
            }

            ISldWorks swApp = CreateSWInstance.Create(swType);

            if (swApp == null)
            {
                return;
            }

            /*
            var path = "C:\\Users\\bolinger\\Desktop\\test install\\C-HSSX.blob.SLDPRT";
            
            DocumentSpecification documentSpecification =
                SWDocSpecification.GetDocumentSpecification(swApp, path);

            if (documentSpecification == null)
            {
                return;
            }

            ModelDoc2 model = (ModelDoc2)swApp.OpenDoc7(
                documentSpecification);

            if (model == null)
            {
                logger.Error("\n ERROR: Could not get Model from " +
                    "Document Specification\n - Exiting Program");

                return;
            }

            Config.model = model;

            EquationMgr equationManager = model.GetEquationMgr();

            if (equationManager == null)
            {
                return;
            }

            Config.equationManager = equationManager;

            Daemon.Start();

            logger.Debug("\n Closing Open SolidWorks Documents" +
                "\n - Exiting Microservice");
            swApp.CloseAllDocuments(true);
            */
        }
    }
}
