using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardEnemiesBalancer.Utility.Helpers.Static
{
    public static class AreaFamiliesHelpers
    {
        public static AreaFamily GetAreaFamilyByKeyWord(string keyword)
        {
            foreach (AreaFamily family in AreaManager.AreaFamilies)
            {
                foreach (string familyKeyword in family.FamilyKeywords)
                {
                    if (familyKeyword.Equals(keyword, StringComparison.OrdinalIgnoreCase))
                        return family;
                }
            }

            return null;
        }

        public static AreaFamily GetAreaFamilyByName(string name)
        {
            foreach (AreaFamily family in AreaManager.AreaFamilies)
            {
                if (family.FamilyName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return family;
            }

            return null;
        }

        public static bool DoesAreaFamilyMatch(AreaFamily family)
        {
            AreaFamily areaFamily = GetActiveAreaFamily();

            if (areaFamily == null || family == null)
                return false;

            if (areaFamily.FamilyName == family.FamilyName)
                return true;

            return false;
        }

        public static AreaFamily GetActiveAreaFamily()
        {
            foreach (AreaFamily areaFamily in AreaManager.AreaFamilies)
            {
                foreach (string familyKeyWord in areaFamily.FamilyKeywords)
                {
                    if (SceneManagerHelper.ActiveSceneName.Contains(familyKeyWord))
                        return areaFamily;
                }
            }

            return null;
        }
    }
}
