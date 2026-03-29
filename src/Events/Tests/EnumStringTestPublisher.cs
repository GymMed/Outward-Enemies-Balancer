#if DEBUG
using OutwardModsCommunicator.EventBus;
using System.Collections.Generic;

namespace OutwardEnemiesBalancer.Events.Tests
{
    public static class EnumStringTestPublisher
    {
        // ValueModifierType as strings
        public static void PublishAddBalanceRuleByEnemyId_StringModifier(
            string enemyId, string statName, float value, string modifierTypeString)
        {
            var payload = new EventPayload();
            payload.Set("enemyId", enemyId);
            payload.Set("statModifications", new Dictionary<string, float?> { { statName, value } });
            payload.Set("modifierType", modifierTypeString); // String instead of enum

            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddBalanceRuleByEnemyId", payload);
        }

        public static void PublishAddBalanceRuleByEnemyName_StringModifier(
            string enemyName, string statName, float value, string modifierTypeString)
        {
            var payload = new EventPayload();
            payload.Set("enemyName", enemyName);
            payload.Set("statModifications", new Dictionary<string, float?> { { statName, value } });
            payload.Set("modifierType", modifierTypeString);

            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddBalanceRuleByEnemyName", payload);
        }

        // Character.Factions as strings
        public static void PublishAddFactionRuleByEnemyId_StringFaction(string enemyId, string newFactionString)
        {
            var payload = new EventPayload();
            payload.Set("enemyId", enemyId);
            payload.Set("newFaction", newFactionString);

            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddFactionRule", payload);
        }

        public static void PublishAddFactionRuleByEnemyName_StringFaction(string enemyName, string newFactionString)
        {
            var payload = new EventPayload();
            payload.Set("enemyName", enemyName);
            payload.Set("newFaction", newFactionString);

            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddFactionRule", payload);
        }

        public static void PublishAddFactionRuleForBosses_StringFaction(
            bool isBoss, bool isBossPawn, bool isStoryBoss,
            bool isUniqueArenaBoss, bool isUniqueEnemy, string newFactionString)
        {
            var payload = new EventPayload();
            payload.Set("isBoss", isBoss);
            payload.Set("isBossPawn", isBossPawn);
            payload.Set("isStoryBoss", isStoryBoss);
            payload.Set("isUniqueArenaBoss", isUniqueArenaBoss);
            payload.Set("isUniqueEnemy", isUniqueEnemy);
            payload.Set("newFaction", newFactionString);

            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddFactionRule", payload);
        }

        // AreaFamily as string
        public static void PublishAddBalanceRuleForArea_StringAreaFamily(string areaFamilyString, string statName, float value)
        {
            var payload = new EventPayload();
            payload.Set("areaFamily", areaFamilyString);
            payload.Set("statModifications", new Dictionary<string, float?> { { statName, value } });

            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddBalanceRuleForArea", payload);
        }

        // Combined: string for both modifier and faction
        public static void PublishAddBalanceRule_StringEverything(
            string enemyId, string statName, float value, string modifierTypeString)
        {
            var payload = new EventPayload();
            payload.Set("enemyId", enemyId);
            payload.Set("statModifications", new Dictionary<string, float?> { { statName, value } });
            payload.Set("modifierType", modifierTypeString);

            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddBalanceRule", payload);
        }

        public static void PublishAddFactionRule_StringEverything(
            string enemyName, string newFactionString, bool isBoss)
        {
            var payload = new EventPayload();
            payload.Set("enemyName", enemyName);
            payload.Set("newFaction", newFactionString);
            payload.Set("isBoss", isBoss);

            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddFactionRule", payload);
        }
    }
}
#endif
