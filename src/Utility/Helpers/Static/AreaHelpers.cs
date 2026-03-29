using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardEnemiesBalancer.Utility.Helpers.Static
{
    public static class AreaHelpers
    {
        public static bool IsAreaInAreaFamily(AreaManager.AreaEnum area)
        {
            foreach (AreaFamily areaFamily in AreaManager.AreaFamilies)
            {
                foreach (string familyKeyWord in areaFamily.FamilyKeywords)
                {
                    if (AreaManager.Instance.GetArea(area).SceneName.Contains(familyKeyWord))
                        return true;
                }
            }

            return false;
        }

        public static AreaManager.AreaEnum? GetAreaEnumFromAreaDefaultName(string areaName)
        {
            foreach (Area area in AreaManager.Instance.Areas)
            {
                if (area.DefaultName == areaName)
                    return (AreaManager.AreaEnum)area.ID;
            }

            return null;
        }

        public static AreaManager.AreaEnum? GetAreaEnumFromAreaName(string areaName)
        {
            foreach (Area area in AreaManager.Instance.Areas)
            {
                if (area.GetName() == areaName)
                    return (AreaManager.AreaEnum)area.ID;
            }

            return null;
        }

        public static AreaManager.AreaEnum GetAreaEnumFromArea(Area area)
        {
            return (AreaManager.AreaEnum)area.ID;
        }

        public static List<AreaManager.AreaEnum> GetAreasFromEnumDictionary<T>(Dictionary<T, string> locations) where T : Enum
        {
            List<AreaManager.AreaEnum> areas = new List<AreaManager.AreaEnum>();

            foreach (KeyValuePair<T, string> kvp in locations)
            {
                foreach (Area currentArea in AreaManager.Instance.Areas)
                {
                    if (currentArea.GetName() == kvp.Value)
                    {
                        areas.Add(AreaHelpers.GetAreaEnumFromArea(currentArea));
                    }
                }
            }

            return areas;
        }

        public static HashSet<AreaManager.AreaEnum> GetUniqueAreasFromEnumDictionary<T>(Dictionary<T, string> locations) where T : Enum
        {
            HashSet<AreaManager.AreaEnum> areas = new HashSet<AreaManager.AreaEnum>();

            foreach (KeyValuePair<T, string> kvp in locations)
            {
                foreach (Area currentArea in AreaManager.Instance.Areas)
                {
                    if (currentArea.GetName() == kvp.Value)
                    {
                        areas.Add(AreaHelpers.GetAreaEnumFromArea(currentArea));
                    }
                }
            }

            return areas;
        }
    }
}
