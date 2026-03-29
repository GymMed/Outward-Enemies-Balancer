using OutwardEnemiesBalancer.Balancing;
using OutwardEnemiesBalancer.Balancing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OutwardEnemiesBalancer.Managers
{
    public class CharacterBalancerManager
    {
        private static CharacterBalancerManager _instance;

        private CharacterBalancerManager()
        {
        }

        public static CharacterBalancerManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CharacterBalancerManager();

                return _instance;
            }
        }

        public void ApplyBalancingRules()
        {
            CharacterAI[] aiArray = UnityEngine.Object.FindObjectsOfType<CharacterAI>();

#if DEBUG
            int totalAI = aiArray.Length;
            int validCharacters = 0;
            int aliveCharacters = 0;
            int charactersWithRules = 0;
            int totalModifications = 0;
#endif

            foreach (CharacterAI ai in aiArray)
            {
                Character character = ai.Character;

#if DEBUG
                if (character != null)
                    validCharacters++;
#endif

                if (character == null)
                    continue;

                if (!character.Alive)
                    continue;

#if DEBUG
                aliveCharacters++;
#endif

                var matchingRules = BalancingRuleRegistryManager.Instance.GetMatchingRules(character);

                if (matchingRules.Count > 0)
                {
#if DEBUG
                    charactersWithRules++;
#endif

                    foreach (var rule in matchingRules)
                    {
                        int modCount = ApplyRuleToCharacter(character, rule);
#if DEBUG
                        totalModifications += modCount;
#endif
                    }
                }
            }

#if DEBUG
            Debug.Log($"[Balancing] AI Found: {totalAI} | Valid Chars: {validCharacters} | Alive: {aliveCharacters} | With Rules: {charactersWithRules} | Mods Applied: {totalModifications}");
#endif
        }

        public void ApplyBalancingRule(BalancingRule rule)
        {
            CharacterAI[] aiArray = UnityEngine.Object.FindObjectsOfType<CharacterAI>();

#if DEBUG
            int totalAI = aiArray.Length;
            int validCharacters = 0;
            int aliveCharacters = 0;
            int matchedCharacters = 0;
            int totalModifications = 0;
#endif

            foreach (CharacterAI ai in aiArray)
            {
                Character character = ai.Character;

#if DEBUG
                if (character != null)
                    validCharacters++;
#endif

                if (character == null)
                    continue;

                if (!character.Alive)
                    continue;

#if DEBUG
                aliveCharacters++;
#endif

                if (!rule.Matches(character))
                    continue;

#if DEBUG
                matchedCharacters++;
#endif

                int modCount = ApplyRuleToCharacter(character, rule);
#if DEBUG
                totalModifications += modCount;
#endif
            }

#if DEBUG
            Debug.Log($"[Balancing-Single] Rule: {rule.id} | AI Found: {totalAI} | Valid Chars: {validCharacters} | Alive: {aliveCharacters} | Matched: {matchedCharacters} | Mods Applied: {totalModifications}");
#endif
        }

        private int ApplyRuleToCharacter(Character character, BalancingRule rule)
        {
            if (character.Stats == null || rule.statModifications == null || rule.statModifications.Count == 0)
                return 0;

            var modifications = StatModificationBuilder.BuildFromDictionary(rule.statModifications, rule.modifierType);
            int count = 0;

            foreach (var mod in modifications)
            {
                ApplyStatModification(character, mod);
                count++;
            }

            return count;
        }

        private void ApplyStatModification(Character character, StatModification mod)
        {
            float currentValue = GetStatValue(character.Stats, mod.StatType);
            float newValue = ApplyModification(currentValue, mod);

            if (mod.MinClamp.HasValue)
                newValue = Mathf.Max(newValue, mod.MinClamp.Value);

            if (mod.MaxClamp.HasValue)
                newValue = Mathf.Min(newValue, mod.MaxClamp.Value);

            SetStatValue(character, mod.StatType, newValue);

#if DEBUG
            float actualValue = GetStatValue(character.Stats, mod.StatType);
            Debug.Log($"[Balancing-Debug] {character.Name} | {mod.StatType} | Current: {currentValue:F2} | New: {newValue:F2} | Actual: {actualValue:F2}");
#endif
        }

        private float ApplyModification(float currentValue, StatModification mod)
        {
            return mod.ModifierType switch
            {
                ValueModifierType.Direct => mod.Value,
                ValueModifierType.Scale => currentValue * mod.Value,
                ValueModifierType.Add => currentValue + mod.Value,
                _ => currentValue
            };
        }

        private CharacterStats.StatType? MapToGameStatType(EnemyBalanceStatType statType)
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

        private Stat GetGameStat(CharacterStats stats, EnemyBalanceStatType statType)
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

        private float GetStatValue(CharacterStats stats, EnemyBalanceStatType statType)
        {
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

        private void SetStatValue(Character character, EnemyBalanceStatType statType, float value)
        {
            CharacterStats stats = character.Stats;

            switch (statType)
            {
                case EnemyBalanceStatType.MaxHealth:
                    float clampedHealth = Mathf.Max(value, 1f);
                    float oldHealth = character.Health;
                    float healthRatio = stats.BaseMaxHealth > 0 ? oldHealth / stats.BaseMaxHealth : 1f;
                    healthRatio = Mathf.Clamp(healthRatio, 0f, 1f);
                    stats.RefreshVitalMaxStat(false);
                    stats.m_maxHealthStat.BaseValue = clampedHealth;
                    stats.RefreshVitalMaxStat(false);
                    stats.SetHealth(healthRatio * clampedHealth);
                    break;

                case EnemyBalanceStatType.MaxStamina:
                    float clampedStamina = Mathf.Max(value, 1f);
                    float oldStamina = character.Stamina;
                    float staminaRatio = stats.MaxStamina > 0 ? oldStamina / stats.MaxStamina : 1f;
                    staminaRatio = Mathf.Clamp(staminaRatio, 0f, 1f);
                    stats.RefreshVitalMaxStat(false);
                    stats.m_maxStamina.BaseValue = clampedStamina;
                    stats.RefreshVitalMaxStat(false);
                    float newStamina = staminaRatio * clampedStamina;
                    float staminaDelta = newStamina - oldStamina;
                    if (Mathf.Abs(staminaDelta) > 0.01f)
                    {
                        stats.AffectStamina(staminaDelta);
                    }
                    break;

                case EnemyBalanceStatType.MaxMana:
                    float clampedMana = Mathf.Max(value, 1f);
                    float oldMana = stats.CurrentMana;
                    float manaRatio = stats.BaseMaxMana > 0 ? oldMana / stats.BaseMaxMana : 1f;
                    manaRatio = Mathf.Clamp(manaRatio, 0f, 1f);
                    stats.RefreshVitalMaxStat(false);
                    stats.m_maxManaStat.BaseValue = clampedMana;
                    stats.RefreshVitalMaxStat(false);
                    stats.SetMana(manaRatio * clampedMana);
                    break;

                case EnemyBalanceStatType.HealthRegen:
                case EnemyBalanceStatType.StaminaRegen:
                case EnemyBalanceStatType.ManaRegen:
                case EnemyBalanceStatType.Impact:
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
            }

            if (statType == EnemyBalanceStatType.HealthRegen ||
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
                statType == EnemyBalanceStatType.FireProtection)
            {
                stats.UpdateStats();
            }
        }
    }
}
