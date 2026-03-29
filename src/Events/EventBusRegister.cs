using OutwardEnemiesBalancer.Utility.Enums;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;

namespace OutwardEnemiesBalancer.Events
{
    public static class EventBusRegister
    {
        private static readonly (string key, Type type, string description)[] TargetingParams =
        {
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.EnemyId),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.EnemyName),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.AreaFamily),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.Faction),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.AreaEnum),
        };

        private static readonly (string key, Type type, string description)[] UniqueEnemyParams =
        {
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForBosses),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForBossesPawns),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForStoryBosses),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForUniqueArenaBosses),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.IsForUniqueEnemies),
        };

        private static readonly (string key, Type type, string description)[] StatModParams =
        {
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.StatModifications),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ModifierType),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.StatType),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.Value),
        };

        private static readonly (string key, Type type, string description)[] ExceptionsParams =
        {
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ExceptIds),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ExceptNames),
        };

        private static readonly (string key, Type type, string description)[] VitalStatsParams =
        {
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.MaxHealth),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.MaxStamina),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.MaxMana),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.HealthRegen),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.StaminaRegen),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ManaRegen),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ModifierType),
        };

        private static readonly (string key, Type type, string description)[] EnvironmentalStatsParams =
        {
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ColdProtection),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.HeatProtection),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.CorruptionResistance),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.Waterproof),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ModifierType),
        };

        private static readonly (string key, Type type, string description)[] CombatStatsParams =
        {
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.Impact),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ImpactResistance),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.MovementSpeed),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.AttackSpeed),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ModifierType),
        };

        private static readonly (string key, Type type, string description)[] FactionRuleParams =
        {
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.NewFaction),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ExceptIds),
            EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ExceptNames),
        };

        public static void RegisterEvents()
        {
            EventBus.RegisterEvent(
                OutwardEnemiesBalancer.GUID,
                EventBusPublisher.Event_BalanceRuleAppended,
                "Published when a balancing rule is appended.",
                EnemyBalanceParamsHelper.Get(EnemyBalanceParams.BalanceRuleId)
            );

            EventBus.RegisterEvent(
                OutwardEnemiesBalancer.GUID,
                EventBusPublisher.Event_BalanceRuleRemoved,
                "Published when a balancing rule is removed.",
                EnemyBalanceParamsHelper.Get(EnemyBalanceParams.BalanceRuleId)
            );

            EventBus.RegisterEvent(
                OutwardEnemiesBalancer.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_AddBalanceRule,
                "Add a full balancing rule with all targeting options and stat modifications.",
                EnemyBalanceParamsHelper.Combine(
                    EnemyBalanceParamsHelper.Get(EnemyBalanceParams.BalanceRuleId),
                    TargetingParams, 
                    UniqueEnemyParams, 
                    StatModParams, 
                    ExceptionsParams
                )
            );

            EventBus.RegisterEvent(
                OutwardEnemiesBalancer.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_AddBalanceRuleByEnemyName,
                "Add balancing rule targeting by enemy name.",
                EnemyBalanceParamsHelper.Combine(
                    EnemyBalanceParamsHelper.Get(EnemyBalanceParams.BalanceRuleId),
                    EnemyBalanceParamsHelper.Get(EnemyBalanceParams.EnemyName),
                    StatModParams,
                    EnemyBalanceParamsHelper.Get(EnemyBalanceParams.ExceptIds)
                )
            );

            EventBus.RegisterEvent(
                OutwardEnemiesBalancer.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_AddBalanceRuleByEnemyId,
                "Add balancing rule targeting by enemy UID.",
                EnemyBalanceParamsHelper.Combine(
                    EnemyBalanceParamsHelper.Get(EnemyBalanceParams.BalanceRuleId),
                    EnemyBalanceParamsHelper.Get(EnemyBalanceParams.EnemyId),
                    StatModParams
                )
            );

            EventBus.RegisterEvent(
                OutwardEnemiesBalancer.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_AddBalanceRuleForBosses,
                "Add balancing rule for boss types.",
                EnemyBalanceParamsHelper.Combine(
                    EnemyBalanceParamsHelper.Get(EnemyBalanceParams.BalanceRuleId),
                    UniqueEnemyParams,
                    StatModParams,
                    ExceptionsParams
                )
            );

            EventBus.RegisterEvent(
                OutwardEnemiesBalancer.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_AddBalanceRuleForUniques,
                "Add balancing rule for unique enemies.",
                EnemyBalanceParamsHelper.Combine(
                    EnemyBalanceParamsHelper.Get(EnemyBalanceParams.BalanceRuleId),
                    UniqueEnemyParams,
                    StatModParams,
                    ExceptionsParams
                )
            );

            EventBus.RegisterEvent(
                OutwardEnemiesBalancer.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_AddBalanceRuleForArea,
                "Add balancing rule for area/family.",
                EnemyBalanceParamsHelper.Combine(
                    EnemyBalanceParamsHelper.Get(EnemyBalanceParams.BalanceRuleId),
                    TargetingParams,
                    StatModParams,
                    ExceptionsParams
                )
            );

            EventBus.RegisterEvent(
                OutwardEnemiesBalancer.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_ModifyVitalStats,
                "Modify vital stats (health, stamina, mana, regen).",
                VitalStatsParams
            );

            EventBus.RegisterEvent(
                OutwardEnemiesBalancer.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_ModifyEnvironmentalStats,
                "Modify environmental stats (cold/heat protection, corruption, waterproof).",
                EnvironmentalStatsParams
            );

            EventBus.RegisterEvent(
                OutwardEnemiesBalancer.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_ModifyCombatStats,
                "Modify combat stats (impact, movement speed, attack speed).",
                CombatStatsParams
            );

            EventBus.RegisterEvent(
                OutwardEnemiesBalancer.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_LoadBalanceRulesFromXml,
                "Load balancing rules from XML file.",
                EnemyBalanceParamsHelper.Get(EnemyBalanceParams.LoadBalanceRulesXmlPath)
            );

            EventBus.RegisterEvent(
                OutwardEnemiesBalancer.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_SaveBalanceRulesToXml,
                "Save balancing rules to XML file.",
                EnemyBalanceParamsHelper.Get(EnemyBalanceParams.StoreBalanceRulesXmlPath)
            );

            EventBus.RegisterEvent(
                OutwardEnemiesBalancer.EVENTS_LISTENER_GUID,
                EventBusSubscriber.Event_AddFactionRule,
                "Add a faction rule to change enemy factions.",
                EnemyBalanceParamsHelper.Combine(
                    TargetingParams,
                    UniqueEnemyParams,
                    FactionRuleParams
                )
            );
        }
    }
}
