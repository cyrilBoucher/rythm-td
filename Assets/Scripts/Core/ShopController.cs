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
        ResourcesController.ResourcesNumberChanged += OnResourcesNumberChanged;
    }

    void OnDisable()
    {
        UpgradesController.UpgradeModified -= OnUpgradeModified;
        ResourcesController.ResourcesNumberChanged -= OnResourcesNumberChanged;
    }

    void Start()
    {
        foreach (UpgradeButtonController upgradeButton in upgradeButtons)
        {
            Upgrade upgrade = UpgradesController.GetUpgradeFromType(upgradeButton.upgradeType);

            upgradeButton.GetComponent<Button>().onClick.AddListener(delegate { OnDefenseUpgradeButtonClicked(upgradeButton); });
            upgradeButton.SetPrice(upgrade.price);
            upgradeButton.SetName(upgrade.name);
            upgradeButton.SetAcquired(upgrade.acquired);
            upgradeButton.SetLevel(upgrade.level);

            UpdateUpgradeButtonBuyableStateFromResourcesNumber(upgradeButton);

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

        upgradeButton.SetAcquired(upgrade.acquired);
        upgradeButton.SetLevel(upgrade.level);
        upgradeButton.SetPrice(upgrade.price);

        UpdateUpgradeButtonBuyableStateFromResourcesNumber(upgradeButton);
    }

    private void OnResourcesNumberChanged()
    {
        foreach(KeyValuePair<Upgrade.Type, UpgradeButtonController> upgradeButtonByType in _upgradeButtonsByType)
        {
            UpdateUpgradeButtonBuyableStateFromResourcesNumber(upgradeButtonByType.Value);
        }
    }

    private void UpdateUpgradeButtonBuyableStateFromResourcesNumber(UpgradeButtonController upgradeButton)
    {
        if (!upgradeButton.HasBeenAcquired())
        {
            bool hasEnoughResources = ResourcesController.GetResourcesNumber() >= upgradeButton.GetPrice();
            upgradeButton.SetBuyable(hasEnoughResources);
        }
    }
}
