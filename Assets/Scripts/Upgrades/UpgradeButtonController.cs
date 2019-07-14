using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonController : MonoBehaviour
{
    public FillButton fillButton { get; private set; }

    public Upgrade.Type upgradeType;
    public Text buttonText;
    public Text priceText;
    public Text levelText;
    public Text boughtText;

    private Button _button;
    private int _price;

    void Awake()
    {
        _button = GetComponent<Button>();
        fillButton = GetComponent<FillButton>();
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
        _price = price;

        priceText.text = _price.ToString();
    }

    public int GetPrice()
    {
        return _price;
    }

    public void SetBuyable(bool buyable)
    {
        _button.interactable = buyable;

        fillButton.enabled = buyable;
    }

    public void SetAcquired(bool acquired)
    {
        boughtText.enabled = acquired;
    }

    public bool HasBeenAcquired()
    {
        return boughtText.enabled;
    }
}
