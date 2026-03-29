using OutwardEnemiesBalancer.Balancing;
using OutwardEnemiesBalancer.Balancing.Serializable;
using OutwardEnemiesBalancer.Utility.Helpers.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace OutwardEnemiesBalancer.Managers
{
    public class BalancingRulesSerializer
    {
        private static BalancingRulesSerializer _instance;

        private BalancingRulesSerializer()
        {
        }

        public static BalancingRulesSerializer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BalancingRulesSerializer();

                return _instance;
            }
        }

        public BalancingRulesFile Load(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    OutwardEnemiesBalancer.LogSL($"BalancingRules file not found at: {path}");
                    return null;
                }

                XmlSerializer serializer = new XmlSerializer(typeof(BalancingRulesFile));

                using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                return serializer.Deserialize(fs) as BalancingRulesFile;
            }
            catch (Exception ex)
            {
                OutwardEnemiesBalancer.LogSL($"Failed to load BalancingRules file at '{path}': {ex.Message}");
                return null;
            }
        }

        public void LoadPlayerBalanceRules()
        {
            if (!File.Exists(PathsManager.DefaultBalanceRulesPath))
                return;

            LoadBalanceRules(PathsManager.DefaultBalanceRulesPath);
        }

        public void LoadBalanceRules(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    OutwardEnemiesBalancer.LogSL($"BalancingRulesSerializer@LoadBalanceRules file not found at: {path}");
                    return;
                }

                BalancingRulesFile file = Load(path);

                if (file == null)
                    return;

                List<BalancingRule> rules = GetBalancingRules(file);
                BalancingRuleRegistryManager.Instance.AppendBalancingRules(rules);
            }
            catch (Exception ex)
            {
                OutwardEnemiesBalancer.LogSL($"BalancingRulesSerializer@LoadBalanceRules failed loading '{path}': {ex.Message}");
            }
        }

        public void SaveBalanceRulesToXml(string filePath, List<BalancingRule> rules)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var file = BuildBalancingRulesFile(rules);

                var serializer = new XmlSerializer(typeof(BalancingRulesFile));

                var xmlWriterSettings = new XmlWriterSettings
                {
                    Indent = true,
                    NewLineOnAttributes = false
                };

                using (var writer = XmlWriter.Create(filePath, xmlWriterSettings))
                {
                    serializer.Serialize(writer, file);
                }
            }
            catch (Exception ex)
            {
                OutwardEnemiesBalancer.LogSL($"BalancingRulesSerializer@SaveBalanceRulesToXml failed saving '{filePath}': {ex.Message}");
            }
        }

        public List<BalancingRule> LoadBalanceRulesFromXmlSync(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    OutwardEnemiesBalancer.LogSL($"LoadBalanceRulesFromXmlSync file not found at: {path}");
                    return null;
                }

                BalancingRulesFile file = Load(path);

                if (file == null)
                    return null;

                return GetBalancingRules(file);
            }
            catch (Exception ex)
            {
                OutwardEnemiesBalancer.LogSL($"LoadBalanceRulesFromXmlSync failed loading '{path}': {ex.Message}");
                return null;
            }
        }

        public List<BalancingRule> GetBalancingRules(BalancingRulesFile file)
        {
            List<BalancingRule> rules = new List<BalancingRule>();

            foreach (BalancingRuleSerializable serializable in file.Rules)
            {
                BalancingRule rule = new BalancingRule(serializable.Id);

                rule.enemyID = string.IsNullOrEmpty(serializable.EnemyID) ? null : serializable.EnemyID;
                rule.enemyName = serializable.EnemyName ?? "";
                rule.areaFamily = AreaFamiliesHelpers.GetAreaFamilyByName(serializable.AreaFamilyName);
                rule.area = AreaHelpers.GetAreaEnumFromAreaName(serializable.AreaName);
                if (!string.IsNullOrEmpty(serializable.FactionName) && 
                    Enum.TryParse<Character.Factions>(serializable.FactionName, out Character.Factions faction))
                {
                    rule.faction = faction;
                }
                rule.exceptIds = serializable.ExceptIds;
                rule.exceptNames = serializable.ExceptNames;
                rule.isBoss = serializable.IsBoss;
                rule.isBossPawn = serializable.IsBossPawn;
                rule.isStoryBoss = serializable.IsStoryBoss;
                rule.isUniqueArenaBoss = serializable.IsUniqueArenaBoss;
                rule.isUniqueEnemy = serializable.IsUniqueEnemy;

                if (serializable.StatModifications != null)
                {
                    foreach (StatModificationSerializable modSerializable in serializable.StatModifications)
                    {
                        rule.statModifications[modSerializable.StatName] = modSerializable.Value;
                    }
                }

                rules.Add(rule);
            }

            return rules;
        }

        public BalancingRulesFile BuildBalancingRulesFile(List<BalancingRule> rules)
        {
            var file = new BalancingRulesFile
            {
                Rules = new List<BalancingRuleSerializable>()
            };

            foreach (BalancingRule rule in rules)
            {
                var serializable = new BalancingRuleSerializable
                {
                    Id = rule.id ?? null,
                    EnemyID = rule.enemyID ?? null,
                    EnemyName = rule.enemyName ?? "",
                    AreaFamilyName = rule.areaFamily?.FamilyName ?? "",
                    AreaName = rule.area?.ToString() ?? "",
                    FactionName = rule.faction?.ToString() ?? "",
                    ExceptIds = rule.exceptIds ?? null,
                    ExceptNames = rule.exceptNames ?? null,
                    IsBoss = rule.isBoss,
                    IsBossPawn = rule.isBossPawn,
                    IsStoryBoss = rule.isStoryBoss,
                    IsUniqueArenaBoss = rule.isUniqueArenaBoss,
                    IsUniqueEnemy = rule.isUniqueEnemy,
                    StatModifications = new List<StatModificationSerializable>()
                };

                foreach (var kvp in rule.statModifications)
                {
                    if (kvp.Value.HasValue)
                    {
                        serializable.StatModifications.Add(new StatModificationSerializable
                        {
                            StatName = kvp.Key,
                            Value = kvp.Value.Value
                        });
                    }
                }

                file.Rules.Add(serializable);
            }

            return file;
        }

        public void LoadFactionRules(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    OutwardEnemiesBalancer.LogSL($"FactionRules file not found at: {path}");
                    return;
                }

                BalancingRulesFile file = Load(path);

                if (file == null || file.FactionRules == null)
                    return;

                List<FactionRule> rules = GetFactionRules(file);
                FactionRuleRegistryManager.Instance.AppendFactionRules(rules);
            }
            catch (Exception ex)
            {
                OutwardEnemiesBalancer.LogSL($"LoadFactionRules failed loading '{path}': {ex.Message}");
            }
        }

        public List<FactionRule> GetFactionRules(BalancingRulesFile file)
        {
            List<FactionRule> rules = new List<FactionRule>();

            if (file.FactionRules == null)
                return rules;

            foreach (FactionRuleSerializable serializable in file.FactionRules)
            {
                FactionRule rule = new FactionRule(serializable.Id);

                rule.enemyID = string.IsNullOrEmpty(serializable.EnemyID) ? null : serializable.EnemyID;
                rule.enemyName = serializable.EnemyName ?? "";
                rule.areaFamily = AreaFamiliesHelpers.GetAreaFamilyByName(serializable.AreaFamilyName);
                rule.area = AreaHelpers.GetAreaEnumFromAreaName(serializable.AreaName);

                if (!string.IsNullOrEmpty(serializable.TargetFactionName) &&
                    Enum.TryParse<Character.Factions>(serializable.TargetFactionName, out Character.Factions targetFaction))
                {
                    rule.targetFaction = targetFaction;
                }

                if (!string.IsNullOrEmpty(serializable.NewFactionName) &&
                    Enum.TryParse<Character.Factions>(serializable.NewFactionName, out Character.Factions newFaction))
                {
                    rule.newFaction = newFaction;
                }

                rule.exceptIds = serializable.ExceptIds;
                rule.exceptNames = serializable.ExceptNames;
                rule.isBoss = serializable.IsBoss;
                rule.isBossPawn = serializable.IsBossPawn;
                rule.isStoryBoss = serializable.IsStoryBoss;
                rule.isUniqueArenaBoss = serializable.IsUniqueArenaBoss;
                rule.isUniqueEnemy = serializable.IsUniqueEnemy;

                rules.Add(rule);
            }

            return rules;
        }

        public void SaveFactionRulesToXml(string filePath, List<FactionRule> rules)
        {
            try
            {
                var file = BuildFactionRulesFile(rules);

                var serializer = new XmlSerializer(typeof(BalancingRulesFile));

                var xmlWriterSettings = new XmlWriterSettings
                {
                    Indent = true,
                    NewLineOnAttributes = false
                };

                using (var writer = XmlWriter.Create(filePath, xmlWriterSettings))
                {
                    serializer.Serialize(writer, file);
                }
            }
            catch (Exception ex)
            {
                OutwardEnemiesBalancer.LogSL($"SaveFactionRulesToXml failed saving '{filePath}': {ex.Message}");
            }
        }

        public BalancingRulesFile BuildFactionRulesFile(List<FactionRule> rules)
        {
            var file = new BalancingRulesFile
            {
                FactionRules = new List<FactionRuleSerializable>()
            };

            foreach (FactionRule rule in rules)
            {
                var serializable = new FactionRuleSerializable
                {
                    Id = rule.id ?? null,
                    EnemyID = rule.enemyID ?? null,
                    EnemyName = rule.enemyName ?? "",
                    AreaFamilyName = rule.areaFamily?.FamilyName ?? "",
                    AreaName = rule.area?.ToString() ?? "",
                    TargetFactionName = rule.targetFaction?.ToString() ?? "",
                    NewFactionName = rule.newFaction.ToString(),
                    ExceptIds = rule.exceptIds ?? null,
                    ExceptNames = rule.exceptNames ?? null,
                    IsBoss = rule.isBoss,
                    IsBossPawn = rule.isBossPawn,
                    IsStoryBoss = rule.isStoryBoss,
                    IsUniqueArenaBoss = rule.isUniqueArenaBoss,
                    IsUniqueEnemy = rule.isUniqueEnemy
                };

                file.FactionRules.Add(serializable);
            }

            return file;
        }
    }
}
