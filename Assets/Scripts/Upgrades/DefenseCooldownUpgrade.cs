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

        defenseController.attackCooldownBeat -= level;

        if(defenseController.attackCooldownBeat < 1)
        {
            defenseController.attackCooldownBeat = 1;
        }

        return true;
    }

    protected override int ComputePriceFromLevel()
    {
        return _startPrice * level;
    }
}
