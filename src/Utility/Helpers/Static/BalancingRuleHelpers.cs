using OutwardEnemiesBalancer.Balancing;
using OutwardEnemiesBalancer.Managers;
using OutwardEnemiesBalancer.Utility.Enums;
using OutwardEnemiesBalancer.Utility.Helpers.Static;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;

namespace OutwardEnemiesBalancer.Utility.Helpers.Static
{
    public static class BalancingRuleHelpers
    {
        public static bool TryToFillRuleWithId(BalancingRule rule, EventPayload payload)
        {
            var param = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.BalanceRuleId);
            string ruleId = payload.Get<string>(param.key, null);

            if (!string.IsNullOrEmpty(ruleId))
            {
                rule.id = ruleId;
                return true;
            }

            return false;
        }

        public static bool TryToFillRuleWithEnemyId(BalancingRule rule, EventPayload payload, bool enforceNonEmpty = false)
        {
            var param = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.EnemyId);
            string enemyId = payload.Get<string>(param.key, null);

            if (string.IsNullOrEmpty(enemyId))
            {
                if (enforceNonEmpty)
                {
                    OutwardEnemiesBalancer.LogSL($"BalancingRuleHelpers@TryToFillRuleWithEnemyId didn't receive {param.key}!");
                }
                return false;
            }

            rule.enemyID = enemyId;
            return true;
        }

        public static bool TryToFillRuleWithEnemyName(BalancingRule rule, EventPayload payload, bool enforceNonEmpty = false)
        {
            var param = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.EnemyName);
            string enemyName = payload.Get<string>(param.key, null);

            if (string.IsNullOrEmpty(enemyName))
            {
                if (enforceNonEmpty)
                {
                    OutwardEnemiesBalancer.LogSL($"BalancingRuleHelpers@TryToFillRuleWithEnemyName didn't receive {param.key}!");
                }
                return false;
            }

            rule.enemyName = enemyName;
            return true;
        }

        public static bool FillRuleWithEnvironmentConditions(BalancingRule rule, EventPayload payload)
        {
            var areaFamilyParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.AreaFamily);
            var factionParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.Faction);
            var areaParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.AreaEnum);

            rule.areaFamily = payload.Get<AreaFamily>(areaFamilyParam.key, null);
            rule.faction = payload.GetEnum<Character.Factions>(factionParam.key, null);
            rule.area = payload.GetEnum<AreaManager.AreaEnum>(areaParam.key, null);

            return rule.areaFamily != null || rule.faction.HasValue || rule.area.HasValue;
        }

        public static bool FillRuleForStrongEnemyTypes(BalancingRule rule, EventPayload payload)
        {
            var isBossParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForBosses);
            var isBossPawnParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForBossesPawns);
            var isStoryBossParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForStoryBosses);
            var isUniqueArenaBossParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForUniqueArenaBosses);
            var isUniqueEnemyParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForUniqueEnemies);

            rule.isBoss = payload.Get<bool>(isBossParam.key, false);
            rule.isBossPawn = payload.Get<bool>(isBossPawnParam.key, false);
            rule.isStoryBoss = payload.Get<bool>(isStoryBossParam.key, false);
            rule.isUniqueArenaBoss = payload.Get<bool>(isUniqueArenaBossParam.key, false);
            rule.isUniqueEnemy = payload.Get<bool>(isUniqueEnemyParam.key, false);

            return rule.isBoss || rule.isBossPawn || rule.isStoryBoss || rule.isUniqueArenaBoss || rule.isUniqueEnemy;
        }

        public static bool FillRuleWithExceptions(BalancingRule rule, EventPayload payload)
        {
            var exceptIdsParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ExceptIds);
            var exceptNamesParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ExceptNames);

            rule.exceptIds = payload.Get<List<string>>(exceptIdsParam.key, null);
            rule.exceptNames = payload.Get<List<string>>(exceptNamesParam.key, null);

            return rule.exceptIds != null || rule.exceptNames != null;
        }

        public static bool FillRuleWithIdExceptions(BalancingRule rule, EventPayload payload)
        {
            var exceptIdsParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ExceptIds);

            rule.exceptIds = payload.Get<List<string>>(exceptIdsParam.key, null);

            return rule.exceptIds != null;
        }

        public static void FillRuleWithStatModifications(BalancingRule rule, EventPayload payload)
        {
            var modifierTypeParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ModifierType);
            var modifierType = payload.GetEnum<ValueModifierType>(modifierTypeParam.key, null);
            if (modifierType.HasValue)
                rule.modifierType = modifierType.Value;

            var statModsParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.StatModifications);
            var statTypeParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.StatType);
            var valueParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.Value);

            Dictionary<string, float?> statMods = payload.Get<Dictionary<string, float?>>(statModsParam.key, null);
            if (statMods != null)
            {
                foreach (var kvp in statMods)
                {
                    rule.statModifications[kvp.Key] = kvp.Value;
                }
            }

            string statTypeName = payload.Get<string>(statTypeParam.key, null);
            float? value = payload.Get<float?>(valueParam.key, null);

            if (!string.IsNullOrEmpty(statTypeName) && value.HasValue)
            {
                rule.statModifications[statTypeName] = value.Value;
            }
        }

        public static void FillRuleWithStatModificationsFromVitalStats(BalancingRule rule, EventPayload payload)
        {
            var maxHealthParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.MaxHealth);
            var maxStaminaParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.MaxStamina);
            var maxManaParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.MaxMana);
            var healthRegenParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.HealthRegen);
            var staminaRegenParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.StaminaRegen);
            var manaRegenParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ManaRegen);

            AddStatMod(rule, payload, EnemyBalanceStatType.MaxHealth.ToString(), maxHealthParam.key);
            AddStatMod(rule, payload, EnemyBalanceStatType.MaxStamina.ToString(), maxStaminaParam.key);
            AddStatMod(rule, payload, EnemyBalanceStatType.MaxMana.ToString(), maxManaParam.key);
            AddStatMod(rule, payload, EnemyBalanceStatType.HealthRegen.ToString(), healthRegenParam.key);
            AddStatMod(rule, payload, EnemyBalanceStatType.StaminaRegen.ToString(), staminaRegenParam.key);
            AddStatMod(rule, payload, EnemyBalanceStatType.ManaRegen.ToString(), manaRegenParam.key);
        }

        private static void AddStatMod(BalancingRule rule, EventPayload payload, string statName, string paramKey)
        {
            float? value = payload.Get<float?>(paramKey, null);
            if (value.HasValue)
            {
                rule.statModifications[statName] = value.Value;
            }
        }

        public static void FillRuleWithStatModificationsFromEnvironmentalStats(BalancingRule rule, EventPayload payload)
        {
            var coldProtectionParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ColdProtection);
            var heatProtectionParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.HeatProtection);
            var corruptionResistanceParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.CorruptionResistance);
            var waterproofParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.Waterproof);

            AddStatMod(rule, payload, EnemyBalanceStatType.ColdProtection.ToString(), coldProtectionParam.key);
            AddStatMod(rule, payload, EnemyBalanceStatType.HeatProtection.ToString(), heatProtectionParam.key);
            AddStatMod(rule, payload, EnemyBalanceStatType.CorruptionProtection.ToString(), corruptionResistanceParam.key);
            AddStatMod(rule, payload, EnemyBalanceStatType.Waterproof.ToString(), waterproofParam.key);
        }

        public static void FillRuleWithStatModificationsFromCombatStats(BalancingRule rule, EventPayload payload)
        {
            var impactParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.Impact);
            var impactResistanceParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ImpactResistance);
            var movementSpeedParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.MovementSpeed);
            var attackSpeedParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.AttackSpeed);

            AddStatMod(rule, payload, EnemyBalanceStatType.Impact.ToString(), impactParam.key);
            AddStatMod(rule, payload, EnemyBalanceStatType.ImpactResistance.ToString(), impactResistanceParam.key);
            AddStatMod(rule, payload, EnemyBalanceStatType.MovementSpeed.ToString(), movementSpeedParam.key);
            AddStatMod(rule, payload, EnemyBalanceStatType.AttackSpeed.ToString(), attackSpeedParam.key);
        }

        public static bool ValidateAndAppendRule(BalancingRule rule)
        {
            bool hasTargeting = !string.IsNullOrEmpty(rule.enemyID) ||
                                !string.IsNullOrEmpty(rule.enemyName) ||
                                rule.areaFamily != null ||
                                rule.faction.HasValue ||
                                rule.area.HasValue ||
                                rule.isBoss ||
                                rule.isBossPawn ||
                                rule.isStoryBoss ||
                                rule.isUniqueArenaBoss ||
                                rule.isUniqueEnemy;

            if (!hasTargeting)
            {
                OutwardEnemiesBalancer.LogSL($"BalancingRuleHelpers@ValidateAndAppendRule: Rule has no targeting criteria!");
                return false;
            }

            if (rule.statModifications.Count == 0)
            {
                OutwardEnemiesBalancer.LogSL($"BalancingRuleHelpers@ValidateAndAppendRule: Rule has no stat modifications!");
                return false;
            }

            BalancingRuleRegistryManager.Instance.AppendBalancingRule(rule);
            return true;
        }
    }
}
