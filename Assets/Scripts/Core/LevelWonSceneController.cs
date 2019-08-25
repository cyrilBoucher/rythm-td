using System.Collections;
using System.Collections.Generic;
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
        int skillPointsEarned = 1;

        if (timeToBeatLevelSeconds <= (timeAchivementMinutes * 60.0f))
        {
            skillPointsEarned++;
        }

        if (resourcesUsed <= resourcesUsedAchievement)
        {
            skillPointsEarned++;
        }

        SkillPointsController.AddSkillPoints(skillPointsEarned);

        return skillPointsEarned;
    }

    public void OnShopButtonClicked()
    {
        SceneManager.LoadSceneAsync("Shop");
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }
}
