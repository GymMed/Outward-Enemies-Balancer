#if DEBUG
using OutwardEnemiesBalancer.Balancing;
using UnityEngine;

namespace OutwardEnemiesBalancer.Events.Tests
{
    public static class EnumStringTestValidator
    {
        private static readonly float Tolerance = 0.01f;

        public static bool ValidateModifierResult(float original, float modifierValue, string modifierTypeString, float expectedResult)
        {
            // Convert string to enum and validate using existing logic
            if (System.Enum.TryParse<ValueModifierType>(modifierTypeString, ignoreCase: true, out var modType))
            {
                return BalancingTestValidator.ValidateModifierResult(original, modifierValue, modType, expectedResult);
            }
            
            Debug.LogWarning($"[EnumStringTestValidator] Could not parse modifier type: {modifierTypeString}");
            return false;
        }

        public static bool ValidateFloat(string testName, float actual, float expected)
        {
            bool passed = Mathf.Abs(actual - expected) < Tolerance;
            Debug.Log($"[EnumStringTestValidator] {testName}: " +
                $"Expected={expected:F2}, Actual={actual:F2}, " +
                $"Result={(passed ? "PASS" : "FAIL")}");
            return passed;
        }

        public static bool ValidateFactionChange(Character character, Character.Factions expectedFaction)
        {
            if (character == null)
            {
                Debug.LogWarning("[EnumStringTestValidator] Character is null - cannot validate faction");
                return false;
            }

            // Check current faction
            Character.Factions currentFaction = character.Faction;
            bool passed = currentFaction == expectedFaction;

            Debug.Log($"[EnumStringTestValidator] Faction Change: " +
                $"Expected={expectedFaction}, Current={currentFaction}, " +
                $"Result={(passed ? "PASS" : "FAIL")}");

            return passed;
        }

        public static bool ValidateFactionNotChanged(Character character, Character.Factions originalFaction)
        {
            if (character == null)
            {
                Debug.LogWarning("[EnumStringTestValidator] Character is null - cannot validate faction");
                return false;
            }

            Character.Factions currentFaction = character.Faction;
            bool passed = currentFaction == originalFaction;

            Debug.Log($"[EnumStringTestValidator] Faction Unchanged: " +
                $"Original={originalFaction}, Current={currentFaction}, " +
                $"Result={(passed ? "PASS" : "FAIL")}");

            return passed;
        }
    }
}
#endif
