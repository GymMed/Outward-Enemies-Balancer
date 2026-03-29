using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SideLoader;
using OutwardModsCommunicator;
using OutwardEnemiesBalancer.Events;
using OutwardEnemiesBalancer.Managers;
using System;
using System.IO;
using System.Reflection;

namespace OutwardEnemiesBalancer
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(SideLoader.SL.GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(OutwardModsCommunicator.OMC.GUID, BepInDependency.DependencyFlags.SoftDependency)]
    public class OutwardEnemiesBalancer : BaseUnityPlugin
    {
        public const string GUID = "gymmed.enemies_balancer";
        public const string NAME = "Enemies Balancer";
        public const string VERSION = "0.0.1";

        public const string EVENTS_LISTENER_GUID = GUID + "_*";

        public static string prefix = "[Enemies-Balancer]";

        internal static ManualLogSource Log;

        internal void Awake()
        {
            Log = this.Logger;
            LogMessage($"Hello world from {NAME} {VERSION}!");

            new Harmony(GUID).PatchAll();

            PathsManager.Initialize();

            EventBusRegister.RegisterEvents();
            EventBusSubscriber.AddSubscribers();

            SL.OnSceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded()
        {
            try
            {
                CharacterBalancerManager.Instance.ApplyBalancingRules();
                FactionBalancerManager.Instance.ApplyFactionRules();

#if DEBUG
                EventBusPublisher.PublishTests();
#endif
            }
            catch (Exception ex)
            {
                LogMessage($"Error applying rules: {ex.Message}");
            }
        }

        internal void Update()
        {
        }

        public static void LogMessage(string message)
        {
            Log.LogMessage($"{OutwardEnemiesBalancer.prefix} {message}");
        }

        public static void LogSL(string message)
        {
            SL.Log($"{OutwardEnemiesBalancer.prefix} {message}");
        }

        public static string GetProjectLocation()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        [HarmonyPatch(typeof(ResourcesPrefabManager), nameof(ResourcesPrefabManager.Load))]
        public class ResourcesPrefabManager_Load
        {
            static void Postfix(ResourcesPrefabManager __instance)
            {
#if DEBUG
                LogSL("ResourcesPrefabManager@Load called!");
#endif
                BalancingRulesSerializer.Instance.LoadPlayerBalanceRules();
                BalancingRulesSerializer.Instance.LoadFactionRules(PathsManager.DefaultBalanceRulesPath);
            }
        }
    }
}
