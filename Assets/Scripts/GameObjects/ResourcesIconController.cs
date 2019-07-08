using UnityEngine;
using UnityEngine.UI;

public class ResourcesIconController : MonoBehaviour
{
    public Text resourcesText;

    // Update is called once per frame
    void Update()
    {
        resourcesText.text = ResourcesController.GetResourcesNumber().ToString();
    }
}
