using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelWonSceneController : MonoBehaviour
{
    public static float timeAchivementMinutes;
    public static float timeToBeatLevelSeconds;

    public static float resourcesUsedAchievement;
    public static float resourcesUsed;

    public Canvas canvas;
    public Text skillPointsEarnedText;
    public Text timeResultText;
    public Text resourcesResultText;

    private readonly Color achievedColor = Color.green;
    private readonly Color failedColor = Color.red;

    void Awake()
    {
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerName = "HUD";

        int skillPointsEarned = EarnSkillPoints();

        skillPointsEarnedText.text = string.Format(skillPointsEarnedText.text, skillPointsEarned);

        int minutes = (int)(timeToBeatLevelSeconds / 60.0f);
        int seconds = (int)(timeToBeatLevelSeconds - (minutes * 60.0f));

        timeResultText.text = string.Format("{0}m {1}s / {2}m", minutes, seconds, timeAchivementMinutes);
        if (timeToBeatLevelSeconds <= (timeAchivementMinutes * 60.0f))
        {
            timeResultText.color = achievedColor;
        }
        else
        {
            timeResultText.color = failedColor;
        }

        resourcesResultText.text = string.Format("{0} / {1}", resourcesUsed, resourcesUsedAchievement);
        if (resourcesUsed <= resourcesUsedAchievement)
        {
            resourcesResultText.color = achievedColor;
        }
        else
        {
            resourcesResultText.color = failedColor;
        }
    }

    int EarnSkillPoints()
    {
        string currentLevel = LevelSelectController.selectedLevelName;

        int skillPointsEarned = 0;

        if (!Player.Instance.BeatLevels.ContainsKey(currentLevel))
        {
            skillPointsEarned++;

            Player.Instance.AddBeatenLevel(currentLevel);
        }

        // TO DO: Find a better way than using Tuple
        // This is very awkward to use in such cases
        bool timeAchievementObtained = timeToBeatLevelSeconds <= (timeAchivementMinutes * 60.0f);
        if (!Player.Instance.BeatLevels[currentLevel].timeAchievementObtained &&
            timeAchievementObtained)
        {
            skillPointsEarned++;
        }

        bool resourcesAchievementObtained = resourcesUsed <= resourcesUsedAchievement;
        if (!Player.Instance.BeatLevels[currentLevel].resourcesAchievementObtained &&
            resourcesAchievementObtained)
        {
            skillPointsEarned++;
        }

        Player.Instance.SetBeatenLevelAchievements(currentLevel, timeAchievementObtained, resourcesAchievementObtained);
        Player.Instance.AddSkillPoints(skillPointsEarned);

        return skillPointsEarned;
    }

    public void OnShopButtonClicked()
    {
        SceneManager.LoadSceneAsync("Shop");
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadSceneAsync("Level");
    }
}
