#if DEBUG
using OutwardEnemiesBalancer.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace OutwardEnemiesBalancer.Events.Tests
{
    public static class FactionTestValidator
    {
        private static Dictionary<Character, Character.Factions> _originalFactions = new Dictionary<Character, Character.Factions>();

        public static void StoreOriginalFaction(Character character)
        {
            if (character == null)
                return;

            _originalFactions[character] = character.Faction;
        }

        public static Character.Factions GetStoredOriginalFaction(Character character)
        {
            if (_originalFactions.TryGetValue(character, out var faction))
                return faction;
            
            return character.Faction;
        }

        public static void RestoreOriginalFaction(Character character)
        {
            if (character == null)
                return;

            if (_originalFactions.TryGetValue(character, out var originalFaction))
            {
                character.ChangeFaction(originalFaction, true);
            }
        }

        public static void ClearStoredFactions()
        {
            _originalFactions.Clear();
        }

        public static bool ValidateFaction(string testName, Character.Factions currentFaction, Character.Factions expectedFaction)
        {
            bool passed = currentFaction == expectedFaction;
            return Validate(testName, passed, $"Current: {currentFaction}, Expected: {expectedFaction}");
        }

        public static bool Validate(string testName, bool passed, string details = "")
        {
            if (passed)
                Debug.Log($"[FactionTests] PASS: {testName}");
            else
                Debug.LogError($"[FactionTests] FAIL: {testName} - {details}");
            
            return passed;
        }
    }
}
#endif
