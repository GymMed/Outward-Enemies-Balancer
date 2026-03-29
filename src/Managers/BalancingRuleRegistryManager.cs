using OutwardEnemiesBalancer.Events;
using OutwardModsCommunicator.EventBus;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutwardEnemiesBalancer.Managers
{
    public class BalancingRuleRegistryManager
    {
        private static BalancingRuleRegistryManager _instance;

        private BalancingRuleRegistryManager()
        {
        }

        public static BalancingRuleRegistryManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BalancingRuleRegistryManager();

                return _instance;
            }
        }

        public List<BalancingRule> balancingRules = new List<BalancingRule>();

        public void AppendBalancingRules(List<BalancingRule> rules)
        {
            foreach (BalancingRule rule in rules)
            {
                AppendBalancingRule(rule);
            }
        }

        public void AppendBalancingRule(BalancingRule rule)
        {
            var existing = balancingRules.FirstOrDefault(r => r.id == rule.id);

            if (existing != null)
            {
                StringBuilder mergeLog = new StringBuilder();
                mergeLog.AppendLine($"Rule '{rule.id}' already exists. Merging stat modifications:");
                mergeLog.AppendLine($"  Old mods: {CountStats(existing.statModifications)}");
                mergeLog.AppendLine($"  New mods: {CountStats(rule.statModifications)}");

                foreach (var kvp in rule.statModifications)
                {
                    string mergeType;
                    if (existing.statModifications.TryGetValue(kvp.Key, out float? oldValue))
                    {
                        mergeType = "updated";
                    }
                    else
                    {
                        mergeType = "added";
                    }
                    existing.statModifications[kvp.Key] = kvp.Value;
                    mergeLog.AppendLine($"    {mergeType}: {kvp.Key} = {kvp.Value}");
                }

                OutwardEnemiesBalancer.LogSL(mergeLog.ToString());
            }
            else
            {
                balancingRules.Add(rule);
                EventBusPublisher.SendAppendBalancingRule(rule.id);
            }
        }

        private string CountStats(Dictionary<string, float?> stats)
        {
            return stats != null ? stats.Count.ToString() : "0";
        }

        public void RemoveBalancingRuleById(string id)
        {
            var rulesToRemove = balancingRules.Where(rule => rule.id == id).ToList();

            foreach (var rule in rulesToRemove)
            {
                RemoveBalancingRule(rule);
            }
        }

        public void RemoveBalancingRule(BalancingRule rule)
        {
            balancingRules.Remove(rule);
            EventBusPublisher.SendRemoveBalancingRule(rule.id);
        }

        public List<BalancingRule> GetMatchingRules(Character character)
        {
            List<BalancingRule> output = new List<BalancingRule>();

            foreach (BalancingRule rule in balancingRules)
            {
                if (rule.Matches(character))
                    output.Add(rule);
            }

            return output;
        }
    }
}
