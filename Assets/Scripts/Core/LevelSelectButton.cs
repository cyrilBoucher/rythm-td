using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public string levelName;
    public Image resourcesAchievementHint;
    public Image timeAchievementHint;

    private LevelData _levelData;
    private Button _button;

    private void Awake()
    {
        _levelData = LevelController.Instance.GetLevelData(levelName);

        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.interactable = !_levelData.locked;

        if (!_levelData.resourcesAchievementObtained)
        {
            resourcesAchievementHint.color = new Color(0.3f, 0.3f, 0.3f, 0.7f);
        }

        if (!_levelData.timeAchievementObtained)
        {
            timeAchievementHint.color = new Color(0.3f, 0.3f, 0.3f, 0.7f);
        }
    }

    public void OnButtonClicked()
    {
        LevelSelectController.selectedLevel = _levelData;
    }
}
