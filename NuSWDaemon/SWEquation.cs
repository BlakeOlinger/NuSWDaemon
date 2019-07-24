using SolidWorks.Interop.sldworks;
using System;

namespace sw_part_auto_test
{
    class SWEquation
    {
        public static void AddEquation(EquationMgr equationMgr,
            string equation)
        {
            Console.WriteLine(" - Equation Manager - Adding Equation - " + equation);

            equationMgr.Add(0, equation);

            if (equationMgr.GetCount() == 1){
                Console.WriteLine(" - Equation Manager - Equation - " + equation +
                    " - Successfully Added");
            } else
            {
                Console.WriteLine(" - WARNING - Equation Manager - Failed to Add Equation - " +
                    equation);
            }
        }

        
        public static void DeleteEquation(EquationMgr equationManager, int index)
        {
            equationManager.Delete(index);

            if (equationManager.GetCount() == 0)
            {
                Console.WriteLine(" - Delete Equation - Success");
            } else
            {
                Console.WriteLine(" - WARNING - Delete Equation - Failed");
            }
        }

        public static void Build(IModelDoc2 model)
        {

            if (model.EditRebuild3()) {
                Console.WriteLine(" - SolidWorks Model - Build - Success");
            } else
            {
                Console.WriteLine(" - WARNING - SolidWorks Model - Build - Failed");
            }
        }
    }
}
