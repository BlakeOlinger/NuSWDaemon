
using SolidWorks.Interop.sldworks;
using System;

namespace sw_part_auto_test
{
    public class CreateSWInstance
    {
        public static ISldWorks Create(Type swType)
        {
            try
            {
                var app = (ISldWorks) Activator.CreateInstance(swType);

                return app;
            } catch(Exception exception)
            {
                return null;
            }
        }
    }
}
