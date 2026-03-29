using OutwardEnemiesBalancer.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardEnemiesBalancer.Utility.Enums
{
    public enum StoryBosseses
    {
        Crimson_Avatar,
        Djinn,
        Forge_Master,
        Light_Mender,
        Plague_Doctor,
    }

    public static class StoryBossesHelper
    {
        public static readonly Dictionary<StoryBosseses, EnemyIdentificationGroupData> Enemies = new()
        {
            { StoryBosseses.Crimson_Avatar, new("Burning Man", "CrimsonAvatar (1)", "Undead_BurningMan", "4eeggsSn2Eyah4IjjqvpYQ", "Scarlet Sanctuary", "Scarlet Sanctuary") },
            { StoryBosseses.Djinn, new("Gold Lich", "LichGold", "Undead_LichGold", "EwoPQ0iVwkK-XtNuaVPf3g", "Oil Refinery", "Oil Refinery") },
            { StoryBosseses.Forge_Master, new("Jade Lich", "LichRust", "Undead_LichJade", "shyc5M7b-UGVHBZsJMdP4Q", "Forgotten Research Laboratory", "Forgotten Research Laboratory") },
            { StoryBosseses.Light_Mender, new("Gold Lich", "LichGold", "Undead_LichGold", "EwoPQ0iVwkK-XtNuaVPf3g", "Spire of Light", "Spire of Light") },
            { StoryBosseses.Plague_Doctor, new("Jade Lich", "LichJade", "Undead_LichJade", "8sjFFBPMvkuJcrcyIYs-KA", "Dark Ziggurat", "Dark Ziggurat Interior") }
        };
    }
}
