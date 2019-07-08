using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
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
            if (ResourcesController.GetResourcesNumber() < upgrade.price)
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
            if (ResourcesController.GetResourcesNumber() < _upgrades[i].price)
            {
                upgradeButtons[i].SetBuyable(false);
            }
        }
    }

    public void OnDefenseUpgradeButtonClicked(Upgrade upgrade)
    {
        ResourcesController.TakeResources(upgrade.price);

        UpgradesController.AddUpgrade(upgrade);
    }

    public void OnDoneButtonClicked()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }
}
