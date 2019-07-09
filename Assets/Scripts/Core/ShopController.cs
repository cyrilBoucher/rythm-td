using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public List<UpgradeButtonController> upgradeButtons = new List<UpgradeButtonController>();

    void Start()
    {
        foreach (UpgradeButtonController upgradeButton in upgradeButtons)
        {
            Upgrade upgrade = UpgradesController.GetUpgradeFromType(upgradeButton.upgradeType);

            upgradeButton.GetComponent<Button>().onClick.AddListener(delegate { OnDefenseUpgradeButtonClicked(upgradeButton); });
            upgradeButton.SetPrice(upgrade.price);
            upgradeButton.SetName(upgrade.name);
            upgradeButton.SetBought(upgrade.bought);
            if (!upgrade.bought &&
                ResourcesController.GetResourcesNumber() < upgrade.price)
            {
                upgradeButton.SetBuyable(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (UpgradeButtonController upgradeButton in upgradeButtons)
        {
            if (!upgradeButton.WasBought() &&
                ResourcesController.GetResourcesNumber() < upgradeButton.GetPrice())
            {
                upgradeButton.SetBuyable(false);
            }
        }
    }

    public void OnDefenseUpgradeButtonClicked(UpgradeButtonController upgradeButton)
    {
        ResourcesController.TakeResources(upgradeButton.GetPrice());
        upgradeButton.SetBought(true);

        UpgradesController.BuyUpgrade(upgradeButton.upgradeType);
    }

    public void OnDoneButtonClicked()
    {
        SceneManager.LoadSceneAsync("Outro");
    }
}
