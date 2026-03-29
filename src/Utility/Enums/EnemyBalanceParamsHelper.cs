using OutwardEnemiesBalancer.Balancing;
using System;
using System.Collections.Generic;

namespace OutwardEnemiesBalancer.Utility.Enums
{
    public static class EnemyBalanceParamsHelper
    {
        private static readonly Dictionary<EnemyBalanceParams, (string key, Type type, string description)> _registry
            = new()
            {
                [EnemyBalanceParams.BalanceRuleId] = ("balanceRuleId", typeof(string), "Optional. Unique identifier for the balancing rule."),
                [EnemyBalanceParams.EnemyId] = ("enemyId", typeof(string), "Optional. Specific enemy UID to target."),
                [EnemyBalanceParams.EnemyName] = ("enemyName", typeof(string), "Optional. Enemy display name to target."),
                [EnemyBalanceParams.AreaEnum] = ("area", typeof(AreaManager.AreaEnum?), "Optional. Specific area filter."),
                [EnemyBalanceParams.AreaFamily] = ("areaFamily", typeof(AreaFamily), "Optional. Area family (region) filter."),
                [EnemyBalanceParams.Faction] = ("faction", typeof(Character.Factions?), "Optional. Faction filter."),
                [EnemyBalanceParams.IsForBosses] = ("isForBosses", typeof(bool), "Optional. Apply to all bosses."),
                [EnemyBalanceParams.IsForBossesPawns] = ("isForBossPawns", typeof(bool), "Optional. Apply to boss pawns."),
                [EnemyBalanceParams.IsForStoryBosses] = ("isForStoryBosses", typeof(bool), "Optional. Apply to story bosses."),
                [EnemyBalanceParams.IsForUniqueArenaBosses] = ("isForUniqueArenaBosses", typeof(bool), "Optional. Apply to unique arena bosses."),
                [EnemyBalanceParams.IsForUniqueEnemies] = ("isForUniqueEnemies", typeof(bool), "Optional. Apply to unique enemies."),
                [EnemyBalanceParams.ExceptIds] = ("listExceptIds", typeof(List<string>), "Optional. List of enemy UIDs to exclude."),
                [EnemyBalanceParams.ExceptNames] = ("listExceptNames", typeof(List<string>), "Optional. List of enemy names to exclude."),
                [EnemyBalanceParams.StatModifications] = ("statModifications", typeof(Dictionary<string, float?>), "Optional. Dictionary of stat name to value. Use with modifierType for scaling."),
                [EnemyBalanceParams.ModifierType] = ("modifierType", typeof(ValueModifierType), "Optional. Default Scale. How to modify: Direct, Scale, Add."),
                [EnemyBalanceParams.StatType] = ("statType", typeof(string), "Optional. Stat name (string) to modify. Use with value parameter."),
                [EnemyBalanceParams.Value] = ("value", typeof(float?), "Optional. Modification value for single stat."),
                [EnemyBalanceParams.MinClamp] = ("minClamp", typeof(float?), "Optional. Minimum clamp for the result."),
                [EnemyBalanceParams.MaxClamp] = ("maxClamp", typeof(float?), "Optional. Maximum clamp for the result."),
                [EnemyBalanceParams.MaxHealth] = ("maxHealth", typeof(float?), "Optional. Max health modification."),
                [EnemyBalanceParams.MaxStamina] = ("maxStamina", typeof(float?), "Optional. Max stamina modification."),
                [EnemyBalanceParams.MaxMana] = ("maxMana", typeof(float?), "Optional. Max mana modification."),
                [EnemyBalanceParams.HealthRegen] = ("healthRegen", typeof(float?), "Optional. Health regen modification."),
                [EnemyBalanceParams.StaminaRegen] = ("staminaRegen", typeof(float?), "Optional. Stamina regen modification."),
                [EnemyBalanceParams.ManaRegen] = ("manaRegen", typeof(float?), "Optional. Mana regen modification."),
                [EnemyBalanceParams.DamageType] = ("damageType", typeof(DamageType.Types), "Optional. Damage type for grouped damage event."),
                [EnemyBalanceParams.DamageValue] = ("damageValue", typeof(float?), "Optional. Damage value for grouped damage event."),
                [EnemyBalanceParams.ResistanceValue] = ("resistanceValue", typeof(float?), "Optional. Resistance value for grouped damage event."),
                [EnemyBalanceParams.ProtectionValue] = ("protectionValue", typeof(float?), "Optional. Protection value for grouped damage event."),
                [EnemyBalanceParams.ColdProtection] = ("coldProtection", typeof(float?), "Optional. Cold protection modification."),
                [EnemyBalanceParams.HeatProtection] = ("heatProtection", typeof(float?), "Optional. Heat protection modification."),
                [EnemyBalanceParams.CorruptionResistance] = ("corruptionResistance", typeof(float?), "Optional. Corruption resistance modification."),
                [EnemyBalanceParams.Waterproof] = ("waterproof", typeof(float?), "Optional. Waterproof modification."),
                [EnemyBalanceParams.Impact] = ("impact", typeof(float?), "Optional. Impact damage modification."),
                [EnemyBalanceParams.ImpactResistance] = ("impactResistance", typeof(float?), "Optional. Impact resistance modification."),
                [EnemyBalanceParams.MovementSpeed] = ("movementSpeed", typeof(float?), "Optional. Movement speed modification."),
                [EnemyBalanceParams.AttackSpeed] = ("attackSpeed", typeof(float?), "Optional. Attack speed modification."),
                [EnemyBalanceParams.LoadBalanceRulesXmlPath] = ("filePath", typeof(string), "Required. Path to load balance rules XML."),
                [EnemyBalanceParams.StoreBalanceRulesXmlPath] = ("filePath", typeof(string), "Optional. Path to save balance rules XML."),
                [EnemyBalanceParams.NewFaction] = ("newFaction", typeof(Character.Factions), "Required. The faction to change enemies to."),
            };

        public static (string key, Type type, string description) Get(EnemyBalanceParams param) => _registry[param];

        public static (string key, Type type, string description)[] Combine(params object[] items)
        {
            var list = new List<(string key, Type type, string description)>();

            foreach (var item in items)
            {
                if (item is ValueTuple<string, Type, string> single)
                {
                    list.Add(single);
                }
                else if (item is ValueTuple<string, Type, string>[] array)
                {
                    list.AddRange(array);
                }
                else
                {
                    throw new ArgumentException($"Unsupported item type: {item?.GetType().FullName}");
                }
            }

            return list.ToArray();
        }
    }
}
