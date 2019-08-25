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
        SetResourcesNumber(ResourcesController.Instance.resourcesNumber);
    }

    void OnResourcesNumberChanged()
    {
        SetResourcesNumber(ResourcesController.Instance.resourcesNumber);
    }

    void SetResourcesNumber(int resourcesNumber)
    {
        resourcesText.text = ResourcesController.Instance.resourcesNumber.ToString();
    }
}
