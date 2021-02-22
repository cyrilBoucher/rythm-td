using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    static public string selectedLevelName;

    public void OnLevelButtonClicked()
    {
        SceneManager.LoadScene("Level");
    }
}
