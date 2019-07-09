using System;
using System.Collections.Generic;

public class UpgradesController
{
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
        upgrade.bought = true;

        if (upgrade is DefenseUpgrade)
        {
            defenseUpgrades.Add((DefenseUpgrade)upgrade);
        }
    }
}
