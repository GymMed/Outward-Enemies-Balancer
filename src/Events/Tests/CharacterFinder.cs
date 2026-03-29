#if DEBUG
using UnityEngine;

namespace OutwardEnemiesBalancer.Events.Tests
{
    public interface ICharacterFinder
    {
        Character FindTestCharacter();
    }

    public class CharacterAICharacterFinder : ICharacterFinder
    {
        public Character FindTestCharacter()
        {
            CharacterAI[] aiArray = Object.FindObjectsOfType<CharacterAI>();
            foreach (var ai in aiArray)
            {
                if (ai.Character != null && ai.Character.Alive && ai.Character.Stats != null)
                {
                    return ai.Character;
                }
            }
            return null;
        }
    }
}
#endif
