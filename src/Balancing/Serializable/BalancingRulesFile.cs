using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OutwardEnemiesBalancer.Balancing.Serializable
{
    [XmlRoot("BalanceRules")]
    public class BalancingRulesFile
    {
        [XmlElement("Rule")]
        public List<BalancingRuleSerializable> Rules { get; set; } = new List<BalancingRuleSerializable>();

        [XmlElement("FactionRule")]
        public List<FactionRuleSerializable> FactionRules { get; set; } = new List<FactionRuleSerializable>();
    }

    [Serializable]
    public class BalancingRuleSerializable
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlElement]
        public string EnemyID { get; set; }

        [XmlElement]
        public string EnemyName { get; set; }

        [XmlElement]
        public string AreaFamilyName { get; set; }

        [XmlElement]
        public string AreaName { get; set; }

        [XmlElement]
        public string FactionName { get; set; }

        [XmlElement]
        public bool IsBoss { get; set; }

        [XmlElement]
        public bool IsBossPawn { get; set; }

        [XmlElement]
        public bool IsStoryBoss { get; set; }

        [XmlElement]
        public bool IsUniqueArenaBoss { get; set; }

        [XmlElement]
        public bool IsUniqueEnemy { get; set; }

        [XmlArray("ExceptIds")]
        [XmlArrayItem("Id")]
        public List<string> ExceptIds { get; set; }

        [XmlArray("ExceptNames")]
        [XmlArrayItem("Name")]
        public List<string> ExceptNames { get; set; }

        [XmlArray("StatModifications")]
        [XmlArrayItem("Stat")]
        public List<StatModificationSerializable> StatModifications { get; set; } = new List<StatModificationSerializable>();
    }

    [Serializable]
    public class StatModificationSerializable
    {
        [XmlAttribute]
        public string StatName { get; set; }

        [XmlAttribute]
        public float Value { get; set; }
    }

    [Serializable]
    public class FactionRuleSerializable
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlElement]
        public string EnemyID { get; set; }

        [XmlElement]
        public string EnemyName { get; set; }

        [XmlElement]
        public string AreaFamilyName { get; set; }

        [XmlElement]
        public string AreaName { get; set; }

        [XmlElement]
        public string TargetFactionName { get; set; }

        [XmlElement]
        public string NewFactionName { get; set; }

        [XmlElement]
        public bool IsBoss { get; set; }

        [XmlElement]
        public bool IsBossPawn { get; set; }

        [XmlElement]
        public bool IsStoryBoss { get; set; }

        [XmlElement]
        public bool IsUniqueArenaBoss { get; set; }

        [XmlElement]
        public bool IsUniqueEnemy { get; set; }

        [XmlArray("ExceptIds")]
        [XmlArrayItem("Id")]
        public List<string> ExceptIds { get; set; }

        [XmlArray("ExceptNames")]
        [XmlArrayItem("Name")]
        public List<string> ExceptNames { get; set; }
    }
}
