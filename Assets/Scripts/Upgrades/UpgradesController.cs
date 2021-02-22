using System;
using System.Collections.Generic;

public class UpgradesController
{
    public delegate void UpgradeModifiedAction(Upgrade upgrade);
    public static event UpgradeModifiedAction UpgradeModified;

    private static Dictionary<Upgrade.Type, Upgrade> _availableUpgrades = new Dictionary<Upgrade.Type, Upgrade>();

    public static readonly List<DefenseUpgrade> defenseUpgrades = new List<DefenseUpgrade>();

    private static bool _initialized = false;

    public static void Initialize()
    {
        if (_initialized)
        {
            return;
        }

        foreach (Upgrade.Type type in Enum.GetValues(typeof(Upgrade.Type)))
        {
            _availableUpgrades.Add(type, UpgradeFactory.CreateUpgrade(type));
        }

        foreach (KeyValuePair<Upgrade.Type, int> upgradeLevelByUpgradeType in Player.Instance.Upgrades)
        {
            _availableUpgrades[upgradeLevelByUpgradeType.Key].Init(upgradeLevelByUpgradeType.Value);
        }

        _initialized = true;
    }

    public static void ApplyDefenseUpgrades(DefenseController defenseController)
    {
        foreach (DefenseUpgrade defenseUpgrade in defenseUpgrades)
        {
            defenseUpgrade.Apply(defenseController);
        }
    }

    public static Upgrade GetUpgradeFromType(Upgrade.Type upgradeType)
    {
        if (!_availableUpgrades.ContainsKey(upgradeType))
        {
            throw new ArgumentException(string.Format("There is no such upgrade with the type {0}", upgradeType));
        }

        return _availableUpgrades[upgradeType];
    }

    public static void BuyUpgrade(Upgrade.Type upgradeType)
    {
        if (!_availableUpgrades.ContainsKey(upgradeType))
        {
            throw new ArgumentException(string.Format("There is no such upgrade with the type {0}", upgradeType));
        }

        Upgrade upgrade = _availableUpgrades[upgradeType];

        if (upgrade.acquired)
        {
            throw new UpgradeAlreadyAcquiredException(string.Format("Upgrade of type {0} was already acquired", upgrade.type));
        }

        Player.Instance.TakeSkillPoints(upgrade.price);

        Player.Instance.BuyUpgrade(upgrade.type, upgrade.level);

        upgrade.AcquireLevel();

        if (upgrade is DefenseUpgrade)
        {
            DefenseUpgrade defenseUpgrade = (DefenseUpgrade)upgrade;
            if (!defenseUpgrades.Contains(defenseUpgrade))
            {
                defenseUpgrades.Add(defenseUpgrade);
            }
        }

        UpgradeModified?.Invoke(upgrade);
    }
}
