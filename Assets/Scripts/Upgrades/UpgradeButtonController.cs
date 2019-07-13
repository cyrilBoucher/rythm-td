using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonController : MonoBehaviour
{
    public Upgrade.Type upgradeType;
    public Text buttonText;
    public Text priceText;
    public Text levelText;

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void SetName(string name)
    {
        buttonText.text = name;
    }

    public void SetLevel(int level)
    {
        levelText.text = string.Format("Lvl {0}", level);
    }

    public void SetPrice(int price)
    {
        priceText.text = price.ToString();
    }

    public void SetBuyable(bool buyable)
    {
        _button.interactable = buyable;
    }
}
