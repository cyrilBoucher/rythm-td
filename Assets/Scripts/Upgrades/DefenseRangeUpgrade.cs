public class DefenseRangeUpgrade : DefenseUpgrade
{
    public DefenseRangeUpgrade() :
        base(startPrice: 50, name: "Defense range", type: Type.DefenseRange, maxLevel: 3)
    {
    }

    public override bool Apply(DefenseController defenseController)
    {
        if (defenseController == null)
        {
            return false;
        }

        defenseController.IncreaseRange((float)level);

        return true;
    }

    protected override int ComputePriceFromLevel()
    {
        return _startPrice * level;
    }
}
