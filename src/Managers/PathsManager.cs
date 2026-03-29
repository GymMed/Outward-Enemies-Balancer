using BepInEx;
using System.IO;

namespace OutwardEnemiesBalancer.Managers
{
    public static class PathsManager
    {
        public const string ConfigDirectoryName = "Enemies_Balancer";
        public static readonly string ConfigPath = Path.Combine(OutwardModsCommunicator.Managers.PathsManager.ConfigPath, ConfigDirectoryName);
        public static readonly string DefaultBalanceRulesPath = Path.Combine(ConfigPath, "BalanceRules.xml");

        public static void Initialize()
        {
            if (!Directory.Exists(ConfigPath))
            {
                Directory.CreateDirectory(ConfigPath);
            }
        }

        static PathsManager()
        {
            Initialize();
        }
    }
}
