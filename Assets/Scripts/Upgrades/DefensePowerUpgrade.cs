public class DefensePowerUpgrade : DefenseUpgrade
{
    public DefensePowerUpgrade() :
        base(startPrice: 1, name: "Defense power", type: Type.DefensePower, maxLevel: 3)
    {
    }

    public override bool Apply(DefenseController defenseController)
    {
        if (defenseController == null)
        {
            return false;
        }

        defenseController.projectilePower += level;

        return true;
    }

    protected override int ComputePriceFromLevel()
    {
        return _startPrice * level;
    }
}
