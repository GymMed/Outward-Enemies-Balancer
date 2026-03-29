#if DEBUG
using OutwardEnemiesBalancer.Balancing;
using OutwardEnemiesBalancer.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OutwardEnemiesBalancer.Events.Tests
{
    public static class BalancingTestValidator
    {
        private static Dictionary<Character, Dictionary<EnemyBalanceStatType, float>> _originalStats = new();

        public static void StoreOriginalStats(Character character, params EnemyBalanceStatType[] stats)
        {
            if (!_originalStats.ContainsKey(character))
                _originalStats[character] = new Dictionary<EnemyBalanceStatType, float>();

            foreach (var stat in stats)
            {
                float value = GetStatValue(character, stat);
                _originalStats[character][stat] = value;
            }
        }

        public static void StoreOriginalStat(Character character, EnemyBalanceStatType stat)
        {
            if (!_originalStats.ContainsKey(character))
                _originalStats[character] = new Dictionary<EnemyBalanceStatType, float>();

            _originalStats[character][stat] = GetStatValue(character, stat);
        }

        public static float GetStatValue(Character character, EnemyBalanceStatType statType)
        {
            var stats = character.Stats;
            return statType switch
            {
                EnemyBalanceStatType.MaxHealth => stats.BaseMaxHealth,
                EnemyBalanceStatType.MaxStamina => stats.MaxStamina,
                EnemyBalanceStatType.MaxMana => stats.BaseMaxMana,
                EnemyBalanceStatType.HealthRegen => stats.HealthRegen,
                EnemyBalanceStatType.StaminaRegen => stats.StaminaRegen,
                EnemyBalanceStatType.ManaRegen => stats.ManaRegen,
                EnemyBalanceStatType.Impact => stats.m_impactModifier.CurrentValue,
                EnemyBalanceStatType.ImpactResistance => stats.GetImpactResistance(),
                EnemyBalanceStatType.MovementSpeed => stats.MovementSpeed,

                EnemyBalanceStatType.PhysicalDamage => stats.m_damageTypesModifier[0].CurrentValue,
                EnemyBalanceStatType.EtherealDamage => stats.m_damageTypesModifier[1].CurrentValue,
                EnemyBalanceStatType.DecayDamage => stats.m_damageTypesModifier[2].CurrentValue,
                EnemyBalanceStatType.ElectricDamage => stats.m_damageTypesModifier[3].CurrentValue,
                EnemyBalanceStatType.FrostDamage => stats.m_damageTypesModifier[4].CurrentValue,
                EnemyBalanceStatType.FireDamage => stats.m_damageTypesModifier[5].CurrentValue,

                EnemyBalanceStatType.PhysicalResistance => stats.m_damageResistance[0].CurrentValue,
                EnemyBalanceStatType.EtherealResistance => stats.m_damageResistance[1].CurrentValue,
                EnemyBalanceStatType.DecayResistance => stats.m_damageResistance[2].CurrentValue,
                EnemyBalanceStatType.ElectricResistance => stats.m_damageResistance[3].CurrentValue,
                EnemyBalanceStatType.FrostResistance => stats.m_damageResistance[4].CurrentValue,
                EnemyBalanceStatType.FireResistance => stats.m_damageResistance[5].CurrentValue,

                EnemyBalanceStatType.PhysicalProtection => stats.m_damageProtection[0].CurrentValue,
                EnemyBalanceStatType.EtherealProtection => stats.m_damageProtection[1].CurrentValue,
                EnemyBalanceStatType.DecayProtection => stats.m_damageProtection[2].CurrentValue,
                EnemyBalanceStatType.ElectricProtection => stats.m_damageProtection[3].CurrentValue,
                EnemyBalanceStatType.FrostProtection => stats.m_damageProtection[4].CurrentValue,
                EnemyBalanceStatType.FireProtection => stats.m_damageProtection[5].CurrentValue,

                _ => GetGameStat(stats, statType)?.CurrentValue ?? 0f
            };
        }

        public static void RestoreOriginalStats(Character character)
        {
            if (!_originalStats.TryGetValue(character, out var stats))
                return;

            foreach (var kvp in stats)
            {
                RestoreStatValue(character, kvp.Key, kvp.Value);
            }
        }

        public static void ClearOriginalStats(Character character)
        {
            _originalStats.Remove(character);
        }

        private static CharacterStats.StatType? MapToGameStatType(EnemyBalanceStatType statType)
        {
            return statType switch
            {
                EnemyBalanceStatType.MaxHealth => CharacterStats.StatType.MaxHealth,
                EnemyBalanceStatType.MaxStamina => CharacterStats.StatType.MaxStamina,
                EnemyBalanceStatType.MaxMana => CharacterStats.StatType.MaxMana,
                EnemyBalanceStatType.HealthRegen => CharacterStats.StatType.HealthRegen,
                EnemyBalanceStatType.StaminaRegen => CharacterStats.StatType.StaminaRegen,
                EnemyBalanceStatType.ManaRegen => CharacterStats.StatType.ManaRegen,
                EnemyBalanceStatType.Impact => CharacterStats.StatType.Impact,
                EnemyBalanceStatType.ImpactResistance => CharacterStats.StatType.ImpactResistance,
                EnemyBalanceStatType.MovementSpeed => CharacterStats.StatType.MovementSpeed,
                EnemyBalanceStatType.AttackSpeed => CharacterStats.StatType.AttackSpeed,
                EnemyBalanceStatType.SkillCooldownModifier => CharacterStats.StatType.SkillCooldownModifier,
                EnemyBalanceStatType.DodgeInvulnerabilityModifier => CharacterStats.StatType.DodgeInvulnerabilityModifier,
                EnemyBalanceStatType.GlobalStatusResistance => CharacterStats.StatType.StatusEffectBuildUpResistance,
                EnemyBalanceStatType.ColdProtection => CharacterStats.StatType.EnvColdProtection,
                EnemyBalanceStatType.HeatProtection => CharacterStats.StatType.EnvHeatProtection,
                EnemyBalanceStatType.ColdRegenRate => CharacterStats.StatType.ColdRegen,
                EnemyBalanceStatType.HeatRegenRate => CharacterStats.StatType.HeatRegen,
                EnemyBalanceStatType.CorruptionProtection => CharacterStats.StatType.CorruptionResistance,
                EnemyBalanceStatType.Waterproof => CharacterStats.StatType.Waterproof,
                _ => null
            };
        }

        private static MethodInfo _getStatMethod;

        private static Stat GetGameStat(CharacterStats stats, EnemyBalanceStatType statType)
        {
            CharacterStats.StatType? gameStatType = MapToGameStatType(statType);
            if (!gameStatType.HasValue)
                return null;

            if (_getStatMethod == null)
            {
                var methods = typeof(CharacterStats).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
                _getStatMethod = methods.FirstOrDefault(m => m.Name == "GetStat" &&
                                    m.GetParameters().Length == 1 &&
                                    m.GetParameters()[0].ParameterType == typeof(CharacterStats.StatType));
            }

            if (_getStatMethod != null)
            {
                return (Stat)_getStatMethod.Invoke(stats, new object[] { gameStatType.Value });
            }

            return null;
        }

        private static void RestoreStatValue(Character character, EnemyBalanceStatType statType, float value)
        {
            var stats = character.Stats;

            switch (statType)
            {
                case EnemyBalanceStatType.MaxHealth:
                    stats.RefreshVitalMaxStat(false);
                    stats.m_maxHealthStat.BaseValue = value;
                    stats.RefreshVitalMaxStat(false);
                    stats.SetHealth(value);
                    break;

                case EnemyBalanceStatType.MaxStamina:
                    stats.RefreshVitalMaxStat(false);
                    stats.m_maxStamina.BaseValue = value;
                    stats.RefreshVitalMaxStat(false);
                    break;

                case EnemyBalanceStatType.MaxMana:
                    stats.RefreshVitalMaxStat(false);
                    stats.m_maxManaStat.BaseValue = value;
                    stats.RefreshVitalMaxStat(false);
                    break;

                case EnemyBalanceStatType.HealthRegen:
                case EnemyBalanceStatType.StaminaRegen:
                case EnemyBalanceStatType.ManaRegen:
                case EnemyBalanceStatType.MovementSpeed:
                case EnemyBalanceStatType.AttackSpeed:
                case EnemyBalanceStatType.SkillCooldownModifier:
                case EnemyBalanceStatType.DodgeInvulnerabilityModifier:
                case EnemyBalanceStatType.GlobalStatusResistance:
                case EnemyBalanceStatType.ColdProtection:
                case EnemyBalanceStatType.HeatProtection:
                case EnemyBalanceStatType.ColdRegenRate:
                case EnemyBalanceStatType.HeatRegenRate:
                case EnemyBalanceStatType.CorruptionProtection:
                case EnemyBalanceStatType.Waterproof:
                case EnemyBalanceStatType.ImpactResistance:
                case EnemyBalanceStatType.Impact:
                    Stat gameStat = GetGameStat(stats, statType);
                    if (gameStat != null)
                    {
                        gameStat.BaseValue = value;
                        gameStat.Update();
                    }
                    break;

                case EnemyBalanceStatType.PhysicalDamage:
                    stats.m_damageTypesModifier[0].BaseValue = value;
                    stats.m_damageTypesModifier[0].Update();
                    break;

                case EnemyBalanceStatType.EtherealDamage:
                    stats.m_damageTypesModifier[1].BaseValue = value;
                    stats.m_damageTypesModifier[1].Update();
                    break;

                case EnemyBalanceStatType.DecayDamage:
                    stats.m_damageTypesModifier[2].BaseValue = value;
                    stats.m_damageTypesModifier[2].Update();
                    break;

                case EnemyBalanceStatType.ElectricDamage:
                    stats.m_damageTypesModifier[3].BaseValue = value;
                    stats.m_damageTypesModifier[3].Update();
                    break;

                case EnemyBalanceStatType.FrostDamage:
                    stats.m_damageTypesModifier[4].BaseValue = value;
                    stats.m_damageTypesModifier[4].Update();
                    break;

                case EnemyBalanceStatType.FireDamage:
                    stats.m_damageTypesModifier[5].BaseValue = value;
                    stats.m_damageTypesModifier[5].Update();
                    break;

                case EnemyBalanceStatType.PhysicalResistance:
                    stats.m_damageResistance[0].BaseValue = value;
                    stats.m_damageResistance[0].Update();
                    break;

                case EnemyBalanceStatType.EtherealResistance:
                    stats.m_damageResistance[1].BaseValue = value;
                    stats.m_damageResistance[1].Update();
                    break;

                case EnemyBalanceStatType.DecayResistance:
                    stats.m_damageResistance[2].BaseValue = value;
                    stats.m_damageResistance[2].Update();
                    break;

                case EnemyBalanceStatType.ElectricResistance:
                    stats.m_damageResistance[3].BaseValue = value;
                    stats.m_damageResistance[3].Update();
                    break;

                case EnemyBalanceStatType.FrostResistance:
                    stats.m_damageResistance[4].BaseValue = value;
                    stats.m_damageResistance[4].Update();
                    break;

                case EnemyBalanceStatType.FireResistance:
                    stats.m_damageResistance[5].BaseValue = value;
                    stats.m_damageResistance[5].Update();
                    break;

                case EnemyBalanceStatType.PhysicalProtection:
                    stats.m_damageProtection[0].BaseValue = value;
                    stats.m_damageProtection[0].Update();
                    break;

                case EnemyBalanceStatType.EtherealProtection:
                    stats.m_damageProtection[1].BaseValue = value;
                    stats.m_damageProtection[1].Update();
                    break;

                case EnemyBalanceStatType.DecayProtection:
                    stats.m_damageProtection[2].BaseValue = value;
                    stats.m_damageProtection[2].Update();
                    break;

                case EnemyBalanceStatType.ElectricProtection:
                    stats.m_damageProtection[3].BaseValue = value;
                    stats.m_damageProtection[3].Update();
                    break;

                case EnemyBalanceStatType.FrostProtection:
                    stats.m_damageProtection[4].BaseValue = value;
                    stats.m_damageProtection[4].Update();
                    break;

                case EnemyBalanceStatType.FireProtection:
                    stats.m_damageProtection[5].BaseValue = value;
                    stats.m_damageProtection[5].Update();
                    break;

                default:
                    Debug.Log($"[BalancingTests] RestoreStatValue: {statType} not handled, skipping restore");
                    break;
            }

            if (statType == EnemyBalanceStatType.PhysicalResistance ||
                statType == EnemyBalanceStatType.EtherealResistance ||
                statType == EnemyBalanceStatType.DecayResistance ||
                statType == EnemyBalanceStatType.ElectricResistance ||
                statType == EnemyBalanceStatType.FrostResistance ||
                statType == EnemyBalanceStatType.FireResistance ||
                statType == EnemyBalanceStatType.PhysicalProtection ||
                statType == EnemyBalanceStatType.EtherealProtection ||
                statType == EnemyBalanceStatType.DecayProtection ||
                statType == EnemyBalanceStatType.ElectricProtection ||
                statType == EnemyBalanceStatType.FrostProtection ||
                statType == EnemyBalanceStatType.FireProtection ||
                statType == EnemyBalanceStatType.HealthRegen ||
                statType == EnemyBalanceStatType.StaminaRegen ||
                statType == EnemyBalanceStatType.ManaRegen ||
                statType == EnemyBalanceStatType.ColdProtection ||
                statType == EnemyBalanceStatType.HeatProtection ||
                statType == EnemyBalanceStatType.ColdRegenRate ||
                statType == EnemyBalanceStatType.HeatRegenRate ||
                statType == EnemyBalanceStatType.CorruptionProtection ||
                statType == EnemyBalanceStatType.Waterproof ||
                statType == EnemyBalanceStatType.ImpactResistance)
            {
                stats.UpdateStats();
            }
        }

        public static bool Validate(string testName, bool condition, string details = "")
        {
            if (condition)
            {
                Debug.Log($"[BalancingTests] PASS: {testName}");
            }
            else
            {
                Debug.LogError($"[BalancingTests] FAIL: {testName} - {details}");
            }
            return condition;
        }

        public static bool ValidateFloat(string testName, float actual, float expected, float tolerance = 0.01f)
        {
            bool passed = Mathf.Abs(actual - expected) <= tolerance;
            string details = $"Expected: {expected:F2}, Actual: {actual:F2}";
            return Validate(testName, passed, details);
        }

        public static bool ValidateModifierResult(float original, float modifierValue, ValueModifierType modType, float expectedResult)
        {
            float actual = modType switch
            {
                ValueModifierType.Direct => modifierValue,
                ValueModifierType.Scale => original * modifierValue,
                ValueModifierType.Add => original + modifierValue,
                _ => original
            };

            return ValidateFloat($"ModifierType.{modType}", actual, expectedResult);
        }
    }
}
#endif
