using UnityEngine;

namespace OutwardEnemiesBalancer.Managers
{
    public class FactionBalancerManager
    {
        private static FactionBalancerManager _instance;

        private FactionBalancerManager()
        {
        }

        public static FactionBalancerManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FactionBalancerManager();

                return _instance;
            }
        }

        public void ApplyFactionRules()
        {
            CharacterAI[] aiArray = UnityEngine.Object.FindObjectsOfType<CharacterAI>();

#if DEBUG
            int totalAI = aiArray.Length;
            int validCharacters = 0;
            int aliveCharacters = 0;
            int charactersWithRules = 0;
            int totalChanges = 0;
#endif

            foreach (CharacterAI ai in aiArray)
            {
                Character character = ai.Character;

#if DEBUG
                if (character != null)
                    validCharacters++;
#endif

                if (character == null)
                    continue;

                if (!character.Alive)
                    continue;

#if DEBUG
                aliveCharacters++;
#endif

                var matchingRules = FactionRuleRegistryManager.Instance.GetMatchingRules(character);

                if (matchingRules.Count > 0)
                {
#if DEBUG
                    charactersWithRules++;
#endif

                    foreach (var rule in matchingRules)
                    {
                        ApplyRuleToCharacter(character, rule);
#if DEBUG
                        totalChanges++;
#endif
                    }
                }
            }

#if DEBUG
            Debug.Log($"[FactionBalancing] AI Found: {totalAI} | Valid Chars: {validCharacters} | Alive: {aliveCharacters} | With Rules: {charactersWithRules} | Changes Applied: {totalChanges}");
#endif
        }

        public void ApplyFactionRule(FactionRule rule)
        {
            CharacterAI[] aiArray = UnityEngine.Object.FindObjectsOfType<CharacterAI>();

#if DEBUG
            int totalAI = aiArray.Length;
            int validCharacters = 0;
            int aliveCharacters = 0;
            int matchedCharacters = 0;
            int totalChanges = 0;
#endif

            foreach (CharacterAI ai in aiArray)
            {
                Character character = ai.Character;

#if DEBUG
                if (character != null)
                    validCharacters++;
#endif

                if (character == null)
                    continue;

                if (!character.Alive)
                    continue;

#if DEBUG
                aliveCharacters++;
#endif

                if (!rule.Matches(character))
                    continue;

#if DEBUG
                matchedCharacters++;
#endif

                ApplyRuleToCharacter(character, rule);
#if DEBUG
                totalChanges++;
#endif
            }

#if DEBUG
            Debug.Log($"[FactionBalancing-Single] Rule: {rule.id} | AI Found: {totalAI} | Valid Chars: {validCharacters} | Alive: {aliveCharacters} | Matched: {matchedCharacters} | Changes Applied: {totalChanges}");
#endif
        }

        private void ApplyRuleToCharacter(Character character, FactionRule rule)
        {
            if (character == null || rule.newFaction == default)
                return;

            Character.Factions oldFaction = character.Faction;
            character.ChangeFaction(rule.newFaction, true);

#if DEBUG
            Debug.Log($"[FactionBalancing-Debug] {character.Name} | Old Faction: {oldFaction} | New Faction: {rule.newFaction}");
#endif
        }
    }
}
