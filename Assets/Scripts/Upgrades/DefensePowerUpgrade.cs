public class DefensePowerUpgrade : DefenseUpgrade
{
    int level = 1;

    public DefensePowerUpgrade()
    {
        price = level * 100;
        name = "Defense power";
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
}
