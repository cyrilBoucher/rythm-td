using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DefenseSpawner : MonoBehaviour {

    private BeatPattern _beatPattern = new BeatPattern();
    private int _patternIndex;
    private int _lastValidatedBeatId;

	// Use this for initialization
	void Start () {
        _patternIndex = 0;
        _lastValidatedBeatId = -1;
        _beatPattern.Add(BeatPattern.Input.OnBeat);
        _beatPattern.Add(BeatPattern.Input.SkipBeat);
        _beatPattern.Add(BeatPattern.Input.OnBeat);
    }
	
	// Update is called once per frame
	void Update () {
        BeatPattern.Input expectedInput = BeatPattern.Input.SkipBeat;
        if (Input.GetMouseButtonDown(0))
        {
            expectedInput = BeatPattern.Input.OnBeat;
        }

        float timeToClosestBeatSec = BeatEngine.TimeToClosestBeatSec();

        if (Math.Abs(timeToClosestBeatSec) > 0.2f &&
            expectedInput == BeatPattern.Input.SkipBeat)
        {
            return;
        }

        if (Math.Abs(timeToClosestBeatSec) <= 0.2f &&
            _beatPattern.At(_patternIndex) == expectedInput &&
            _lastValidatedBeatId != BeatEngine.ClosestBeatId())
        {
            Debug.Log(string.Format("Success on beat {0}!", BeatEngine.ClosestBeatId()));

            _patternIndex++;
            _lastValidatedBeatId = BeatEngine.ClosestBeatId();

            return;
        }

        Debug.Log(string.Format("Failure on beat {0}!", BeatEngine.ClosestBeatId()));

        _patternIndex = 0;
        _lastValidatedBeatId = -1;
	}

    private void resetBeatPatternIndex()
    {
        Debug.Log("Resetting pattern");
        _patternIndex = 0;
    }
}
