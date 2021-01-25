using UnityEngine;
using UnityEngine.UI;

public class SkillPointsCounter : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField]
    private Text skillPointsNumberText;
#pragma warning restore CS0649

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
