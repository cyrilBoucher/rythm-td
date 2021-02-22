public abstract class Upgrade
{
    public enum Type
    {
        DefensePower,
        DefenseRange,
        DefenseCooldown
    }

    public int price { get; protected set; }
    public string name { get; protected set; }
    public Type type { get; protected set; }
    public int level { get; protected set; }

    public bool acquired { get; protected set; }

    protected int _startPrice;
    protected int _maxLevel;

    private Upgrade() { }
    public Upgrade(int startPrice, string name, Type type, int maxLevel)
    {
        level = 1;
        _startPrice = startPrice;
        _maxLevel = maxLevel;
        price = ComputePriceFromLevel();
        this.name = name;
        this.type = type;
    }

    public void Init(int level)
    {
        this.level = level;
        if (this.level == _maxLevel)
        {
            acquired = true;

            return;
        }

        IncreaseLevel();
    }

    public void AcquireLevel()
    {
        if (acquired)
        {
            return;
        }

        if (level == _maxLevel)
        {
            acquired = true;

            return;
        }

        IncreaseLevel();
    }

    protected void IncreaseLevel()
    {
        level++;

        price = ComputePriceFromLevel();
    }

    protected abstract int ComputePriceFromLevel();
}
