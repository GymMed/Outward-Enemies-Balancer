using OutwardEnemiesBalancer.Balancing;
using System;
using System.Collections.Generic;

namespace OutwardEnemiesBalancer.Balancing.Internal
{
    internal static class StatModificationBuilder
    {
        public static List<StatModification> BuildFromDictionary(
            Dictionary<string, float?> modifications,
            ValueModifierType defaultModifierType)
        {
            List<StatModification> result = new List<StatModification>();

            if (modifications == null)
                return result;

            foreach (var kvp in modifications)
            {
                if (kvp.Value.HasValue && 
                    Enum.TryParse<EnemyBalanceStatType>(kvp.Key, ignoreCase: true, out EnemyBalanceStatType statType))
                {
                    result.Add(new StatModification(statType, defaultModifierType, kvp.Value.Value));
                }
            }

            return result;
        }

        public static List<StatModification> BuildFromSingleStat(
            string statTypeName,
            ValueModifierType modifierType,
            float value,
            float? minClamp = null,
            float? maxClamp = null)
        {
            List<StatModification> result = new List<StatModification>();

            if (Enum.TryParse<EnemyBalanceStatType>(statTypeName, ignoreCase: true, out EnemyBalanceStatType statType))
            {
                result.Add(new StatModification(statType, modifierType, value, minClamp, maxClamp));
            }

            return result;
        }

        public static List<StatModification> BuildFromPayload(
            Dictionary<string, float?> modifications,
            ValueModifierType defaultModifierType,
            string statTypeName = null,
            float? value = null,
            ValueModifierType? modifierType = null,
            float? minClamp = null,
            float? maxClamp = null)
        {
            List<StatModification> result = new List<StatModification>();

            if (modifications != null)
            {
                result.AddRange(BuildFromDictionary(modifications, defaultModifierType));
            }

            if (!string.IsNullOrEmpty(statTypeName) && value.HasValue)
            {
                result.AddRange(BuildFromSingleStat(
                    statTypeName,
                    modifierType ?? defaultModifierType,
                    value.Value,
                    minClamp,
                    maxClamp));
            }

            return result;
        }
    }
}
