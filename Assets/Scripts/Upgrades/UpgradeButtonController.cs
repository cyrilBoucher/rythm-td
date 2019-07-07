using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonController : MonoBehaviour
{
    public Upgrade.Type upgradeType;
    public Text buttonText;
    public Text priceText;

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void SetName(string name)
    {
        buttonText.text = name;
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
