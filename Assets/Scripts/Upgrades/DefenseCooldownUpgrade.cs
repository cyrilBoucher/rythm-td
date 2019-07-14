public class DefenseCooldownUpgrade : DefenseUpgrade
{
    public DefenseCooldownUpgrade() :
        base(60, "Defense cooldown", Upgrade.Type.DefenseCooldown, 2)
    {
    }

    public override bool Apply(DefenseController defenseController)
    {
        if (defenseController == null)
        {
            return false;
        }

        defenseController.attackCooldownBeat--;

        return true;
    }

    protected override int ComputePriceFromLevel()
    {
        return _startPrice * level;
    }
}
