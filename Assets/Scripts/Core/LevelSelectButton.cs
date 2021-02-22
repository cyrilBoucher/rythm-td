using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public string levelName;
    public Image resourcesAchievementHint;
    public Image timeAchievementHint;

    private void Start()
    {
        LevelData currentLevelData = new LevelData();
        if (Player.Instance.BeatLevels.ContainsKey(levelName))
        {
            currentLevelData = Player.Instance.BeatLevels[levelName];
        }

        if (!currentLevelData.resourcesAchievementObtained)
        {
            resourcesAchievementHint.color = new Color(0.3f, 0.3f, 0.3f, 0.7f);
        }

        if (!currentLevelData.timeAchievementObtained)
        {
            timeAchievementHint.color = new Color(0.3f, 0.3f, 0.3f, 0.7f);
        }
    }

    public void OnButtonClicked()
    {
        LevelSelectController.selectedLevelName = levelName;
    }
}
