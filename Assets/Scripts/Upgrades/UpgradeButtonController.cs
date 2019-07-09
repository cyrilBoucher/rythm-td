using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonController : MonoBehaviour
{
    public Upgrade.Type upgradeType;
    public Text buttonText;
    public Text priceText;

    private Button _button;
    private int _price;
    private bool _bought;

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
        _price = price;

        priceText.text = _price.ToString();
    }

    public int GetPrice()
    {
        return _price;
    }

    public void SetBought(bool bought)
    {
        _bought = bought;

        SetBuyable(!_bought);
    }

    public bool WasBought()
    {
        return _bought;
    }

    public void SetBuyable(bool buyable)
    {
        _button.interactable = buyable;
    }
}
