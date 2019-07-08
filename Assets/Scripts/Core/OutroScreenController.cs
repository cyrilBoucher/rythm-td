using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OutroScreenController : MonoBehaviour
{
    public Text _resultText;

    // Start is called before the first frame update
    void Start()
    {
        switch (OutroData.outroState)
        {
            case OutroData.OutroState.Win:
                _resultText.text = "You won!";
                break;
            case OutroData.OutroState.Loose:
                _resultText.text = "You lost...";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void OnShopButtonClicked()
    {
        SceneManager.LoadSceneAsync("Shop");
    }
}