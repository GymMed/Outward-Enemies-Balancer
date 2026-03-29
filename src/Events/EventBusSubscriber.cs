using OutwardEnemiesBalancer.Balancing;
using OutwardEnemiesBalancer.Managers;
using OutwardEnemiesBalancer.Utility.Enums;
using OutwardEnemiesBalancer.Utility.Helpers.Static;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.IO;

namespace OutwardEnemiesBalancer.Events
{
    public static class EventBusSubscriber
    {
        public const string Event_AddBalanceRule = "AddBalanceRule";
        public const string Event_AddBalanceRuleByEnemyName = "AddBalanceRuleByEnemyName";
        public const string Event_AddBalanceRuleByEnemyId = "AddBalanceRuleByEnemyId";
        public const string Event_AddBalanceRuleForBosses = "AddBalanceRuleForBosses";
        public const string Event_AddBalanceRuleForUniques = "AddBalanceRuleForUniques";
        public const string Event_AddBalanceRuleForArea = "AddBalanceRuleForArea";
        public const string Event_ModifyVitalStats = "ModifyVitalStats";
        public const string Event_ModifyEnvironmentalStats = "ModifyEnvironmentalStats";
        public const string Event_ModifyCombatStats = "ModifyCombatStats";
        public const string Event_LoadBalanceRulesFromXml = "LoadBalanceRulesFromXml";
        public const string Event_SaveBalanceRulesToXml = "SaveBalanceRulesToXml";
        public const string Event_AddFactionRule = "AddFactionRule";

        public static void AddSubscribers()
        {
            EventBus.Subscribe(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, Event_AddBalanceRule, AddBalanceRule);
            EventBus.Subscribe(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, Event_AddBalanceRuleByEnemyName, AddBalanceRuleByEnemyName);
            EventBus.Subscribe(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, Event_AddBalanceRuleByEnemyId, AddBalanceRuleByEnemyId);
            EventBus.Subscribe(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, Event_AddBalanceRuleForBosses, AddBalanceRuleForBosses);
            EventBus.Subscribe(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, Event_AddBalanceRuleForUniques, AddBalanceRuleForUniques);
            EventBus.Subscribe(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, Event_AddBalanceRuleForArea, AddBalanceRuleForArea);
            EventBus.Subscribe(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, Event_ModifyVitalStats, ModifyVitalStats);
            EventBus.Subscribe(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, Event_ModifyEnvironmentalStats, ModifyEnvironmentalStats);
            EventBus.Subscribe(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, Event_ModifyCombatStats, ModifyCombatStats);
            EventBus.Subscribe(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, Event_LoadBalanceRulesFromXml, LoadBalanceRulesFromXml);
            EventBus.Subscribe(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, Event_SaveBalanceRulesToXml, SaveBalanceRulesToXml);
            EventBus.Subscribe(OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, Event_AddFactionRule, AddFactionRule);
        }

        public static BalancingRule CreateBalanceRuleFromPayload(EventPayload payload)
        {
            if (payload == null)
                return null;

            BalancingRule rule = new BalancingRule();
            BalancingRuleHelpers.TryToFillRuleWithId(rule, payload);
            BalancingRuleHelpers.FillRuleWithEnvironmentConditions(rule, payload);
            BalancingRuleHelpers.FillRuleForStrongEnemyTypes(rule, payload);
            BalancingRuleHelpers.TryToFillRuleWithEnemyName(rule, payload);
            BalancingRuleHelpers.TryToFillRuleWithEnemyId(rule, payload);
            BalancingRuleHelpers.FillRuleWithExceptions(rule, payload);
            BalancingRuleHelpers.FillRuleWithStatModifications(rule, payload);

            return rule;
        }

        public static void AddBalanceRule(EventPayload payload)
        {
            if (payload == null) return;

            BalancingRule rule = CreateBalanceRuleFromPayload(payload);
            if (BalancingRuleHelpers.ValidateAndAppendRule(rule))
                CharacterBalancerManager.Instance.ApplyBalancingRule(rule);
        }

        public static void AddBalanceRuleByEnemyName(EventPayload payload)
        {
            if (payload == null) return;

            BalancingRule rule = new BalancingRule();
            BalancingRuleHelpers.TryToFillRuleWithId(rule, payload);

            if (!BalancingRuleHelpers.TryToFillRuleWithEnemyName(rule, payload, true))
                return;

            BalancingRuleHelpers.FillRuleWithStatModifications(rule, payload);
            BalancingRuleHelpers.FillRuleWithIdExceptions(rule, payload);

            BalancingRuleRegistryManager.Instance.AppendBalancingRule(rule);
            CharacterBalancerManager.Instance.ApplyBalancingRule(rule);
        }

        public static void AddBalanceRuleByEnemyId(EventPayload payload)
        {
            if (payload == null) return;

            BalancingRule rule = new BalancingRule();
            BalancingRuleHelpers.TryToFillRuleWithId(rule, payload);

            if (!BalancingRuleHelpers.TryToFillRuleWithEnemyId(rule, payload, true))
                return;

            BalancingRuleHelpers.FillRuleWithStatModifications(rule, payload);

            BalancingRuleRegistryManager.Instance.AppendBalancingRule(rule);
            CharacterBalancerManager.Instance.ApplyBalancingRule(rule);
        }

        public static void AddBalanceRuleForBosses(EventPayload payload)
        {
            if (payload == null) return;

            BalancingRule rule = new BalancingRule();
            BalancingRuleHelpers.TryToFillRuleWithId(rule, payload);
            BalancingRuleHelpers.FillRuleForStrongEnemyTypes(rule, payload);
            BalancingRuleHelpers.FillRuleWithStatModifications(rule, payload);
            BalancingRuleHelpers.FillRuleWithExceptions(rule, payload);

            if (BalancingRuleHelpers.ValidateAndAppendRule(rule))
                CharacterBalancerManager.Instance.ApplyBalancingRule(rule);
        }

        public static void AddBalanceRuleForUniques(EventPayload payload)
        {
            if (payload == null) return;

            BalancingRule rule = new BalancingRule();
            BalancingRuleHelpers.TryToFillRuleWithId(rule, payload);
            BalancingRuleHelpers.FillRuleForStrongEnemyTypes(rule, payload);
            BalancingRuleHelpers.FillRuleWithStatModifications(rule, payload);
            BalancingRuleHelpers.FillRuleWithExceptions(rule, payload);

            if (BalancingRuleHelpers.ValidateAndAppendRule(rule))
                CharacterBalancerManager.Instance.ApplyBalancingRule(rule);
        }

        public static void AddBalanceRuleForArea(EventPayload payload)
        {
            if (payload == null) return;

            BalancingRule rule = new BalancingRule();
            BalancingRuleHelpers.TryToFillRuleWithId(rule, payload);
            BalancingRuleHelpers.FillRuleWithEnvironmentConditions(rule, payload);
            BalancingRuleHelpers.FillRuleWithStatModifications(rule, payload);
            BalancingRuleHelpers.FillRuleWithExceptions(rule, payload);

            if (BalancingRuleHelpers.ValidateAndAppendRule(rule))
                CharacterBalancerManager.Instance.ApplyBalancingRule(rule);
        }

        public static void ModifyVitalStats(EventPayload payload)
        {
            if (payload == null) return;

            BalancingRule rule = new BalancingRule();
            BalancingRuleHelpers.TryToFillRuleWithId(rule, payload);
            BalancingRuleHelpers.FillRuleWithStatModificationsFromVitalStats(rule, payload);

            BalancingRuleRegistryManager.Instance.AppendBalancingRule(rule);
            CharacterBalancerManager.Instance.ApplyBalancingRule(rule);
        }

        public static void ModifyEnvironmentalStats(EventPayload payload)
        {
            if (payload == null) return;

            BalancingRule rule = new BalancingRule();
            BalancingRuleHelpers.TryToFillRuleWithId(rule, payload);
            BalancingRuleHelpers.FillRuleWithStatModificationsFromEnvironmentalStats(rule, payload);

            BalancingRuleRegistryManager.Instance.AppendBalancingRule(rule);
            CharacterBalancerManager.Instance.ApplyBalancingRule(rule);
        }

        public static void ModifyCombatStats(EventPayload payload)
        {
            if (payload == null) return;

            BalancingRule rule = new BalancingRule();
            BalancingRuleHelpers.TryToFillRuleWithId(rule, payload);
            BalancingRuleHelpers.FillRuleWithStatModificationsFromCombatStats(rule, payload);

            BalancingRuleRegistryManager.Instance.AppendBalancingRule(rule);
            CharacterBalancerManager.Instance.ApplyBalancingRule(rule);
        }

        public static void LoadBalanceRulesFromXml(EventPayload payload)
        {
            if (payload == null) return;

            var xmlFilePathParameter = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.LoadBalanceRulesXmlPath);
            string xmlFilePath = payload.Get<string>(xmlFilePathParameter.key, null);

            if (string.IsNullOrEmpty(xmlFilePath))
            {
                OutwardEnemiesBalancer.LogSL($"EventBusSubscriber@LoadBalanceRulesFromXml didn't receive {xmlFilePathParameter.key} variable!");
                return;
            }

            BalancingRulesSerializer.Instance.LoadBalanceRules(xmlFilePath);
        }

        public static void SaveBalanceRulesToXml(EventPayload payload)
        {
            if (payload == null) return;

            var xmlFilePathParameter = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.StoreBalanceRulesXmlPath);
            string xmlFilePath = payload.Get<string>(xmlFilePathParameter.key, null);

            if (string.IsNullOrEmpty(xmlFilePath))
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                xmlFilePath = Path.Combine(PathsManager.ConfigPath, $"BalanceRules-{timestamp}.xml");
                OutwardEnemiesBalancer.LogSL($"EventBusSubscriber@SaveBalanceRulesToXml didn't receive {xmlFilePathParameter.key} variable! Using: \"{xmlFilePath}\".");
            }

            BalancingRulesSerializer.Instance.SaveBalanceRulesToXml(xmlFilePath, BalancingRuleRegistryManager.Instance.balancingRules);
        }

        public static void AddFactionRule(EventPayload payload)
        {
            if (payload == null) return;

            var newFactionParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.NewFaction);
            Character.Factions? newFactionNullable = payload.GetEnum<Character.Factions>(newFactionParam.key, null);
            if (!newFactionNullable.HasValue)
            {
                OutwardEnemiesBalancer.LogSL($"EventBusSubscriber@AddFactionRule didn't receive valid {newFactionParam.key}!");
                return;
            }
            Character.Factions newFaction = newFactionNullable.Value;

            FactionRule rule = new FactionRule();

            var ruleIdParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.BalanceRuleId);
            string ruleId = payload.Get<string>(ruleIdParam.key, null);
            if (!string.IsNullOrEmpty(ruleId))
                rule.id = ruleId;

            var enemyIdParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.EnemyId);
            rule.enemyID = payload.Get<string>(enemyIdParam.key, null);

            var enemyNameParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.EnemyName);
            rule.enemyName = payload.Get<string>(enemyNameParam.key, null) ?? "";

            var areaFamilyParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.AreaFamily);
            rule.areaFamily = payload.Get<AreaFamily>(areaFamilyParam.key, null);

            var factionParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.Faction);
            rule.targetFaction = payload.GetEnum<Character.Factions>(factionParam.key, null);

            var areaParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.AreaEnum);
            rule.area = payload.GetEnum<AreaManager.AreaEnum>(areaParam.key, null);

            var isBossParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForBosses);
            rule.isBoss = payload.Get<bool>(isBossParam.key, false);

            var isBossPawnParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForBossesPawns);
            rule.isBossPawn = payload.Get<bool>(isBossPawnParam.key, false);

            var isStoryBossParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForStoryBosses);
            rule.isStoryBoss = payload.Get<bool>(isStoryBossParam.key, false);

            var isUniqueArenaBossParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForUniqueArenaBosses);
            rule.isUniqueArenaBoss = payload.Get<bool>(isUniqueArenaBossParam.key, false);

            var isUniqueEnemyParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForUniqueEnemies);
            rule.isUniqueEnemy = payload.Get<bool>(isUniqueEnemyParam.key, false);

            var exceptIdsParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ExceptIds);
            rule.exceptIds = payload.Get<List<string>>(exceptIdsParam.key, null);

            var exceptNamesParam = EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ExceptNames);
            rule.exceptNames = payload.Get<List<string>>(exceptNamesParam.key, null);

            rule.newFaction = newFaction;

            bool hasTargeting = !string.IsNullOrEmpty(rule.enemyID) ||
                                !string.IsNullOrEmpty(rule.enemyName) ||
                                rule.areaFamily != null ||
                                rule.targetFaction.HasValue ||
                                rule.area.HasValue ||
                                rule.isBoss ||
                                rule.isBossPawn ||
                                rule.isStoryBoss ||
                                rule.isUniqueArenaBoss ||
                                rule.isUniqueEnemy;

            if (!hasTargeting)
            {
                OutwardEnemiesBalancer.LogSL($"EventBusSubscriber@AddFactionRule: Rule has no targeting criteria!");
                return;
            }

            FactionRuleRegistryManager.Instance.AppendFactionRule(rule);
            FactionBalancerManager.Instance.ApplyFactionRule(rule);
        }
    }
}
