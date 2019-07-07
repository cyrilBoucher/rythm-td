using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesController : MonoBehaviour
{
    public Text resourcesText;
    public int resourcesNumber;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        resourcesText.text = resourcesNumber.ToString();
    }
}
