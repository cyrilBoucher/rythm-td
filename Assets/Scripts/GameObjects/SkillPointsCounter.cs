using UnityEngine;
using UnityEngine.UI;

public class SkillPointsCounter : MonoBehaviour
{
    [SerializeField]
    private Text skillPointsNumberText;

    void OnEnable()
    {
        SkillPointsController.SkillPointsNumberChanged += OnSkillPointsNumberChanged;
    }

    void OnDisable()
    {
        SkillPointsController.SkillPointsNumberChanged -= OnSkillPointsNumberChanged;
    }

    void Start()
    {
        SetSkillPointsNumber(SkillPointsController.skillPoints);
    }

    void OnSkillPointsNumberChanged()
    {
        SetSkillPointsNumber(SkillPointsController.skillPoints);
    }

    void SetSkillPointsNumber(int resourcesNumber)
    {
        skillPointsNumberText.text = SkillPointsController.skillPoints.ToString();
    }
}
