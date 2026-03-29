#if DEBUG
using OutwardEnemiesBalancer.Balancing;
using OutwardEnemiesBalancer.Utility.Helpers.Static;
using UnityEngine;

namespace OutwardEnemiesBalancer.Events.Tests
{
    public static class EnumStringTestRunner
    {
        // Dependency: character finder (follows DIP)
        private static ICharacterFinder _characterFinder;
        
        private static Character _testCharacter;
        private static Character.Factions _originalFaction;
        private static float _originalMaxHealth;
        private static int _passCount = 0;
        private static int _failCount = 0;

        public static void RunAllTests()
        {
            Debug.Log("[EnumStringTests] ================== Starting Enum String Tests ==================");

            _passCount = 0;
            _failCount = 0;

            // Use dependency injection for character finder (OCP - open for extension)
            _characterFinder = new CharacterAICharacterFinder();
            _testCharacter = _characterFinder.FindTestCharacter();

            if (_testCharacter == null)
            {
                Debug.Log("[EnumStringTests] No alive character found - skipping tests");
                return;
            }

            // Store original values for restoration
            _originalFaction = _testCharacter.Faction;
            _originalMaxHealth = _testCharacter.Stats.BaseMaxHealth;

            Debug.Log($"[EnumStringTests] Using test character: {_testCharacter.Name} (UID: {_testCharacter.UID.Value})");

            // Run test groups
            TestValueModifierTypeAsString();
            TestCharacterFactionAsString();
            TestAreaFamilyAsString();

            // Cleanup: restore original values
            RestoreOriginalValues();

            Debug.Log("[EnumStringTests] ================== Test Summary ==================");
            Debug.Log($"[EnumStringTests] PASSED: {_passCount}");
            Debug.Log($"[EnumStringTests] FAILED: {_failCount}");
            Debug.Log($"[EnumStringTests] TOTAL: {_passCount + _failCount}");
            Debug.Log("[EnumStringTests] ================== Tests Complete ==================");
        }

        public static void RunAllTests(ICharacterFinder characterFinder)
        {
            // Overload allowing dependency injection from outside (useful for testing the runner itself)
            _characterFinder = characterFinder;
            RunAllTests();
        }

        private static void RecordResult(bool passed)
        {
            if (passed)
                _passCount++;
            else
                _failCount++;
        }

        private static void TestValueModifierTypeAsString()
        {
            Debug.Log("[EnumStringTests] ----- ValueModifierType as String Tests -----");

            // Test "Direct" as string
            float testValue = 500f;
            BalancingTestValidator.StoreOriginalStat(_testCharacter, EnemyBalanceStatType.MaxHealth);

            EnumStringTestPublisher.PublishAddBalanceRuleByEnemyId_StringModifier(
                _testCharacter.UID.Value, "MaxHealth", testValue, "Direct");

            bool directPass = EnumStringTestValidator.ValidateFloat(
                "StringModifierType.Direct",
                _testCharacter.Stats.BaseMaxHealth,
                testValue);
            RecordResult(directPass);

            BalancingTestValidator.RestoreOriginalStats(_testCharacter);

            // Test "Scale" as string (case insensitive)
            float scaleValue = 2f;
            BalancingTestValidator.StoreOriginalStat(_testCharacter, EnemyBalanceStatType.MaxHealth);

            EnumStringTestPublisher.PublishAddBalanceRuleByEnemyId_StringModifier(
                _testCharacter.UID.Value, "MaxHealth", scaleValue, "scale");

            bool scalePass = EnumStringTestValidator.ValidateFloat(
                "StringModifierType.scale (case insensitive)",
                _testCharacter.Stats.BaseMaxHealth,
                _originalMaxHealth * scaleValue);
            RecordResult(scalePass);

            BalancingTestValidator.RestoreOriginalStats(_testCharacter);

            // Test "Add" as string
            float addValue = 100f;
            BalancingTestValidator.StoreOriginalStat(_testCharacter, EnemyBalanceStatType.MaxHealth);

            EnumStringTestPublisher.PublishAddBalanceRuleByEnemyId_StringModifier(
                _testCharacter.UID.Value, "MaxHealth", addValue, "ADD");

            bool addPass = EnumStringTestValidator.ValidateFloat(
                "StringModifierType.ADD (uppercase)",
                _testCharacter.Stats.BaseMaxHealth,
                _originalMaxHealth + addValue);
            RecordResult(addPass);

            BalancingTestValidator.RestoreOriginalStats(_testCharacter);

            Debug.Log("[EnumStringTests] ----- ValueModifierType as String Tests Complete -----");
        }

        private static void TestCharacterFactionAsString()
        {
            Debug.Log("[EnumStringTests] ----- Character.Factions as String Tests -----");

            // Test faction change using string
            // Note: Using valid faction values from Character.Factions enum
            // If original is Player, switch to Bandits, otherwise switch to Player
            Character.Factions targetFaction = _originalFaction == Character.Factions.Player 
                ? Character.Factions.Bandits 
                : Character.Factions.Player;
            string targetFactionString = targetFaction.ToString();

            // Use ByEnemyId to change faction
            EnumStringTestPublisher.PublishAddFactionRuleByEnemyId_StringFaction(
                _testCharacter.UID.Value, targetFactionString);

            bool factionPass = EnumStringTestValidator.ValidateFactionChange(
                _testCharacter, targetFaction);
            RecordResult(factionPass);

            // Restore original faction for next test
            RestoreFaction(_originalFaction);

            // Test case insensitive faction string - use "bandits" (lowercase)
            string lowercaseFaction = "banDiTs";
            EnumStringTestPublisher.PublishAddFactionRuleByEnemyId_StringFaction(
                _testCharacter.UID.Value, lowercaseFaction);

            bool caseInsensitivePass = EnumStringTestValidator.ValidateFactionChange(
                _testCharacter, Character.Factions.Bandits);
            RecordResult(caseInsensitivePass);

            RestoreFaction(_originalFaction);

            Debug.Log("[EnumStringTests] ----- Character.Factions as String Tests Complete -----");
        }

        private static void TestAreaFamilyAsString()
        {
            Debug.Log("[EnumStringTests] ----- AreaFamily as String Tests -----");

            // Test with AreaFamily string (will apply to all enemies in that area family)
            // This is a simple smoke test - we verify the event publishes without error
            // Actual matching depends on character being in the right area
            BalancingTestValidator.StoreOriginalStat(_testCharacter, EnemyBalanceStatType.MaxHealth);

            EnumStringTestPublisher.PublishAddBalanceRuleForArea_StringAreaFamily(
                "Impressed", "MaxHealth", 1.5f);

            // We expect no error, but result depends on character location
            // Just verify no exception was thrown
            Debug.Log("[EnumStringTests] AreaFamily string test completed (no exception)");

            BalancingTestValidator.RestoreOriginalStats(_testCharacter);

            Debug.Log("[EnumStringTests] ----- AreaFamily as String Tests Complete -----");
        }

        private static void RestoreOriginalValues()
        {
            RestoreFaction(_originalFaction);
            BalancingTestValidator.RestoreOriginalStats(_testCharacter);
        }

        private static void RestoreFaction(Character.Factions faction)
        {
            // Use the internal helper to restore faction
            var payload = new OutwardModsCommunicator.EventBus.EventPayload();
            payload.Set("enemyId", _testCharacter.UID.Value);
            payload.Set("newFaction", faction);

            OutwardModsCommunicator.EventBus.EventBus.Publish(
                OutwardEnemiesBalancer.EVENTS_LISTENER_GUID, 
                "AddFactionRule", 
                payload);
        }
    }
}
#endif
