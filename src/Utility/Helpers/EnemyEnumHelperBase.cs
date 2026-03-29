using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardEnemiesBalancer.Utility.Helpers
{
    public abstract class EnemyEnumHelperBase<T> where T : Enum
    {
        protected abstract Dictionary<T, string> EnemyNames { get; }

        public T GetEnumFromCharacter(Character character)
        {
            if (character == null)
                return default;

            foreach (var kvp in EnemyNames)
            {
                if (character.Name.Equals(kvp.Value, StringComparison.OrdinalIgnoreCase))
                    return kvp.Key;
            }

            return default;
        }

        public string GetEnemyName(T enemy)
        {
            return EnemyNames.TryGetValue(enemy, out string name) ? name : enemy.ToString();
        }
    }
}
