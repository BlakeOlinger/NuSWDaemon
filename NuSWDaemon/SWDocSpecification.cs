using SolidWorks.Interop.sldworks;
using System;
using System.IO;

namespace sw_part_auto_test
{
    public class SWDocSpecification
    {
        public static DocumentSpecification GetDocumentSpecification(
            ISldWorks swApp, string path)
        {
            try
            {
                try
                {
                    if (path == null ||
                        string.Compare(path, "") == 0 ||
                        swApp == null)
                        throw new ArgumentException();
                }
                catch (ArgumentException)
                {
                    return null;
                }

                try
                {
                    if (!File.Exists(path))
                        throw new ArgumentException();
                }
                catch (ArgumentException )
                {
                    return null;
                }

                DocumentSpecification documentSpecification =
                    (DocumentSpecification)swApp.GetOpenDocSpec(path);

                if (documentSpecification == null)
                    throw new Exception();

                return documentSpecification;
            } catch (Exception)
            {
                return null;
            }
        }
    }
}
