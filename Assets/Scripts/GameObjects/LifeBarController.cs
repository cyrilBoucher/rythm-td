using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ColorStep
{
    public float percentage;
    public Color color;
}

public class LifeBarController : MonoBehaviour
{
    public int maxLife;
    public ColorStep[] colorSteps;

    private int _life;
    private Gradient _gradient;
    private GradientColorKey[] _colorKeys;

    public GameObject _lifeBarFill;
    private Image _lifeBarFillImage;

    // Start is called before the first frame update
    void Start()
    {
        _lifeBarFillImage = _lifeBarFill.GetComponent<Image>();

        _gradient = new Gradient();

        _colorKeys = new GradientColorKey[colorSteps.Length];
        for (int i = 0; i < colorSteps.Length; i++)
        {
            _colorKeys[i].color = colorSteps[i].color;
            _colorKeys[i].time = colorSteps[i].percentage;
        }

        _gradient.colorKeys = _colorKeys;

        _life = maxLife;
        _lifeBarFillImage.color = _gradient.Evaluate(1.0f);
    }

    public void TakeDamage(int damage)
    {
        if (damage == 0)
        {
            return;
        }

        _life -= damage;

        if (_life == 0)
        {
            _lifeBarFill.SetActive(false);

            return;
        }

        float percentage = (float)_life / (float)maxLife;
        _lifeBarFillImage.fillAmount = percentage;
        _lifeBarFillImage.color = _gradient.Evaluate(percentage);
    }
}
