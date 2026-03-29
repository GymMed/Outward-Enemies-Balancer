using OutwardEnemiesBalancer.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardEnemiesBalancer.Utility.Enums
{
    public enum BossCategories
    {
        Story,
        Arena,
        Pawn
    }

    public struct BossID
    {
        public BossCategories Category;
        public EnemyIdentificationGroupData EnemyData;
        public Enum EnumValue;

        public BossID(BossCategories category, EnemyIdentificationGroupData data, Enum enumValue = null)
        {
            Category = category;
            EnemyData = data;
            EnumValue = enumValue;
        }

        public override string ToString()
        {
            string enemiesData = "";

            foreach (EnemyIdentificationData enemy in EnemyData.Enemies)
            {
                enemiesData += $"Name {enemy.DisplayName} Id {enemy.ID} enum {EnumValue.ToString()}";
            }

            return $"{Category}:{enemiesData}";
        }
    }
}
