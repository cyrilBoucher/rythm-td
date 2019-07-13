using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public List<UpgradeButtonController> upgradeButtons = new List<UpgradeButtonController>();

    private Dictionary<Upgrade.Type, UpgradeButtonController> _upgradeButtonsByType = new Dictionary<Upgrade.Type, UpgradeButtonController>();

    void OnEnable()
    {
        UpgradesController.UpgradeModified += OnUpgradeModified;
    }

    void OnDisable()
    {
        UpgradesController.UpgradeModified -= OnUpgradeModified;
    }

    void Start()
    {
        foreach (UpgradeButtonController upgradeButton in upgradeButtons)
        {
            Upgrade upgrade = UpgradesController.GetUpgradeFromType(upgradeButton.upgradeType);

            upgradeButton.GetComponent<Button>().onClick.AddListener(delegate { OnDefenseUpgradeButtonClicked(upgradeButton); });
            upgradeButton.SetPrice(upgrade.price);
            upgradeButton.SetName(upgrade.name);
            upgradeButton.SetBuyable(!upgrade.acquired);
            upgradeButton.SetLevel(upgrade.level);

            if (ResourcesController.GetResourcesNumber() < upgrade.price)
            {
                upgradeButton.SetBuyable(false);
            }

            _upgradeButtonsByType.Add(upgradeButton.upgradeType, upgradeButton);
        }
    }

    public void OnDefenseUpgradeButtonClicked(UpgradeButtonController upgradeButton)
    {
        UpgradesController.BuyUpgrade(upgradeButton.upgradeType);
    }

    public void OnDoneButtonClicked()
    {
        SceneManager.LoadSceneAsync("Outro");
    }

    private void OnUpgradeModified(Upgrade upgrade)
    {
        Upgrade.Type upgradeType = upgrade.type;

        if (!_upgradeButtonsByType.ContainsKey(upgradeType))
        {
            throw new MissingReferenceException(string.Format("Missing upgrade of type {0} in ShopController", upgradeType));
        }

        UpgradeButtonController upgradeButton = _upgradeButtonsByType[upgradeType];

        upgradeButton.SetBuyable(!upgrade.acquired);
        upgradeButton.SetLevel(upgrade.level);
        upgradeButton.SetPrice(upgrade.price);
    }
}
