public class DefenseRangeUpgrade : DefenseUpgrade
{
    int level = 1;

    public DefenseRangeUpgrade()
    {
        price = level * 50;
        name = "Defense range";
    }

    protected override bool Apply(DefenseController defenseController)
    {
        if (defenseController == null)
        {
            return false;
        }

        defenseController.IncreaseRange((float)level);

        return true;
    }
}
