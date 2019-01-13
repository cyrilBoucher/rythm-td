using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            float remaining = BeatEngine.RemainingTimeUntilNextBeatSec();
            float secondsPerBeat = BeatEngine.SecondsPerBeat();
            float deltaAroundBeat = secondsPerBeat - remaining;
            if (deltaAroundBeat <= 0.2f)
            {
                Debug.Log("Clicked on beat!");
            }
            else
            {
                float lateTime = deltaAroundBeat - 0.2f;
                Debug.Log(string.Format("Missed! You were {0} seconds too late", lateTime));
            }
        }
	}
}
