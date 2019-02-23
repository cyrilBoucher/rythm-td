using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DefenseSpawner : MonoBehaviour {

    private BeatPattern _beatPattern = new BeatPattern();
    private BeatPatternResolver _beatPatternResolver = new BeatPatternResolver();

    // Use this for initialization
    void Start () {
        _beatPattern.Add(BeatPattern.Input.OnBeat);
        _beatPattern.Add(BeatPattern.Input.OnBeat);

        _beatPatternResolver.SetPattern(_beatPattern);
    }
	
	// Update is called once per frame
	void Update () {
        bool result = _beatPatternResolver.Run();

        if (result)
        {
            Debug.Log("IT WOOOOOOOOOOOOORKS");
        }
	}
}
