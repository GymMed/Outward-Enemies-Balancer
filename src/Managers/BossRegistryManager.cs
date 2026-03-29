using OutwardEnemiesBalancer.Utility.Data;
using OutwardEnemiesBalancer.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardEnemiesBalancer.Managers
{
    public class BossRegistryManager
    {
        private static BossRegistryManager _instance;

        private BossRegistryManager()
        {
            RegisterEnum(StoryBossesHelper.Enemies, BossCategories.Story);
            RegisterEnum(UniqueArenaBossesHelper.Enemies, BossCategories.Arena);
            RegisterEnum(BossPawnsHelper.Enemies, BossCategories.Pawn);
        }

        public static BossRegistryManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BossRegistryManager();

                return _instance;
            }
        }

        private readonly Dictionary<string, BossID> bossLookup =
            new Dictionary<string, BossID>(StringComparer.Ordinal);

        private void RegisterEnum<T>(
            Dictionary<T, EnemyIdentificationGroupData> mapping,
            BossCategories category)
            where T : Enum
        {
            foreach (var kvp in mapping)
            {
                var enumKey = kvp.Key;
                var groupData = kvp.Value;
                var bossId = new BossID(category, groupData, enumKey);

                foreach (var enemy in groupData.Enemies)
                {
                    if (!string.IsNullOrWhiteSpace(enemy.ID))
                        bossLookup[GetIdentificatorFromEnemyIdentification(enemy)] = bossId;
                }
            }
        }

        public bool TryGetBoss(string key, out BossID boss) =>
            bossLookup.TryGetValue(key, out boss);

        public bool IsBoss(Character character) =>
            bossLookup.ContainsKey(GetEnemyBossIdentificator(character));

        public bool IsBossOfCategory(string key, BossCategories category) =>
            TryGetBoss(key, out var boss) && boss.Category == category;

        public bool IsBossOfCategory(Character character, BossCategories category)
        {
            return TryGetBoss(GetEnemyBossIdentificator(character), out var boss) && boss.Category == category;
        }

        public IEnumerable<BossID> GetBossesOfCategory(BossCategories category) =>
            bossLookup.Values.Where(b => b.Category == category);

        public static string GetEnemyBossIdentificator(Character character)
        {
            string location = AreaManager.Instance.CurrentArea?.GetName();

            if (string.IsNullOrEmpty(location))
                return character.UID.Value;

            return $"{character.UID.Value}_{FixAreaNameForCode(location)}";
        }

        public static string GetIdentificatorFromEnemyIdentification(EnemyIdentificationData enemy)
        {
            return $"{enemy.ID}_{FixAreaNameForCode(enemy.GameLocation)}";
        }

        public static string FixAreaNameForCode(string name)
        {
            return name.Trim().Replace(' ', '_');
        }
    }
}
