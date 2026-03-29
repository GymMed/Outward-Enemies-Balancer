using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardEnemiesBalancer.Utility.Data
{
    public class EnemyIdentificationGroupData
    {
        public List<EnemyIdentificationData> Enemies { get; }

        public EnemyIdentificationGroupData(string displayName, string internalName, string locKey,
            string uid, string wikiLocation, string gameLocation, string sceneName = "")
        {
            Enemies = new List<EnemyIdentificationData>
            {
                new EnemyIdentificationData(displayName, internalName, locKey, uid, wikiLocation, gameLocation, sceneName)
            };
        }

        public EnemyIdentificationGroupData(params EnemyIdentificationData[] entries)
        {
            Enemies = new List<EnemyIdentificationData>();
            Enemies.AddRange(entries);
        }

        public bool Matches(Character character, params Func<EnemyIdentificationData, Character, bool>[] comparers)
        {
            return Enemies.Any(e => e.Matches(character, comparers));
        }

        public EnemyIdentificationData GetMatching(Character character)
        {
            return Enemies.FirstOrDefault(e => e.Matches(character));
        }
    }
}
