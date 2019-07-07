using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public ResourcesController resourcesController;
    public List<UpgradeButtonController> upgradeButtons = new List<UpgradeButtonController>();

    private List<Upgrade> _upgrades = new List<Upgrade>();

    void Start()
    {
        foreach (UpgradeButtonController upgradeButton in upgradeButtons)
        {
            Upgrade upgrade = UpgradeFactory.CreateUpgrade(upgradeButton.upgradeType);
            _upgrades.Add(upgrade);

            upgradeButton.GetComponent<Button>().onClick.AddListener(delegate { OnDefenseUpgradeButtonClicked(upgrade); });
            upgradeButton.SetPrice(upgrade.price);
            upgradeButton.SetName(upgrade.name);
            if (resourcesController.resourcesNumber < upgrade.price)
            {
                upgradeButton.SetBuyable(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            if (resourcesController.resourcesNumber < _upgrades[i].price)
            {
                upgradeButtons[i].SetBuyable(false);
            }
        }
    }

    public void OnDefenseUpgradeButtonClicked(Upgrade upgrade)
    {
        Debug.Log(string.Format("Bought {0} upgrade! Spending {1} resources", upgrade.name, upgrade.price));
        resourcesController.resourcesNumber -= upgrade.price;

        // TODO: Add this upgrade somewhere
    }
}
