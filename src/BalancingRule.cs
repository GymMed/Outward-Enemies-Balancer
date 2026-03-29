using OutwardEnemiesBalancer.Balancing;
using OutwardEnemiesBalancer.Managers;
using OutwardEnemiesBalancer.Utility.Enums;
using OutwardEnemiesBalancer.Utility.Extensions;
using OutwardEnemiesBalancer.Utility.Helpers.Static;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OutwardEnemiesBalancer
{
    public class BalancingRule
    {
        public string id;
        public string enemyID;
        public string enemyName;
        public AreaFamily areaFamily;
        public AreaManager.AreaEnum? area;
        public Character.Factions? faction;
        public bool isBoss;
        public bool isBossPawn;
        public bool isStoryBoss;
        public bool isUniqueArenaBoss;
        public bool isUniqueEnemy;
        public List<string> exceptNames;
        public List<string> exceptIds;
        public Dictionary<string, float?> statModifications;
        public ValueModifierType modifierType = ValueModifierType.Scale;

        public BalancingRule(string id = null)
        {
            if (string.IsNullOrEmpty(id))
                this.id = UID.Generate().Value;
            else
                this.id = id;

            statModifications = new Dictionary<string, float?>();
        }

        public bool Matches(Character character)
        {
            if (character == null)
                return false;

            if (!string.IsNullOrEmpty(enemyID))
                return character.UID.Value == enemyID;

            if (!string.IsNullOrEmpty(enemyName) &&
                !character.Name.Equals(enemyName, StringComparison.OrdinalIgnoreCase))
                return false;

            if (exceptIds != null && exceptIds.Contains(character.UID.Value))
                return false;

            if (exceptNames != null && exceptNames.Contains(character.Name))
                return false;

            if (faction.HasValue && character.Faction != faction.Value)
                return false;

            if (area.HasValue)
            {
                var currentArea = AreaManager.Instance.CurrentArea;
                var ruleArea = AreaManager.Instance.GetArea(area.Value);

                if (currentArea == null || ruleArea == null || currentArea.ID != ruleArea.ID)
                    return false;
            }

            if (areaFamily != null)
            {
                if (!AreaFamiliesHelpers.DoesAreaFamilyMatch(areaFamily))
                    return false;
            }

            if (isBoss)
            {
                if (!BossRegistryManager.Instance.IsBoss(character))
                    return false;
                return true;
            }
            else
            {
                if (BossRegistryManager.Instance.IsBoss(character))
                    return false;
            }

            if (isUniqueArenaBoss)
            {
                if (!UniqueArenaBossesHelper.Enemies.TryGetEnum(character, out UniqueArenaBosses boss))
                    return false;
            }
            else
            {
                if (BossRegistryManager.Instance.IsBossOfCategory(character, BossCategories.Arena))
                    return false;
            }

            if (isStoryBoss)
            {
                if (!StoryBossesHelper.Enemies.TryGetEnum(character, out StoryBosseses boss))
                    return false;
            }
            else
            {
                if (BossRegistryManager.Instance.IsBossOfCategory(character, BossCategories.Story))
                    return false;
            }

            if (isBossPawn)
            {
                if (!BossPawnsHelper.Enemies.TryGetEnum(character, out BossPawns boss))
                    return false;
            }
            else
            {
                if (BossRegistryManager.Instance.IsBossOfCategory(character, BossCategories.Pawn))
                    return false;
            }

            if (isUniqueEnemy)
            {
                if (!UniqueEnemiesHelper.Enemies.TryGetEnum(character, out UniqueEnemies enemy))
                    return false;
            }
            else
            {
                if (UniqueEnemiesHelper.Enemies.TryGetEnum(character, out UniqueEnemies enemy))
                    return false;
            }

            return true;
        }
    }
}
