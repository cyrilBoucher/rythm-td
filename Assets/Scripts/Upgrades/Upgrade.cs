public abstract class Upgrade
{
    public enum Type
    {
        DefensePower,
        DefenseRange
    }

    public int price;
    public string name;
    public Type type;
    public bool bought = false;
}
