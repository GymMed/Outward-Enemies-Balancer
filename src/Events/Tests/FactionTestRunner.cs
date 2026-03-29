#if DEBUG
using OutwardEnemiesBalancer.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace OutwardEnemiesBalancer.Events.Tests
{
    public static class FactionTestRunner
    {
        private static Character _testCharacter;
        private static int _passCount = 0;
        private static int _failCount = 0;

        public static void RunAllTests()
        {
            Debug.Log("[FactionTests] ================== Starting Faction Tests ==================");
            
            _passCount = 0;
            _failCount = 0;

            _testCharacter = FindTestCharacter();
            if (_testCharacter == null)
            {
                Debug.Log("[FactionTests] No alive character found - skipping tests");
                return;
            }

            Debug.Log($"[FactionTests] Using test character: {_testCharacter.Name} (UID: {_testCharacter.UID.Value})");
            Debug.Log($"[FactionTests] Original faction: {_testCharacter.Faction}");

            TestFactionChangeByEnemyId();
            TestFactionChangeByEnemyName();
            TestFactionRevert();
            TestFactionChangeWithExceptions();

            FactionTestValidator.RestoreOriginalFaction(_testCharacter);
            FactionTestValidator.ClearStoredFactions();
            
            Debug.Log("[FactionTests] ================== Test Summary ==================");
            Debug.Log($"[FactionTests] PASSED: {_passCount}");
            Debug.Log($"[FactionTests] FAILED: {_failCount}");
            Debug.Log($"[FactionTests] TOTAL: {_passCount + _failCount}");
            Debug.Log("[FactionTests] ================== Tests Complete ==================");
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

        private static void TestFactionChangeByEnemyId()
        {
            Debug.Log("[FactionTests] ----- TestFactionChangeByEnemyId -----");

            FactionTestValidator.StoreOriginalFaction(_testCharacter);
            Character.Factions originalFaction = _testCharacter.Faction;
            Character.Factions targetFaction = Character.Factions.Player;

            FactionTestPublisher.PublishAddFactionRuleByEnemyId(_testCharacter.UID.Value, targetFaction);
            
            bool changePass = FactionTestValidator.ValidateFaction(
                "FactionChangeByEnemyId - Changed",
                _testCharacter.Faction,
                targetFaction);
            RecordResult(changePass);

            FactionTestValidator.RestoreOriginalFaction(_testCharacter);
            
            bool revertPass = FactionTestValidator.ValidateFaction(
                "FactionChangeByEnemyId - Reverted",
                _testCharacter.Faction,
                originalFaction);
            RecordResult(revertPass);

            Debug.Log("[FactionTests] ----- TestFactionChangeByEnemyId Complete -----");
        }

        private static void TestFactionChangeByEnemyName()
        {
            Debug.Log("[FactionTests] ----- TestFactionChangeByEnemyName -----");

            FactionTestValidator.StoreOriginalFaction(_testCharacter);
            Character.Factions originalFaction = _testCharacter.Faction;
            Character.Factions targetFaction = Character.Factions.Player;

            FactionTestPublisher.PublishAddFactionRuleByEnemyName(_testCharacter.Name, targetFaction);
            
            bool changePass = FactionTestValidator.ValidateFaction(
                "FactionChangeByEnemyName - Changed",
                _testCharacter.Faction,
                targetFaction);
            RecordResult(changePass);

            FactionTestValidator.RestoreOriginalFaction(_testCharacter);
            
            bool revertPass = FactionTestValidator.ValidateFaction(
                "FactionChangeByEnemyName - Reverted",
                _testCharacter.Faction,
                originalFaction);
            RecordResult(revertPass);

            Debug.Log("[FactionTests] ----- TestFactionChangeByEnemyName Complete -----");
        }

        private static void TestFactionRevert()
        {
            Debug.Log("[FactionTests] ----- TestFactionRevert -----");

            FactionTestValidator.StoreOriginalFaction(_testCharacter);
            Character.Factions originalFaction = _testCharacter.Faction;

            Character.Factions firstFaction = Character.Factions.Player;
            FactionTestPublisher.PublishAddFactionRuleByEnemyId(_testCharacter.UID.Value, firstFaction);
            
            bool firstChangePass = FactionTestValidator.ValidateFaction(
                "FactionRevert - First Change",
                _testCharacter.Faction,
                firstFaction);
            RecordResult(firstChangePass);

            Character.Factions secondFaction = Character.Factions.Player;
            FactionTestPublisher.PublishAddFactionRuleByEnemyId(_testCharacter.UID.Value, secondFaction);
            
            bool secondChangePass = FactionTestValidator.ValidateFaction(
                "FactionRevert - Second Change",
                _testCharacter.Faction,
                secondFaction);
            RecordResult(secondChangePass);

            FactionTestValidator.RestoreOriginalFaction(_testCharacter);
            
            bool revertPass = FactionTestValidator.ValidateFaction(
                "FactionRevert - Back to Original",
                _testCharacter.Faction,
                originalFaction);
            RecordResult(revertPass);

            Debug.Log("[FactionTests] ----- TestFactionRevert Complete -----");
        }

        private static void TestFactionChangeWithExceptions()
        {
            Debug.Log("[FactionTests] ----- TestFactionChangeWithExceptions -----");

            FactionTestValidator.StoreOriginalFaction(_testCharacter);
            Character.Factions originalFaction = _testCharacter.Faction;
            Character.Factions targetFaction = Character.Factions.Player;

            string nonExistentName = "NonExistentEnemy12345";
            var exceptNames = new List<string> { nonExistentName };
            
            FactionTestPublisher.PublishAddFactionRuleWithExceptions(
                _testCharacter.Name, exceptNames, targetFaction);
            
            bool exceptionPass = FactionTestValidator.ValidateFaction(
                "FactionChangeWithExceptions - Exception Name Ignored",
                _testCharacter.Faction,
                targetFaction);
            RecordResult(exceptionPass);

            FactionTestValidator.RestoreOriginalFaction(_testCharacter);
            
            bool revertPass = FactionTestValidator.ValidateFaction(
                "FactionChangeWithExceptions - Reverted",
                _testCharacter.Faction,
                originalFaction);
            RecordResult(revertPass);

            Debug.Log("[FactionTests] ----- TestFactionChangeWithExceptions Complete -----");
        }
    }
}
#endif
