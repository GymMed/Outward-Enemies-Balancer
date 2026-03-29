using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardEnemiesBalancer.Utility.Data
{
    public class EnemyIdentificationData
    {
        public string DisplayName;
        public string InternalName;
        public string LocKey;
        public string ID;
        public string WikiLocation;
        public string GameLocation;
        public string SceneName;

        public EnemyIdentificationData(string Name, string m_name, string m_nameLoc, string id, string wikiLocation, string gameLocation, string sceneName = "")
        {
            this.DisplayName = Name;
            this.InternalName = m_name;
            this.LocKey = m_nameLoc;
            this.ID = id;
            this.WikiLocation = wikiLocation;
            this.GameLocation = gameLocation;
            this.SceneName = sceneName;
        }

        public bool Matches(Character character, params Func<EnemyIdentificationData, Character, bool>[] comparers)
        {
            if (comparers == null || comparers.Length == 0)
            {
                return string.Equals(ID, character.UID.Value, StringComparison.Ordinal);
            }

            return comparers.Any(c => c(this, character));
        }
    }
}
