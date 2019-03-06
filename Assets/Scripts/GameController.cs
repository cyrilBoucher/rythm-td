using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text beatCountText;
    public bool loadMap = true;

	// Use this for initialization
	void Start () {
        beatCountText.text = "0";

        if (loadMap)
        {
            Map.LoadMap();
        }

        BeatEngine.StartBeat(120);
    }

	// Update is called once per frame
	void Update () {
        beatCountText.text = BeatEngine.BeatId().ToString();
	}
}
