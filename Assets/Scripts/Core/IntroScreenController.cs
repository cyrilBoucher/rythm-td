﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Player.Instance.LoadOrCreateFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadSceneAsync("LevelSelect");
    }
}
