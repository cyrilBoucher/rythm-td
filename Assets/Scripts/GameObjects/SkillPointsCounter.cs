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
        Player.Instance.SkillPointsNumberChanged += OnSkillPointsNumberChanged;
    }

    void OnDisable()
    {
        Player.Instance.SkillPointsNumberChanged -= OnSkillPointsNumberChanged;
    }

    void Start()
    {
        SetSkillPointsNumber(Player.Instance.SkillPoints);
    }

    void OnSkillPointsNumberChanged()
    {
        SetSkillPointsNumber(Player.Instance.SkillPoints);
    }

    void SetSkillPointsNumber(int skillPointsNumber)
    {
        skillPointsNumberText.text = skillPointsNumber.ToString();
    }
}
