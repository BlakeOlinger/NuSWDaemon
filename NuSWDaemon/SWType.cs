using System;

namespace sw_part_auto_test
{
    public class SWType
    {
       public static Type GetFromProgID(string progID)
        {

            try
            {

                var swType = Type.GetTypeFromProgID(progID);

                if (swType == null)
                    throw new Exception();

                return swType;
            } catch(Exception)
            {
                return null;
            }
            
        }
    }
}
