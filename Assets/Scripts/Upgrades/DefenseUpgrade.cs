public abstract class DefenseUpgrade : Upgrade
{
    public DefenseUpgrade(int startPrice, string name, Type type, int maxLevel) : base(startPrice, name, type, maxLevel)
    {
    }

    public abstract bool Apply(DefenseController defenseController);
}
