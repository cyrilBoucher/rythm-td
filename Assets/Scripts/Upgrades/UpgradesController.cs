using System.Collections.Generic;

public class UpgradesController
{
    public static readonly List<DefenseUpgrade> defenseUpgrades = new List<DefenseUpgrade>();

    public static void AddUpgrade(Upgrade upgrade)
    {
        if (upgrade is DefenseUpgrade)
        {
            defenseUpgrades.Add((DefenseUpgrade)upgrade);
        }
    }

    public static void ApplyDefenseUpgrades(DefenseController defenseController)
    {
        foreach (DefenseUpgrade defenseUpgrade in defenseUpgrades)
        {
            defenseUpgrade.Apply(defenseController);
        }
    }
}
