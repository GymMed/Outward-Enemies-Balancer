using OutwardEnemiesBalancer.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardEnemiesBalancer.Utility.Enums
{
    public enum UniqueEnemies
    {
        Accursed_Wendigo,
        Altered_Gargoyle,
        Ancestral_General,
        Ancestral_Soldier,
        Bloody_Alexis,
        Calygrey_Hero,
        Chromatic_Arcane_Elemental,
        Cracked_Gargoyle,
        Elemental_Parasite,
        Executioner_Bug,
        Ghost_of_Vanasse,
        Giant_Horror,
        Glacial_Tuanosaur,
        Golden_Matriarch,
        Grandmother_Medyse,
        Greater_Grotesque,
        Guardian_of_the_Compass,
        Kazite_Admiral,
        Lightning_Dancer,
        Liquid_Cooled_Golem,
        Luke_the_Pearlescent,
        Mad_Captains_Bones,
        Matriarch_Myrmitaur,
        Quartz_Elemental,
        Razorhorn_Stekosaur,
        Royal_Manticore,
        Rusted_Enforcer,
        Sandrose_Horror,
        She_Who_Speaks,
        That_Annoying_Troglodyte,
        The_Crusher,
        The_First_Cannibal,
        The_Last_Acolyte,
        Thunderbolt_Golem,
        Titanic_Guardian_Mk_7,
        Troglodyte_Archmage,
        Tyrant_of_the_Hive,
        Vile_Illuminator,
        Virulent_Hiveman,
        Volcanic_Gastrocin,
    }

    public static class UniqueEnemiesHelper
    {
        public static readonly Dictionary<UniqueEnemies, EnemyIdentificationGroupData> Enemies = new()
        {
            { UniqueEnemies.Accursed_Wendigo, new("Accursed Wendigo", "WendigoAccursed", "Unique_DefEd_Wendigo", "ElxncPIfuEuWkexSKkqFXg", "Corrupted Tombs", "Corrupted Tombs") },
            { UniqueEnemies.Altered_Gargoyle, new("Altered Gargoyle", "GargoyleAltered", "Unique_DefEd_AlteredGargoyle", "dhQVMNRU5kCIWsWRFYpDdw", "Ziggurat Passage", "Ziggurat Passage") },
            { UniqueEnemies.Ancestral_General, new("Ancestral General", "SkeletonBig (1)", "Unique_DefEd_AncestorBig", "XVuyIaCAVkatv89kId9Uqw", "Necropolis", "Necropolis") },
            { UniqueEnemies.Ancestral_Soldier, new("Ancestral Soldier", "SkeletonSmall (1)", "Unique_DefEd_AncestorSmall", "IwZYxBIQZkaXXMWT9HC5nA", "Necropolis", "Necropolis") },
            { UniqueEnemies.Bloody_Alexis, new("Bloody Alexis", "HumanArmoredThug", "Unique_DefEd_ArmoredThug", "VfYPPb4wcESdDVSiEq4UhA", "Undercity Passage", "Undercity Passage") },
            { UniqueEnemies.Calygrey_Hero, new("Calygrey Hero", "LionmanElite (1)", "Elite_Calygrey", "pMfhK69Stky7MvE9Ro0XMQ", "Steam Bath Tunnels", "Steam Bath Tunnels") },
            { UniqueEnemies.Chromatic_Arcane_Elemental, new("Chromatic Arcane Elemental", "ElementalChromatic", "Unique_DefEd_ChromaElemental", "RM13rq4JTEqbuANnncMCKA", "Compromised Mana Transfer Station", "Compromised Mana Transfer Station") },
            { UniqueEnemies.Cracked_Gargoyle, new("Cracked Gargoyle", "GargoyleCracked", "Unique_DefEd_CrackedGargoyle", "-McLNdZsNEa3itw-ny7YBw", "Ark of the Exiled", "Ark of the Exiled") },
            { UniqueEnemies.Elemental_Parasite, new("Crescent Shark", "ElementalParasite", "Wildlife_CrescentShark", "YDPy9S-An0-qGuvrJAM8yA", "Crumbling Loading Docks", "Crumbling Loading Docks") },
            { UniqueEnemies.Executioner_Bug, new("Executioner Bug", "ExecutionerBug", "Unique_DefEd_ExecutionerBug", "MWplmyrxokSXELL63oWcAg", "The Slide", "The Slide") },
            { UniqueEnemies.Ghost_of_Vanasse, new("Ghost of Vanasse", "GhostOfVanasse", "Undead_GhostVanasse", "R5UngwGS5EGWCH13toZO8Q", "Chersonese", "Chersonese") },
            { UniqueEnemies.Giant_Horror, new("Giant_Horror", "GiantHorror", "Giant_Horror", "SntMM-EzuE-ptgmqp4qkYQ", "Crumbling Loading Docks", "Crumbling Loading Docks") },
            { UniqueEnemies.Glacial_Tuanosaur, new("Glacial Tuanosaur", "TuanosaurIce", "Unique_DefEd_GlacialTuano", "1IKBT9DYc0yIkESwKtU40g", "Conflux Chambers", "Conflux Chambers") },
            { UniqueEnemies.Golden_Matriarch, new("Golden Matriarch", "SpecterMeleeMatriarch", "Unique_DefEd_GoldenMatriarch", "GI-aE4Ry7UOIyAYYk7emFg", "Voltaic Hatchery", "Voltaic Hatchery") },
            { UniqueEnemies.Grandmother_Medyse, new("Grandmother Medyse", "JellyFishMother", "Unique_DefEd_GrandmotherMedyse", "kp9R4kaoG02YfLdS9ROM4w", "Sulphuric Caverns", "Sulphuric Caverns") },
            { UniqueEnemies.Greater_Grotesque, new("Greater Grotesque", "ImmaculateHorrorGreater", "Unique_DefEd_GreaterGrotestue", "JmeufMpL_E6eYnqCYP2r3w", "Lost Golem Manufacturing Facility", "Lost Golem Manufacturing Facility") },
            { UniqueEnemies.Guardian_of_the_Compass, new("Guardian of the Compass", "GolemBoss", "Golem_Basic2", "BINT--E9xUaCgp4onBM54g", "The Walled Garden", "Abrassar") },
            { UniqueEnemies.Kazite_Admiral, new("Kazite Admiral", "HumanKaziteCaptain", "Unique_DefEd_KaziteCaptain", "XVuyIaCAVkatv89kId9Uqw", "Abandoned Living Quarters", "Abandoned Living Quarters") },
            { UniqueEnemies.Lightning_Dancer, new("Lightning Dancer", "BladeDancerLight", "Unique_DefEd_LightningDancer", "QRzc3AYY10CWyXOMgrIQTg", "Ancient Foundry", "Ancient Foundry") },
            { UniqueEnemies.Liquid_Cooled_Golem, new("Liquid-Cooled Golem", "GolemShieldedIce", "Unique_DefEd_LiquidGolem", "8ztut4_yiEmK0-NFLa-XNQ", "Destroyed Test Chambers", "Destroyed Test Chambers") },
            { UniqueEnemies.Luke_the_Pearlescent, new("Luke the Pearlescent", "NewBanditEquip_WhiteScavengerCaptainBoss_A (1)", "Bandit_Standard_Captain2", "XVuyIaCAVkatv89kId9Uqw", "Ruins of Old Levant", "Abrassar") },
            { UniqueEnemies.Mad_Captains_Bones, new("Mad Captain's Bones", "SkeletFighter", "Undead_Skeleton2", "JM_HjGXMlkq7a1Yb6gijgQ", "Pirates' Hideout", "Chersonese Misc. Dungeons") },
            { UniqueEnemies.Matriarch_Myrmitaur, new("Matriarch Myrmitaur", "MyrmElite (1)", "Elite_Myrm", "6sB4_5lOJU2bWuMHnOL4Ww", "Myrmitaur's Haven", "Myrmitaur's Haven") },
            { UniqueEnemies.Quartz_Elemental, new("Quartz Elemental", "ObsidianElementalQuartz", "Unique_DefEd_QuartzElemental", "LhhpSt8BO0aRN5mbeSuDrw", "The Grotto of Chalcedony", "The Grotto of Chalcedony") },
            { UniqueEnemies.Razorhorn_Stekosaur, new("Razorhorn Stekosaur", "SteakosaurBlack (1)", "Unique_DefEd_BlackSteko", "03dSXwJMRUuzGu8s3faATQ", "Reptilian Lair", "Reptilian Lair") },
            { UniqueEnemies.Royal_Manticore, new("The Royal Manticore", "RoyalManticore", "Wildlife_Manticore2", "RM13rq4JTEqbuANnncMCKA", "Enmerkar Forest", "Enmerkar Forest") },
            { UniqueEnemies.Rusted_Enforcer, new("Rusted Enforcer", "GolemRusted (1)", "Unique_DefEd_RustyGolem", "Ed2bzrgz5k-cRx3bUYTfmg", "Ghost Pass", "Ghost Pass") },
            { UniqueEnemies.Sandrose_Horror, new("Sandrose Horror", "ShelledHorrorBurning", "Unique_DefEd_SandroseHorror", "H7HoCKhBl0mC1j9UOECDrQ", "Sand Rose Cave", "Sand Rose Cave") },
            { UniqueEnemies.She_Who_Speaks, new("She Who Speaks", "AncientDwellerSpeak", "Unique_DefEd_BossDweller", "MBooN38mU0GPjQJGRuJ95g", "The Vault of Stone", "The Vault of Stone") },
            { UniqueEnemies.That_Annoying_Troglodyte, new("That Annoying Troglodyte", "TroglodyteAnnoying", "Unique_DefEd_AnnoyingTrog", "no-Z4ibpcEWbNntm_wRwZA", "Jade Quarry", "Jade Quarry") },
            { UniqueEnemies.The_Crusher, new("The Crusher", "HumanCrusher (1)", "Unique_DefEd_DesertCrusher", "AZL-EjXmhkOYB1obj0VkTw", "Ancestor's Resting Place", "Ancestor's Resting Place") },
            { UniqueEnemies.The_First_Cannibal, new("The First Cannibal", "WendigoCanibal", "Wildlife_Wendigo2", "wrYHXXh8J0KMhwoV8AC59w", "Face of the Ancients", "Face of the Ancients") },
            { UniqueEnemies.The_Last_Acolyte, new("The Last Acolyte", "HumanAcolyte (1)", "Unique_DefEd_LastAcolyte", "YeYzQP-gYUmSivlk5JCJew", "Stone Titan Caves", "Stone Titan Caves") },
            { UniqueEnemies.Thunderbolt_Golem, new("Thunderbolt Golem", "ForgeGolemLight (1)", "Unique_DefEd_ProtypeForgeGolem", "4qCAJzcAfEKNcl_c2k0r9g", "Electric Lab", "Electric Lab") },
            { UniqueEnemies.Titanic_Guardian_Mk_7, new(
                new ("Jade Lich", "TitanGolemHalberd", "Undead_LichJade", "65aI6XT89kmHa1bwJz5PGQ", "Ruined Warehouse", "Ruined Warehouse"),
                new ("Jade Lich", "TitanGolemHammer", "Undead_LichJade", "G_Q0oH1ttkWAZXCMuaAHjA", "Ruined Warehouse", "Ruined Warehouse"),
                new ("Jade Lich", "TitanGolemSword", "Undead_LichJade", "wj3frikyIkqwVv7myrc5gw", "Ruined Warehouse", "Ruined Warehouse")
            )},
            { UniqueEnemies.Troglodyte_Archmage, new("Troglodyte Archmage", "TroglodyteArcMageDefEd (1)", "Unique_DefEd_TrogMage", "syKWNGT3QUO3nXxPt1WEcQ", "Blister Burrow", "Blister Burrow") },
            { UniqueEnemies.Tyrant_of_the_Hive, new("Tyrant of the Hive", "HiveLord1AID+", "Undead_Hivelord2", "yOo-iKN3-0mAtZ2pG16pyw", "Forest Hives", "Forest Hives") },
            { UniqueEnemies.Vile_Illuminator, new("Vile Illuminator", "IlluminatorHorrorVile", "Unique_DefEd_VileIlluminator", "l5ignQfsE0Cv4imB9DZJ5w", "Cabal of Wind Temple", "Cabal of Wind Temple") },
            { UniqueEnemies.Virulent_Hiveman, new("Virulent Hiveman", "HiveManVirulent", "Unique_DefEd_VirulentHiveman", "v1PnLFpcxEmm_IrZaP-eyg", "Ancient Hive", "Ancient Hive") },
            { UniqueEnemies.Volcanic_Gastrocin, new("Volcanic Gastrocin", "SlughellVolcanic", "Unique_DefEd_VolcanicSlug", "fEFTRdXp1kOWX-Z9OMAkBg", "The Eldest Brother", "The Eldest Brother") },
        };
    }
}
