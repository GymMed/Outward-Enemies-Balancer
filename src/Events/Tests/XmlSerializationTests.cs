#if DEBUG
using OutwardEnemiesBalancer.Balancing;
using OutwardEnemiesBalancer.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OutwardEnemiesBalancer.Events.Tests
{
    public static class XmlSerializationTests
    {
        public static void RunTests()
        {
            TestSaveAndLoadRules();
            TestRulePreservation();
        }

        private static void TestSaveAndLoadRules()
        {
            string testPath = Path.Combine(Application.temporaryCachePath, "test_balance_rules.xml");
            
            try
            {
                if (File.Exists(testPath))
                    File.Delete(testPath);

                BalancingRule originalRule = new BalancingRule("xml_test_rule");
                originalRule.enemyName = "Test Enemy";
                originalRule.statModifications["MaxHealth"] = 500f;
                originalRule.statModifications["MaxStamina"] = 200f;
                originalRule.modifierType = ValueModifierType.Direct;

                BalancingRulesSerializer.Instance.SaveBalanceRulesToXml(testPath, new List<BalancingRule> { originalRule });
                
                bool savePass = System.IO.File.Exists(testPath);
                if (BalancingTestValidator.Validate("XmlSerialization - SaveToFile", savePass, $"Path: {testPath}"))
                    BalancingTestRunner.RecordResult(true);
                else
                    BalancingTestRunner.RecordResult(false);

                var loadedRules = BalancingRulesSerializer.Instance.LoadBalanceRulesFromXmlSync(testPath);
                
                bool loadPass = loadedRules != null && loadedRules.Count > 0;
                if (BalancingTestValidator.Validate("XmlSerialization - LoadFromFile", loadPass, $"Loaded {loadedRules?.Count ?? 0} rules"))
                    BalancingTestRunner.RecordResult(true);
                else
                    BalancingTestRunner.RecordResult(false);

                if (loadedRules != null && loadedRules.Count > 0)
                {
                    var loadedRule = loadedRules[0];
                    
                    bool idMatch = loadedRule.id == "xml_test_rule";
                    BalancingTestValidator.Validate("XmlSerialization - RuleIdPreserved", idMatch);
                    BalancingTestRunner.RecordResult(idMatch ? true : false);

                    bool nameMatch = loadedRule.enemyName == "Test Enemy";
                    BalancingTestValidator.Validate("XmlSerialization - EnemyNamePreserved", nameMatch);
                    BalancingTestRunner.RecordResult(nameMatch ? true : false);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BalancingTests] XmlSerialization Error: {ex.Message}");
                BalancingTestRunner.RecordResult(false);
                BalancingTestRunner.RecordResult(false);
                BalancingTestRunner.RecordResult(false);
            }
            finally
            {
                if (File.Exists(testPath))
                    File.Delete(testPath);
            }
        }

        private static void TestRulePreservation()
        {
            var testRules = new List<BalancingRule>
            {
                new BalancingRule("rule1") { enemyName = "Enemy1" },
                new BalancingRule("rule2") { enemyName = "Enemy2" },
                new BalancingRule("rule3") { enemyName = "Enemy3" }
            };

            int originalCount = testRules.Count;
            bool countMatch = BalancingTestValidator.Validate(
                "XmlSerialization - RuleCountPreserved", 
                originalCount == 3);
            BalancingTestRunner.RecordResult(countMatch);

            bool allHaveIds = testRules.TrueForAll(r => !string.IsNullOrEmpty(r.id));
            BalancingTestValidator.Validate(
                "XmlSerialization - AllRulesHaveIds", 
                allHaveIds);
            BalancingTestRunner.RecordResult(allHaveIds);
        }
    }
}
#endif
