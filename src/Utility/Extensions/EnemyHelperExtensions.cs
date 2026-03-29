using OutwardEnemiesBalancer.Managers;
using OutwardEnemiesBalancer.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardEnemiesBalancer.Utility.Extensions
{
    public static class EnemyHelperExtensions
    {
        public static Dictionary<TEnum, TResult> ToDictionaryBySelector<TEnum, TResult>(
            this Dictionary<TEnum, EnemyIdentificationGroupData> dict,
            Func<EnemyIdentificationGroupData, TResult> selector)
            where TEnum : Enum
        {
            return dict.ToDictionary(kvp => kvp.Key, kvp => selector(kvp.Value));
        }

        public static List<TResult> GetAll<TResult, TEnum>(
            this Dictionary<TEnum, EnemyIdentificationGroupData> dict,
            Func<EnemyIdentificationData, TResult> selector)
            where TEnum : Enum
        {
            return dict
                .SelectMany(kvp => kvp.Value.Enemies)
                .Select(selector)
                .Distinct()
                .ToList();
        }

        public static List<string> GetAll<TEnum>(
            this Dictionary<TEnum, EnemyIdentificationGroupData> dict)
            where TEnum : Enum
        {
            return dict.GetAll(e => e.ID);
        }

        public static bool TryGetEnum<TEnum>(
            this Dictionary<TEnum, EnemyIdentificationGroupData> dict,
            Character character,
            out TEnum result)
            where TEnum : Enum
        {
            string enumIdentificator = BossRegistryManager.GetEnemyBossIdentificator(character);

            foreach (var kvp in dict)
            {
                if (kvp.Value.Matches(character,
                    (idData, character) => enumIdentificator.Equals(BossRegistryManager.GetIdentificatorFromEnemyIdentification(idData)))
                )
                {
                    result = kvp.Key;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static Dictionary<TEnum, string> GetFirstDisplayNameFromGroup<TEnum>(
            this Dictionary<TEnum, EnemyIdentificationGroupData> dict)
            where TEnum : Enum
        {
            return dict.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Enemies.First().DisplayName
            );
        }

        public static Dictionary<TEnum, string> GetFirstNameLocFromGroup<TEnum>(
            this Dictionary<TEnum, EnemyIdentificationGroupData> dict)
            where TEnum : Enum
        {
            return dict.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Enemies.First().LocKey
            );
        }

        public static Dictionary<TEnum, string> GetFirstWikiLocationsFromGroup<TEnum>(
            this Dictionary<TEnum, EnemyIdentificationGroupData> dict)
            where TEnum : Enum
        {
            return dict.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Enemies.First().WikiLocation
            );
        }

        public static Dictionary<TEnum, string> GetFirstGameLocationsFromGroup<TEnum>(
            this Dictionary<TEnum, EnemyIdentificationGroupData> dict)
            where TEnum : Enum
        {
            return dict.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Enemies.First().GameLocation
            );
        }

        public static List<string> GetAllDisplayNames<TEnum>(
            this Dictionary<TEnum, EnemyIdentificationGroupData> dict)
            where TEnum : Enum
        {
            return dict.GetAll(e => e.DisplayName);
        }

        public static List<string> GetAllIds<TEnum>(
            this Dictionary<TEnum, EnemyIdentificationGroupData> dict)
            where TEnum : Enum
        {
            return dict.GetAll(e => e.ID);
        }

        public static List<string> GetAllGameLocations<TEnum>(
            this Dictionary<TEnum, EnemyIdentificationGroupData> dict)
            where TEnum : Enum
        {
            return dict.GetAll(e => e.GameLocation);
        }

        public static List<string> GetAllWikiLocations<TEnum>(
            this Dictionary<TEnum, EnemyIdentificationGroupData> dict)
            where TEnum : Enum
        {
            return dict.GetAll(e => e.WikiLocation);
        }
    }
}
