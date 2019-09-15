public class LevelData
{
    public string levelName { private set; get; }
    public bool locked { private set; get; }
    public bool timeAchievementObtained { private set; get; }
    public bool resourcesAchievementObtained { private set; get; }

    public LevelData(string levelName)
    {
        this.levelName = levelName;
    }
    
}
