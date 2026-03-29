using OutwardModsCommunicator.EventBus;
using System;

namespace OutwardEnemiesBalancer.Events
{
    public static class EventBusPublisher
    {
        public const string Event_BalanceRuleAppended = "BalanceRuleAppended";
        public const string Event_BalanceRuleRemoved = "BalanceRuleRemoved";
        public const string Event_FactionRuleAppended = "FactionRuleAppended";
        public const string Event_FactionRuleRemoved = "FactionRuleRemoved";

        public static void SendAppendBalancingRule(string ruleId)
        {
            EventPayload payload = new EventPayload();
            payload.Set("balanceRuleId", ruleId);
            EventBus.Publish(OutwardEnemiesBalancer.GUID, Event_BalanceRuleAppended, payload);
        }

        public static void SendRemoveBalancingRule(string ruleId)
        {
            EventPayload payload = new EventPayload();
            payload.Set("balanceRuleId", ruleId);
            EventBus.Publish(OutwardEnemiesBalancer.GUID, Event_BalanceRuleRemoved, payload);
        }

        public static void SendAppendFactionRule(string ruleId)
        {
            EventPayload payload = new EventPayload();
            payload.Set("factionRuleId", ruleId);
            EventBus.Publish(OutwardEnemiesBalancer.GUID, Event_FactionRuleAppended, payload);
        }

        public static void SendRemoveFactionRule(string ruleId)
        {
            EventPayload payload = new EventPayload();
            payload.Set("factionRuleId", ruleId);
            EventBus.Publish(OutwardEnemiesBalancer.GUID, Event_FactionRuleRemoved, payload);
        }

#if DEBUG
        public static void PublishTests()
        {
            Tests.BalancingTestRunner.RunAllTests();
            Tests.FactionTestRunner.RunAllTests();
            Tests.EnumStringTestRunner.RunAllTests();
        }
#endif
    }
}
