#if DEBUG
using OutwardEnemiesBalancer.Balancing;
using OutwardModsCommunicator.EventBus;
using System.Collections.Generic;

namespace OutwardEnemiesBalancer.Events.Tests
{
    public static class BalancingTestPublisher
    {
        public static void PublishAddBalanceRule(string enemyId, string enemyName, 
            Dictionary<string, float?> statModifications, ValueModifierType modifierType)
        {
            var payload = new EventPayload();
            
            if (!string.IsNullOrEmpty(enemyId))
                payload.Set("enemyId", enemyId);
            
            if (!string.IsNullOrEmpty(enemyName))
                payload.Set("enemyName", enemyName);
            
            if (statModifications != null)
                payload.Set("statModifications", statModifications);
            
            payload.Set("modifierType", modifierType);
            
            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddBalanceRule", payload);
        }

        public static void PublishAddBalanceRuleByEnemyId(string enemyId, 
            Dictionary<string, float?> statModifications, ValueModifierType modifierType)
        {
            var payload = new EventPayload();
            payload.Set("enemyId", enemyId);
            
            if (statModifications != null)
                payload.Set("statModifications", statModifications);
            
            payload.Set("modifierType", modifierType);
            
            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddBalanceRuleByEnemyId", payload);
        }

        public static void PublishAddBalanceRuleByEnemyId(string enemyId, string statName, float value, ValueModifierType modifierType)
        {
            PublishAddBalanceRuleByEnemyId(enemyId, new Dictionary<string, float?> { { statName, value } }, modifierType);
        }

        public static void PublishAddBalanceRuleByEnemyName(string enemyName, 
            Dictionary<string, float?> statModifications, ValueModifierType modifierType)
        {
            var payload = new EventPayload();
            payload.Set("enemyName", enemyName);
            
            if (statModifications != null)
                payload.Set("statModifications", statModifications);
            
            payload.Set("modifierType", modifierType);
            
            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddBalanceRuleByEnemyName", payload);
        }

        public static void PublishAddBalanceRuleByEnemyName(string enemyName, string statName, float value, ValueModifierType modifierType)
        {
            PublishAddBalanceRuleByEnemyName(enemyName, new Dictionary<string, float?> { { statName, value } }, modifierType);
        }

        public static void PublishAddBalanceRuleForBosses(bool isBoss, bool isBossPawn, bool isStoryBoss, 
            bool isUniqueArenaBoss, bool isUniqueEnemy, Dictionary<string, float?> statModifications, ValueModifierType modifierType)
        {
            var payload = new EventPayload();
            payload.Set("isBoss", isBoss);
            payload.Set("isBossPawn", isBossPawn);
            payload.Set("isStoryBoss", isStoryBoss);
            payload.Set("isUniqueArenaBoss", isUniqueArenaBoss);
            payload.Set("isUniqueEnemy", isUniqueEnemy);
            
            if (statModifications != null)
                payload.Set("statModifications", statModifications);
            
            payload.Set("modifierType", modifierType);
            
            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddBalanceRuleForBosses", payload);
        }

        public static void PublishAddBalanceRuleForUniques(bool isUniqueEnemy, bool isUniqueArenaBoss,
            Dictionary<string, float?> statModifications, ValueModifierType modifierType)
        {
            var payload = new EventPayload();
            payload.Set("isUniqueEnemy", isUniqueEnemy);
            payload.Set("isUniqueArenaBoss", isUniqueArenaBoss);
            
            if (statModifications != null)
                payload.Set("statModifications", statModifications);
            
            payload.Set("modifierType", modifierType);
            
            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddBalanceRuleForUniques", payload);
        }

        public static void PublishModifyVitalStats(float? maxHealth, float? maxStamina, float? maxMana,
            float? healthRegen, float? staminaRegen, float? manaRegen, ValueModifierType modifierType)
        {
            var payload = new EventPayload();
            
            if (maxHealth.HasValue) payload.Set("maxHealth", maxHealth.Value);
            if (maxStamina.HasValue) payload.Set("maxStamina", maxStamina.Value);
            if (maxMana.HasValue) payload.Set("maxMana", maxMana.Value);
            if (healthRegen.HasValue) payload.Set("healthRegen", healthRegen.Value);
            if (staminaRegen.HasValue) payload.Set("staminaRegen", staminaRegen.Value);
            if (manaRegen.HasValue) payload.Set("manaRegen", manaRegen.Value);
            
            payload.Set("modifierType", modifierType);
            
            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "ModifyVitalStats", payload);
        }

        public static void PublishModifyEnvironmentalStats(float? coldProtection, float? heatProtection,
            float? corruptionResistance, float? waterproof, ValueModifierType modifierType)
        {
            var payload = new EventPayload();
            
            if (coldProtection.HasValue) payload.Set("coldProtection", coldProtection.Value);
            if (heatProtection.HasValue) payload.Set("heatProtection", heatProtection.Value);
            if (corruptionResistance.HasValue) payload.Set("corruptionResistance", corruptionResistance.Value);
            if (waterproof.HasValue) payload.Set("waterproof", waterproof.Value);
            
            payload.Set("modifierType", modifierType);
            
            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "ModifyEnvironmentalStats", payload);
        }

        public static void PublishModifyCombatStats(float? impact, float? impactResistance,
            float? movementSpeed, float? attackSpeed, ValueModifierType modifierType)
        {
            var payload = new EventPayload();
            
            if (impact.HasValue) payload.Set("impact", impact.Value);
            if (impactResistance.HasValue) payload.Set("impactResistance", impactResistance.Value);
            if (movementSpeed.HasValue) payload.Set("movementSpeed", movementSpeed.Value);
            if (attackSpeed.HasValue) payload.Set("attackSpeed", attackSpeed.Value);
            
            payload.Set("modifierType", modifierType);
            
            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "ModifyCombatStats", payload);
        }

        public static void PublishAddBalanceRuleWithExceptions(string enemyName, List<string> exceptNames,
            Dictionary<string, float?> statModifications, ValueModifierType modifierType)
        {
            var payload = new EventPayload();
            payload.Set("enemyName", enemyName);
            
            if (exceptNames != null)
                payload.Set("exceptNames", exceptNames);
            
            if (statModifications != null)
                payload.Set("statModifications", statModifications);
            
            payload.Set("modifierType", modifierType);
            
            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddBalanceRule", payload);
        }
    }
}
#endif
