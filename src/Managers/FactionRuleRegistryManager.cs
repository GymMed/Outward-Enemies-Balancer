using OutwardEnemiesBalancer.Events;
using OutwardModsCommunicator.EventBus;
using System.Collections.Generic;
using System.Linq;

namespace OutwardEnemiesBalancer.Managers
{
    public class FactionRuleRegistryManager
    {
        private static FactionRuleRegistryManager _instance;

        private FactionRuleRegistryManager()
        {
        }

        public static FactionRuleRegistryManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FactionRuleRegistryManager();

                return _instance;
            }
        }

        public List<FactionRule> factionRules = new List<FactionRule>();

        public void AppendFactionRules(List<FactionRule> rules)
        {
            foreach (FactionRule rule in rules)
            {
                AppendFactionRule(rule);
            }
        }

        public void AppendFactionRule(FactionRule rule)
        {
            var existing = factionRules.FirstOrDefault(r => r.id == rule.id);

            if (existing != null)
            {
                OutwardEnemiesBalancer.LogSL($"FactionRule '{rule.id}' already exists. Replacing.");
                factionRules.Remove(existing);
            }

            factionRules.Add(rule);
            EventBusPublisher.SendAppendFactionRule(rule.id);
        }

        public void RemoveFactionRuleById(string id)
        {
            var rulesToRemove = factionRules.Where(rule => rule.id == id).ToList();

            foreach (var rule in rulesToRemove)
            {
                RemoveFactionRule(rule);
            }
        }

        public void RemoveFactionRule(FactionRule rule)
        {
            factionRules.Remove(rule);
            EventBusPublisher.SendRemoveFactionRule(rule.id);
        }

        public List<FactionRule> GetMatchingRules(Character character)
        {
            List<FactionRule> output = new List<FactionRule>();

            foreach (FactionRule rule in factionRules)
            {
                if (rule.Matches(character))
                    output.Add(rule);
            }

            return output;
        }

        public void Clear()
        {
            factionRules.Clear();
        }
    }
}
