using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    static public LevelData selectedLevel;

    public void OnLevelButtonClicked()
    {
        SceneManager.LoadScene("Level");
    }
}
