#if DEBUG
using OutwardEnemiesBalancer.Balancing;
using OutwardEnemiesBalancer.Managers;
using OutwardModsCommunicator.EventBus;
using System.Collections.Generic;
using UnityEngine;

namespace OutwardEnemiesBalancer.Events.Tests
{
    public static class BalancingTestRunner
    {
        private static Character _testCharacter;
        private static int _passCount = 0;
        private static int _failCount = 0;

        public static void RunAllTests()
        {
            Debug.Log("[BalancingTests] ================== Starting All Tests ==================");
            
            _passCount = 0;
            _failCount = 0;

            _testCharacter = FindTestCharacter();
            if (_testCharacter == null)
            {
                Debug.Log("[BalancingTests] No alive character found - skipping tests");
                return;
            }

            Debug.Log($"[BalancingTests] Using test character: {_testCharacter.Name} (UID: {_testCharacter.UID.Value})");

            TestModifierTypes();
            TestVitalStatClamping();
            TestVitalStats();
            TestDamageTypes();
            TestResistances();
            TestProtections();
            TestEnvironmentalStats();
            TestCombatStats();
            TestXmlSerialization();

            BalancingTestValidator.ClearOriginalStats(_testCharacter);
            
            Debug.Log("[BalancingTests] ================== Test Summary ==================");
            Debug.Log($"[BalancingTests] PASSED: {_passCount}");
            Debug.Log($"[BalancingTests] FAILED: {_failCount}");
            Debug.Log($"[BalancingTests] TOTAL: {_passCount + _failCount}");
            Debug.Log("[BalancingTests] ================== Tests Complete ==================");
        }

        private static Character FindTestCharacter()
        {
            CharacterAI[] aiArray = UnityEngine.Object.FindObjectsOfType<CharacterAI>();
            foreach (var ai in aiArray)
            {
                if (ai.Character != null && ai.Character.Alive && ai.Character.Stats != null)
                {
                    return ai.Character;
                }
            }
            return null;
        }

        public static void RecordResult(bool passed)
        {
            if (passed)
                _passCount++;
            else
                _failCount++;
        }

        private static void TestModifierTypes()
        {
            Debug.Log("[BalancingTests] ----- ModifierType Tests -----");

            float originalHealth = _testCharacter.Stats.BaseMaxHealth;
            float originalStamina = _testCharacter.Stats.MaxStamina;
            float originalMana = _testCharacter.Stats.BaseMaxMana;

            BalancingTestValidator.StoreOriginalStat(_testCharacter, EnemyBalanceStatType.MaxHealth);
            BalancingTestValidator.StoreOriginalStat(_testCharacter, EnemyBalanceStatType.MaxStamina);
            BalancingTestValidator.StoreOriginalStat(_testCharacter, EnemyBalanceStatType.MaxMana);

            float testValue = 500f;
            float scaleValue = 2f;
            float addValue = 100f;

            BalancingTestPublisher.PublishAddBalanceRuleByEnemyId(
                _testCharacter.UID.Value, "MaxHealth", testValue, ValueModifierType.Direct);
            bool directPass = BalancingTestValidator.ValidateFloat(
                "ModifierType.Direct - MaxHealth",
                _testCharacter.Stats.BaseMaxHealth,
                testValue);
            RecordResult(directPass);
            BalancingTestValidator.RestoreOriginalStats(_testCharacter);

            BalancingTestPublisher.PublishAddBalanceRuleByEnemyId(
                _testCharacter.UID.Value, "MaxHealth", scaleValue, ValueModifierType.Scale);
            bool scalePass = BalancingTestValidator.ValidateFloat(
                "ModifierType.Scale - MaxHealth (2x)",
                _testCharacter.Stats.BaseMaxHealth,
                originalHealth * scaleValue);
            RecordResult(scalePass);
            BalancingTestValidator.RestoreOriginalStats(_testCharacter);

            BalancingTestPublisher.PublishAddBalanceRuleByEnemyId(
                _testCharacter.UID.Value, "MaxHealth", addValue, ValueModifierType.Add);
            bool addPass = BalancingTestValidator.ValidateFloat(
                "ModifierType.Add - MaxHealth (+100)",
                _testCharacter.Stats.BaseMaxHealth,
                originalHealth + addValue);
            RecordResult(addPass);
            BalancingTestValidator.RestoreOriginalStats(_testCharacter);

            Debug.Log($"[BalancingTests] ----- ModifierType Tests Complete -----");
        }

        private static void TestVitalStatClamping()
        {
            Debug.Log("[BalancingTests] ----- Vital Stat Clamping Tests -----");

            BalancingTestValidator.StoreOriginalStat(_testCharacter, EnemyBalanceStatType.MaxHealth);

            BalancingTestPublisher.PublishAddBalanceRuleByEnemyId(
                _testCharacter.UID.Value, "MaxHealth", 0f, ValueModifierType.Direct);
            bool clampPass = BalancingTestValidator.ValidateFloat(
                "VitalStatClamping - MaxHealth min 1",
                _testCharacter.Stats.BaseMaxHealth,
                1f);
            RecordResult(clampPass);

            BalancingTestPublisher.PublishAddBalanceRuleByEnemyId(
                _testCharacter.UID.Value, "MaxHealth", -100f, ValueModifierType.Direct);
            bool negativeClampPass = BalancingTestValidator.ValidateFloat(
                "VitalStatClamping - MaxHealth no negative",
                _testCharacter.Stats.BaseMaxHealth,
                1f);
            RecordResult(negativeClampPass);

            BalancingTestValidator.RestoreOriginalStats(_testCharacter);
            Debug.Log($"[BalancingTests] ----- Vital Stat Clamping Tests Complete -----");
        }

        private static void TestXmlSerialization()
        {
            Debug.Log("[BalancingTests] ----- XML Serialization Tests -----");

            XmlSerializationTests.RunTests();
            
            Debug.Log($"[BalancingTests] ----- XML Serialization Tests Complete -----");
        }

        private static void TestVitalStats()
        {
            Debug.Log("[BalancingTests] ----- Vital Stats Tests -----");

            var vitalStats = new EnemyBalanceStatType[]
            {
                EnemyBalanceStatType.MaxHealth,
                EnemyBalanceStatType.MaxStamina,
                EnemyBalanceStatType.MaxMana,
                EnemyBalanceStatType.HealthRegen,
                EnemyBalanceStatType.StaminaRegen,
                EnemyBalanceStatType.ManaRegen
            };

            foreach (var stat in vitalStats)
            {
                TestStatWithModifiers(stat, 500f, 2f, 100f);
            }

            Debug.Log($"[BalancingTests] ----- Vital Stats Tests Complete -----");
        }

        private static void TestDamageTypes()
        {
            Debug.Log("[BalancingTests] ----- Damage Types Tests -----");

            var damageStats = new EnemyBalanceStatType[]
            {
                EnemyBalanceStatType.PhysicalDamage,
                EnemyBalanceStatType.EtherealDamage,
                EnemyBalanceStatType.DecayDamage,
                EnemyBalanceStatType.ElectricDamage,
                EnemyBalanceStatType.FrostDamage,
                EnemyBalanceStatType.FireDamage
            };

            foreach (var stat in damageStats)
            {
                TestStatWithModifiers(stat, 50f, 1.5f, 25f);
            }

            Debug.Log($"[BalancingTests] ----- Damage Types Tests Complete -----");
        }

        private static void TestResistances()
        {
            Debug.Log("[BalancingTests] ----- Resistances Tests -----");

            var resistanceStats = new EnemyBalanceStatType[]
            {
                EnemyBalanceStatType.PhysicalResistance,
                EnemyBalanceStatType.EtherealResistance,
                EnemyBalanceStatType.DecayResistance,
                EnemyBalanceStatType.ElectricResistance,
                EnemyBalanceStatType.FrostResistance,
                EnemyBalanceStatType.FireResistance
            };

            foreach (var stat in resistanceStats)
            {
                TestStatWithModifiers(stat, 50f, 1.5f, 25f);
            }

            Debug.Log($"[BalancingTests] ----- Resistances Tests Complete -----");
        }

        private static void TestProtections()
        {
            Debug.Log("[BalancingTests] ----- Protections Tests -----");

            var protectionStats = new EnemyBalanceStatType[]
            {
                EnemyBalanceStatType.PhysicalProtection,
                EnemyBalanceStatType.EtherealProtection,
                EnemyBalanceStatType.DecayProtection,
                EnemyBalanceStatType.ElectricProtection,
                EnemyBalanceStatType.FrostProtection,
                EnemyBalanceStatType.FireProtection
            };

            foreach (var stat in protectionStats)
            {
                TestStatWithModifiers(stat, 20f, 1.5f, 10f);
            }

            Debug.Log($"[BalancingTests] ----- Protections Tests Complete -----");
        }

        private static void TestEnvironmentalStats()
        {
            Debug.Log("[BalancingTests] ----- Environmental Stats Tests -----");

            var environmentalStats = new EnemyBalanceStatType[]
            {
                EnemyBalanceStatType.ColdProtection,
                EnemyBalanceStatType.HeatProtection,
                EnemyBalanceStatType.ColdRegenRate,
                EnemyBalanceStatType.HeatRegenRate,
                EnemyBalanceStatType.CorruptionProtection,
                EnemyBalanceStatType.Waterproof
            };

            foreach (var stat in environmentalStats)
            {
                TestStatWithModifiers(stat, 30f, 1.5f, 15f);
            }

            Debug.Log($"[BalancingTests] ----- Environmental Stats Tests Complete -----");
        }

        private static void TestCombatStats()
        {
            Debug.Log("[BalancingTests] ----- Combat Stats Tests -----");

            var combatStats = new EnemyBalanceStatType[]
            {
                EnemyBalanceStatType.Impact,
                EnemyBalanceStatType.ImpactResistance,
                EnemyBalanceStatType.MovementSpeed,
                EnemyBalanceStatType.AttackSpeed,
                EnemyBalanceStatType.SkillCooldownModifier,
                EnemyBalanceStatType.DodgeInvulnerabilityModifier,
                EnemyBalanceStatType.GlobalStatusResistance
            };

            foreach (var stat in combatStats)
            {
                TestStatWithModifiers(stat, 20f, 1.5f, 10f);
            }

            Debug.Log($"[BalancingTests] ----- Combat Stats Tests Complete -----");
        }

        private static void TestStatWithModifiers(EnemyBalanceStatType stat, float directValue, float scaleValue, float addValue)
        {
            string statName = stat.ToString();
            float originalValue = BalancingTestValidator.GetStatValue(_testCharacter, stat);

            BalancingTestValidator.StoreOriginalStat(_testCharacter, stat);

            BalancingTestPublisher.PublishAddBalanceRuleByEnemyId(
                _testCharacter.UID.Value, statName, directValue, ValueModifierType.Direct);
            float expected = directValue;
            bool directPass = BalancingTestValidator.ValidateFloat(
                $"Direct.{statName}",
                BalancingTestValidator.GetStatValue(_testCharacter, stat),
                expected);
            RecordResult(directPass);
            BalancingTestValidator.RestoreOriginalStats(_testCharacter);

            BalancingTestPublisher.PublishAddBalanceRuleByEnemyId(
                _testCharacter.UID.Value, statName, scaleValue, ValueModifierType.Scale);
            expected = originalValue * scaleValue;
            bool scalePass = BalancingTestValidator.ValidateFloat(
                $"Scale.{statName}",
                BalancingTestValidator.GetStatValue(_testCharacter, stat),
                expected);
            RecordResult(scalePass);
            BalancingTestValidator.RestoreOriginalStats(_testCharacter);

            BalancingTestPublisher.PublishAddBalanceRuleByEnemyId(
                _testCharacter.UID.Value, statName, addValue, ValueModifierType.Add);
            expected = originalValue + addValue;
            bool addPass = BalancingTestValidator.ValidateFloat(
                $"Add.{statName}",
                BalancingTestValidator.GetStatValue(_testCharacter, stat),
                expected);
            RecordResult(addPass);
            BalancingTestValidator.RestoreOriginalStats(_testCharacter);
        }
    }
}
#endif
