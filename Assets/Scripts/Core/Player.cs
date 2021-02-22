using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YamlDotNet.Serialization.NamingConventions;

public class Player
{
    public event Action SkillPointsNumberChanged;

    private static string m_PlayerFilePath;
    public static string PlayerFilePath
    {
        get
        {
            if (string.IsNullOrEmpty(m_PlayerFilePath))
            {
                m_PlayerFilePath = Path.Combine(Application.persistentDataPath, "player.txt");
            }

            return m_PlayerFilePath;
        }
    }

    private static Player m_Instance;
    public static Player Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new Player();
            }

            return m_Instance;
        }
    }

    public int SkillPoints { get; set; }

    public Dictionary<Upgrade.Type, int> Upgrades { get; set; }

    public Dictionary<string, LevelData> BeatLevels { get; set; }

    public void LoadOrCreateFile()
    {
        if (!File.Exists(PlayerFilePath))
        {
            SetupDefaults();

            Serialize();

            return;
        }

        Deserialize();
    }

    public void Deserialize()
    {
        StringReader input = new StringReader(File.ReadAllText(PlayerFilePath));

        YamlDotNet.Serialization.Deserializer deserializer = new YamlDotNet.Serialization.DeserializerBuilder().WithNamingConvention(new CamelCaseNamingConvention()).Build();

        m_Instance = deserializer.Deserialize<Player>(input);
    }

    public void Serialize()
    {
        YamlDotNet.Serialization.Serializer serializer = new YamlDotNet.Serialization.SerializerBuilder().EmitDefaults().WithNamingConvention(new CamelCaseNamingConvention()).Build();
        string yamlSerializedPlayer = serializer.Serialize(Instance);

        File.WriteAllText(PlayerFilePath, yamlSerializedPlayer);
    }

    private void SetupDefaults()
    {
        Instance.SkillPoints = 0;

        Instance.BeatLevels = new Dictionary<string, LevelData>();

        Instance.Upgrades = new Dictionary<Upgrade.Type, int>();
    }

    public void AddSkillPoints(int skillPointsNumber)
    {
        if (skillPointsNumber < 0)
        {
            throw new ArgumentException("Skill points number cannot be negative");
        }

        SkillPoints += skillPointsNumber;

        Serialize();

        SkillPointsNumberChanged?.Invoke();
    }

    public void TakeSkillPoints(int skillPointsNumber)
    {
        if (skillPointsNumber < 0)
        {
            throw new ArgumentException("Skill points number cannot be negative");
        }

        if (skillPointsNumber > SkillPoints)
        {
            throw new NotEnoughSkillPointsException(string.Format("Cannot take {0} skill points as there is only {1} remaining", skillPointsNumber, SkillPoints));
        }

        SkillPoints -= skillPointsNumber;
        Serialize();

        SkillPointsNumberChanged?.Invoke();
    }

    public void BuyUpgrade(Upgrade.Type upgradeTypeToBuy, int upgradeLevelToBuy)
    {
        if (upgradeLevelToBuy > 1)
        {
            Upgrades[upgradeTypeToBuy] = upgradeLevelToBuy;

            return;
        }

        Upgrades.Add(upgradeTypeToBuy, upgradeLevelToBuy);

        Serialize();
    }

    public void AddBeatenLevel(string levelName)
    {
        if (BeatLevels.ContainsKey(levelName))
        {
            return;
        }

        BeatLevels.Add(levelName, new LevelData());

        Serialize();
    }

    public void SetBeatenLevelAchievements(string levelName, bool timeAchievementObtained, bool resourcesAchievementObtained)
    {
        if (!BeatLevels.ContainsKey(levelName))
        {
            return;
        }

        if (!BeatLevels[levelName].timeAchievementObtained &&
            timeAchievementObtained)
        {
            BeatLevels[levelName].timeAchievementObtained = true;
        }

        if (!BeatLevels[levelName].resourcesAchievementObtained &&
            resourcesAchievementObtained)
        {
            BeatLevels[levelName].resourcesAchievementObtained = true;
        }

        Serialize();
    }
}
