#if DEBUG
using OutwardModsCommunicator.EventBus;
using System.Collections.Generic;

namespace OutwardEnemiesBalancer.Events.Tests
{
    public static class FactionTestPublisher
    {
        public static void PublishAddFactionRuleByEnemyId(string enemyId, Character.Factions newFaction)
        {
            var payload = new EventPayload();
            payload.Set("enemyId", enemyId);
            payload.Set("newFaction", newFaction);

            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddFactionRule", payload);
        }

        public static void PublishAddFactionRuleByEnemyName(string enemyName, Character.Factions newFaction)
        {
            var payload = new EventPayload();
            payload.Set("enemyName", enemyName);
            payload.Set("newFaction", newFaction);

            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddFactionRule", payload);
        }

        public static void PublishAddFactionRuleForBosses(
            bool isBoss, bool isBossPawn, bool isStoryBoss,
            bool isUniqueArenaBoss, bool isUniqueEnemy, Character.Factions newFaction)
        {
            var payload = new EventPayload();
            payload.Set("isBoss", isBoss);
            payload.Set("isBossPawn", isBossPawn);
            payload.Set("isStoryBoss", isStoryBoss);
            payload.Set("isUniqueArenaBoss", isUniqueArenaBoss);
            payload.Set("isUniqueEnemy", isUniqueEnemy);
            payload.Set("newFaction", newFaction);

            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddFactionRule", payload);
        }

        public static void PublishAddFactionRuleWithExceptions(string enemyName, 
            List<string> exceptNames, Character.Factions newFaction)
        {
            var payload = new EventPayload();
            payload.Set("enemyName", enemyName);
            
            if (exceptNames != null)
                payload.Set("exceptNames", exceptNames);
            
            payload.Set("newFaction", newFaction);

            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddFactionRule", payload);
        }

        public static void PublishAddFactionRuleForArea(AreaFamily areaFamily, 
            AreaManager.AreaEnum? area, Character.Factions newFaction)
        {
            var payload = new EventPayload();
            
            if (areaFamily != null)
                payload.Set("areaFamily", areaFamily);
            
            if (area.HasValue)
                payload.Set("area", area.Value);
            
            payload.Set("newFaction", newFaction);

            EventBus.Publish(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, "AddFactionRule", payload);
        }
    }
}
#endif
