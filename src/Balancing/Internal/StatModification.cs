namespace OutwardEnemiesBalancer.Balancing
{
    internal class StatModification
    {
        public EnemyBalanceStatType StatType { get; set; }
        public ValueModifierType ModifierType { get; set; }
        public float Value { get; set; }
        public float? MinClamp { get; set; }
        public float? MaxClamp { get; set; }

        public StatModification() { }

        public StatModification(EnemyBalanceStatType statType, ValueModifierType modifierType, float value, float? minClamp = null, float? maxClamp = null)
        {
            StatType = statType;
            ModifierType = modifierType;
            Value = value;
            MinClamp = minClamp;
            MaxClamp = maxClamp;
        }
    }
}
