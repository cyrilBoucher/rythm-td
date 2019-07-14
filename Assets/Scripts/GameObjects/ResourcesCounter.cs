using UnityEngine;
using UnityEngine.UI;

public class ResourcesCounter : MonoBehaviour
{
    public Text resourcesText;

    void OnEnable()
    {
        ResourcesController.ResourcesNumberChanged += OnResourcesNumberChanged;
    }

    void OnDisable()
    {
        ResourcesController.ResourcesNumberChanged -= OnResourcesNumberChanged;
    }

    void Start()
    {
        SetResourcesNumber(ResourcesController.GetResourcesNumber());
    }

    void OnResourcesNumberChanged()
    {
        SetResourcesNumber(ResourcesController.GetResourcesNumber());
    }

    void SetResourcesNumber(int resourcesNumber)
    {
        resourcesText.text = ResourcesController.GetResourcesNumber().ToString();
    }
}
