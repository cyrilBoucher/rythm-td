using System;
using UnityEngine;

public class BeatPatternResolver
{
    private BeatPattern _beatPattern;
    private int beatIdToValidate;
    private int currentPatternIndex = 0;
    public float validationOffset = 0.2f;

    public void SetPattern(BeatPattern pattern)
    {
        _beatPattern = pattern;
    }

    // This function can be called at any time during execution
    // to start validating the current beat pattern. It is the responsibility
    // of the caller to call it enough times to validate the whole patter.
    // This function will return either true or false whether the pattern
    // was validated or not.
    public bool Run()
    {
        float timeToClosestBeatSec = BeatEngine.TimeToClosestBeatSec();

        if (currentPatternIndex == 0)
        {
            if (timeToClosestBeatSec > validationOffset)
            {
                beatIdToValidate = BeatEngine.BeatId();
            }
            else
            {
                beatIdToValidate = BeatEngine.ClosestBeatId();
            }
        }

        // Success
        if (Input.GetMouseButtonDown(0) &&
            Math.Abs(timeToClosestBeatSec) <= validationOffset &&
            _beatPattern.At(currentPatternIndex) == BeatPattern.Input.OnBeat &&
            beatIdToValidate == BeatEngine.ClosestBeatId())
        {
            Debug.Log(String.Format("Success on beat at frame {0}", Time.frameCount));

            return Success();
        }

        // Success
        if (!Input.GetMouseButtonDown(0) &&
            timeToClosestBeatSec > validationOffset &&
            _beatPattern.At(currentPatternIndex) == BeatPattern.Input.SkipBeat &&
            beatIdToValidate == BeatEngine.ClosestBeatId())
        {
            Debug.Log("Success skip beat");

            return Success();
        }

        // Fail
        if (Input.GetMouseButtonDown(0) &&
            Math.Abs(timeToClosestBeatSec) > validationOffset)
        {
            Debug.Log(String.Format("Failure: pressed outside zone at frame {0}", Time.frameCount));

            return Failure();
        }

        // Fail
        if (currentPatternIndex != 0 &&
            !Input.GetMouseButtonDown(0) &&
            timeToClosestBeatSec > validationOffset &&
            _beatPattern.At(currentPatternIndex) == BeatPattern.Input.OnBeat &&
            beatIdToValidate == BeatEngine.ClosestBeatId())
        {
            Debug.Log("Failure: no press when it should have been pressed");

            return Failure();
        }

        // Fail
        if (Input.GetMouseButtonDown(0) &&
            Math.Abs(timeToClosestBeatSec) <= validationOffset &&
            _beatPattern.At(currentPatternIndex) == BeatPattern.Input.SkipBeat)
        {
            Debug.Log("Failure: pressed, should have skipped");

            return Failure();
        }

        // Fail
        if (Input.GetMouseButtonDown(0) &&
            Math.Abs(timeToClosestBeatSec) <= validationOffset &&
            _beatPattern.At(currentPatternIndex) == BeatPattern.Input.OnBeat &&
            beatIdToValidate != BeatEngine.ClosestBeatId())
        {
            Debug.Log("Failure: pressed on wrong beat");

            return Failure();
        }

        return false;
    }

    bool Success()
    {
        currentPatternIndex++;

        if (currentPatternIndex == _beatPattern.pattern.Count)
        {
            Debug.Log("Success");
            currentPatternIndex = 0;

            return true;
        }

        beatIdToValidate++;

        return false;
    }

    bool Failure()
    {
        currentPatternIndex = 0;

        return false;
    }
}
