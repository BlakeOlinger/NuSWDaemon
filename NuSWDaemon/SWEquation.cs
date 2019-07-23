using SolidWorks.Interop.sldworks;
using System;

namespace sw_part_auto_test
{
    class SWEquation
    {
        public static void AddEquation(EquationMgr equationMgr,
            string equation)
        {
            if ((equationMgr.Add(equationMgr.GetCount(), equation)) == 1){

            } else
            {
            }
        }

        
        public static void DeleteEquation(EquationMgr equationManager, int index)
        {
            equationManager.Delete(index);

            if (equationManager.GetCount() != 0)
            {

            }
        }

        public static void Build(IModelDoc2 model)
        {

            if (model.EditRebuild3()) {
            }
        }
    }
}
