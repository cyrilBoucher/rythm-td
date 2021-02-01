using System;

public class SkillPointsController
{
    public delegate void SkillPointsNumberChangedAction();
    public static event SkillPointsNumberChangedAction SkillPointsNumberChanged;

    public static int skillPoints { get; private set; }

    private static bool _initialized = false;

    public static void Initialize(int skillPointsNumber)
    {
        if (_initialized)
        {
            return;
        }

        SetSkillPointsNumber(skillPointsNumber);

        _initialized = true;
    }

    public static void SetSkillPointsNumber(int skillPointsNumber)
    {
        if (skillPointsNumber < 0)
        {
            throw new ArgumentException("Skill points number cannot be negative");
        }

        skillPoints = skillPointsNumber;

        SkillPointsNumberChanged?.Invoke();
    }

    public static void AddSkillPoints(int skillPointsNumber)
    {
        if (skillPointsNumber < 0)
        {
            throw new ArgumentException("Skill points number cannot be negative");
        }

        skillPoints += skillPointsNumber;

        SkillPointsNumberChanged?.Invoke();
    }

    public static void TakeSkillPoints(int skillPointsNumber)
    {
        if (skillPointsNumber < 0)
        {
            throw new ArgumentException("Skill points number cannot be negative");
        }

        if (skillPointsNumber > skillPoints)
        {
            throw new NotEnoughSkillPointsException(string.Format("Cannot take {0} skill points as there is only {1} remaining", skillPointsNumber, skillPoints));
        }

        skillPoints -= skillPointsNumber;

        SkillPointsNumberChanged?.Invoke();
    }
}
