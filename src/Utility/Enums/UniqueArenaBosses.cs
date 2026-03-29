using OutwardEnemiesBalancer.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardEnemiesBalancer.Utility.Enums
{
    public enum UniqueArenaBosses
    {
        Ash_Giant_Highmonk,
        Brand_Squire,
        Breath_of_Darkness,
        Calixa_Boss,
        Concealed_Knight,
        Elite_Alpha_Tuanosaur,
        Elite_Ash_Giant,
        Elite_Beast_Golem,
        Elite_Boozu,
        Elite_Burning_Man,
        Elite_Crescent_Shark,
        Elite_Crimson_Avatar,
        Elite_Gargoyle_Alchemist,
        Elite_Gargoyle_Mage,
        Elite_Gargoyle_Warrior,
        Elite_Mantis_Shrimp,
        Elite_Sublime_Shell,
        Elite_Torcrab,
        Grandmother,
        Immaculate_Dreamer,
        Immaculates_Bird,
        Light_Mender,
        Plague_Doctor,
        Troglodyte_Queen,
    }

    public static class UniqueArenaBossesHelper
    {
        public static readonly Dictionary<UniqueArenaBosses, EnemyIdentificationGroupData> Enemies = new()
        {
            { UniqueArenaBosses.Ash_Giant_Highmonk, new("Ash Giant Priest", "EliteAshGiantPriest", "Giant_Priest", "UnIXpnDMzUSfBu4S-ZDgsA", "Giant's Village Arena (south)", "Unknown Arena", "HallowedDungeonsBosses") },
            { UniqueArenaBosses.Brand_Squire, new("Desert Bandit", "EliteBrandSquire", "Bandit_Desert_Basic", "sb0TOkOPS06jhp56AOYJCw", "Conflux Mountain Arena", "Unknown Arena", "ChersoneseDungeonsBosses") },
            { UniqueArenaBosses.Breath_of_Darkness, new("Breath of Darkness", "AncientDwellerDark", "Elite_Dweller", "JmeufMpL_E6eYnqCYP2r3w", "The Vault of Stone", "The Vault of Stone") },
            { UniqueArenaBosses.Calixa_Boss, new("Cyrene", "EliteCalixa", "Cyrene", "eCz766tEIEOWfK81om19wg", "Levant Arena", "Unknown Arena", "AbrassarDungeonsBosses") },
            { UniqueArenaBosses.Concealed_Knight, new("???", "NewBandit", "name_unpc_unknown_01", "XVuyIaCAVkatv89kId9Uqw", "Shipwreck (Castaway)", "CierzoTutorial", "CierzoTutorial") },
            { UniqueArenaBosses.Elite_Alpha_Tuanosaur, new("Alpha Tuanosaur", "EliteTuanosaurAlpha", "Wildlife_TuanosaurAlpha", "El8bA54i4E6vZraXsVZMow", "Ziggurat Passage Arena", "Unknown Arena", "HallowedDungeonsBosses") },
            { UniqueArenaBosses.Elite_Ash_Giant, new(
                new("Ash Giant", "EliteAshGiantPaf", "Giant_Guard", "3vXChaIK90qgq03PmsHFCg", "Unknown Arena", "Unknown Arena", "HallowedDungeonsBosses"),
                new("Ash Giant", "EliteAshGiantPif", "Giant_Guard", "851czvFVDUaB42CgVzfKdg", "Unknown Arena", "Unknown Arena", "HallowedDungeonsBosses"),
                new("Ash Giant", "EliteAshGiantPouf", "Giant_Guard", "kNmmOHZzKU-82F3OoX9NXw", "Unknown Arena", "Unknown Arena", "HallowedDungeonsBosses")
            )},
            { UniqueArenaBosses.Elite_Beast_Golem, new("Beast Golem", "EliteBeastGolem", "Golem_Beast", "n83g2QJhwUyUrN469WC4jA", "Parched Shipwrecks Arena", "Unknown Arena", "AbrassarDungeonsBosses") },
            { UniqueArenaBosses.Elite_Boozu, new("Blade Dancer", "BoozuProudBeast", "Wildlife_BladeDancer", "2Ef5z9OfYkev7M7Oi9GN-A", "Mana Lake Arena", "Unknown Arena", "AntiqueFieldDungeonsBosses") },
            { UniqueArenaBosses.Elite_Burning_Man, new("Burning Man", "EliteBurningMan", "Undead_BurningMan", "JmeufMpL_E6eYnqCYP2r3w", "Burning Tree Arena", "Unknown Arena", "EmercarDungeonsBosses") },
            { UniqueArenaBosses.Elite_Crescent_Shark, new(
                new("Crescent Shark", "EliteCrescentShark", "Wildlife_CrescentShark", "RM13rq4JTEqbuANnncMCKA", "Electric Lab Arena", "Unknown Arena", "AbrassarDungeonsBosses"),
                new("Crescent Shark", "EliteCrescentShark (1)", "Wildlife_CrescentShark", "ElDi5-rvqEqJKcXhEdgwBQ", "Electric Lab Arena", "Unknown Arena", "AbrassarDungeonsBosses"),
                new("Crescent Shark", "EliteCrescentShark (2)", "Wildlife_CrescentShark", "z3sfjJtqQEmUZ_S6g2RPIg", "Electric Lab Arena", "Unknown Arena", "AbrassarDungeonsBosses")
            )},
            { UniqueArenaBosses.Elite_Crimson_Avatar, new("Burning Man", "CrimsonAvatarElite (1)", "Undead_BurningMan", "JmeufMpL_E6eYnqCYP2r3w", "Vault of Stone Arena", "Unknown Arena", "CalderaDungeonsBosses") },
            { UniqueArenaBosses.Elite_Gargoyle_Alchemist, new("Shell Horror", "GargoyleBossMelee (1)", "Horror_Shell", "k75QmVu5t0e_zIjHnUFbIQ", "New Sirocco Arena", "Unknown Arena", "CalderaDungeonsBosses") },
            { UniqueArenaBosses.Elite_Gargoyle_Mage, new("Shell Horror", "GargoyleBossMelee (1)", "Horror_Shell", "AKBYHSSMJUaH9ddLiz_SZA", "New Sirocco Arena", "Unknown Arena", "CalderaDungeonsBosses") },
            { UniqueArenaBosses.Elite_Gargoyle_Warrior, new("Shell Horror", "GargoyleBossMelee (1)", "Horror_Shell", "Z6yTTWK4u0GjDPfZ9X332A", "New Sirocco Arena", "Unknown Arena", "CalderaDungeonsBosses") },
            { UniqueArenaBosses.Elite_Mantis_Shrimp, new("Mantis Shrimp", "EliteMantisShrimp", "Wildlife_Shrimp", "RM13rq4JTEqbuANnncMCKA", "Voltaic Hatchery Arena", "Unknown Arena", "ChersoneseDungeonsBosses") },
            { UniqueArenaBosses.Elite_Sublime_Shell, new("Nicolas", "CageArmorBoss (1)", "Nicolas", "X-dfltOoGUm7YlCE_Li1zQ", "Isolated Windmill Arena", "Unknown Arena", "AntiqueFieldDungeonsBosses") },
            { UniqueArenaBosses.Elite_Torcrab, new("Wildlife_Torcrab", "TorcrabGiant (1)", "Wildlife_Torcrab", "gQDvpLQh3kimgwMmvXJc4g", "River of Red Arena", "Unknown Arena", "CalderaDungeonsBosses") },
            { UniqueArenaBosses.Grandmother, new("Ghost", "Grandmother", "Undead_Ghost", "7G5APgUksEGdQrBxKXr04g", "Tower of Regrets Arena", "Unknown Arena", "CalderaDungeonsBosses") },
            { UniqueArenaBosses.Immaculate_Dreamer, new("Immaculate", "EliteImmaculate", "Horror_Immaculate", "9jsiejBtHkOzeo4tOyyweg", "Cabal of Wind Temple Arena", "Unknown Arena", "EmercarDungeonsBosses") },
            { UniqueArenaBosses.Immaculates_Bird, new("Immaculate", "EliteSupremeShell (1)", "Horror_Immaculate", "JsyOv_Cwu0K0HlXyZInRQQ", "Immaculate's Camp Arena", "Unknown Arena", "AntiqueFieldDungeonsBosses") },
            { UniqueArenaBosses.Light_Mender, new("Gold Lich", "LichGold (1)", "Undead_LichGold", "v9mN1u1uMkaxsncBXhIM9A", "Spire of Light", "Unknown Arena", "EmercarDungeonsBosses") },
            { UniqueArenaBosses.Plague_Doctor, new("Jade Lich", "LichJade (1)", "Undead_LichJade", "GfWl16_MZ0uS7UYIKpS5Lg", "Dark Ziggurat", "Unknown Arena", "EmercarDungeonsBosses") },
            { UniqueArenaBosses.Troglodyte_Queen, new("Mana Troglodyte", "TroglodyteMana (1)", "Troglodyte_Mana", "pcQlY_whLUCC-FZel18VMg", "Blister Burrow Arena", "Unknown Arena", "ChersoneseDungeonsBosses") },
        };
    }
}
