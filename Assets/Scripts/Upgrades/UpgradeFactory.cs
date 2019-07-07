public class UpgradeFactory
{
    public static Upgrade CreateUpgrade(Upgrade.Type upgradeType)
    {
        switch (upgradeType)
        {
            default:
            case Upgrade.Type.DefensePower:
                return new DefensePowerUpgrade();
            case Upgrade.Type.DefenseRange:
                return new DefenseRangeUpgrade();
        }
    }
}
