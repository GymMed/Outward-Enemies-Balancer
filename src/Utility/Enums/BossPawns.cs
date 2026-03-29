using OutwardEnemiesBalancer.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardEnemiesBalancer.Utility.Enums
{
    public enum BossPawns
    {
        Elite_Krypteia_warrior,
        Elite_Krypteia_Witch,
        Elite_Obsidian_Elemental,
    }

    public static class BossPawnsHelper
    {
        public static readonly Dictionary<BossPawns, EnemyIdentificationGroupData> Enemies = new()
        {
            { BossPawns.Elite_Krypteia_warrior, new("Balira", "KrypteiaGuard", "name_unpc_balira_01", "AbvgKMnPLUiffB6LzjaguQ", "Tower of Regrets Arena", "Unknown Arena", "CalderaDungeonsBosses") },
            { BossPawns.Elite_Krypteia_Witch, new("Balira", "KrypteiaMage", "name_unpc_balira_01", "MfBjNPYsvkODdyLjYrlXXw", "Tower of Regrets Arena", "Unknown Arena", "CalderaDungeonsBosses") },
            { BossPawns.Elite_Obsidian_Elemental, new(
                new ("Obsidian Elemental", "ObsidianElemental", "Wildlife_ObsidianElemental", "RM13rq4JTEqbuANnncMCKA", "Burning Tree Arena", "Unknown Arena", "EmercarDungeonsBosses"),
                new ("Obsidian Elemental (1)", "ObsidianElemental", "Wildlife_ObsidianElemental", "Qrq3e4nUpkS8CH3yd8J-ow", "Burning Tree Arena", "Unknown Arena", "EmercarDungeonsBosses")
            ) },
        };
    }
}
