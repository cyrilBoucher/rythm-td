using System;
using UnityEngine;

public class BeatPatternResolver
{
    private BeatPattern _beatPattern;
    private int _beatIdToValidate;
    private int _currentPatternIndex = 0;
    public float validationOffset = 0.2f;

    public enum ReturnType
    {
        Early,
        Late,
        WrongNote,
        WrongBeat,
        Missed,
        Good,
        Waiting,
        Validated
    }

    public void SetPattern(BeatPattern pattern)
    {
        _beatPattern = pattern;
    }

    public ReturnType Run2(bool input)
    {
        if (_currentPatternIndex == 0)
        {
            _beatIdToValidate = BeatEngine.ClosestBeatId();
            ReturnType result = Run(BeatEngine.TimeToClosestBeatSec(), input);

            // In this particular missed means we have not started the
            // pattern
            if (result == ReturnType.Missed)
            {
                return ReturnType.Waiting;
            }
            else
            {
                return result;
            }
        }
        else
        {
            return Run(BeatEngine.TimeToBeatIdSec(_beatIdToValidate), input);
        }
    }

    // This function can be called at any time during execution
    // to start validating the current beat pattern. It is the responsibility
    // of the caller to call it enough times to validate the whole pattern.
    // This function will return an enumerator describing the success or failure.
    public ReturnType Run(float timeToNextBeatIdToValidateSec, bool input)
    {
        // Success
        // Hit the beat correctly
        if (input &&
            Math.Abs(timeToNextBeatIdToValidateSec) <= validationOffset &&
            _beatPattern.At(_currentPatternIndex) == BeatPattern.Input.OnBeat)
        {
            return Success();
        }

        // Success
        // Skipped the beat correctly
        if (!input &&
            timeToNextBeatIdToValidateSec > validationOffset &&
            _beatPattern.At(_currentPatternIndex) == BeatPattern.Input.SkipBeat)
        {
            return Success();
        }

        // Fail
        // Either hit before or after the beat
        if (input &&
            Math.Abs(timeToNextBeatIdToValidateSec) > validationOffset &&
            _beatPattern.At(_currentPatternIndex) == BeatPattern.Input.OnBeat)
        {
            Failure();

            if (timeToNextBeatIdToValidateSec < 0.0f)
            {
                return ReturnType.Early;
            }
            else
            {
                return ReturnType.Late;
            }
        }

        // Fail
        // Hit the beat when it should have been skipped
        if (input &&
            _beatPattern.At(_currentPatternIndex) == BeatPattern.Input.SkipBeat)
        {
            Failure();

            return ReturnType.WrongNote;
        }

        // Fail
        // Did not hit the beat
        if (!input &&
            timeToNextBeatIdToValidateSec > validationOffset &&
            _beatPattern.At(_currentPatternIndex) == BeatPattern.Input.OnBeat)
        {
            Failure();

            return ReturnType.Missed;
        }

        // Called in between inputs, waiting for next call
        return ReturnType.Waiting;
    }

    // Returns whether this current call of Success()
    // either validates the whole pattern or not
    ReturnType Success()
    {
        _currentPatternIndex++;

        if (_currentPatternIndex == _beatPattern.pattern.Count)
        {
            _currentPatternIndex = 0;

            return ReturnType.Validated;
        }

        _beatIdToValidate++;

        return ReturnType.Good;
    }

    void Failure()
    {
        _currentPatternIndex = 0;
    }

    public String EnumToString(ReturnType returnType)
    {
        switch(returnType)
        {
            case ReturnType.Early:
                return "Early";
            case ReturnType.Late:
                return "Late";
            case ReturnType.Missed:
                return "Missed";
            case ReturnType.WrongBeat:
                return "WrongBeat";
            case ReturnType.WrongNote:
                return "WrongNote";
            case ReturnType.Good:
                return "Good";
            case ReturnType.Validated:
                return "Validated";
            default:
            case ReturnType.Waiting:
                return "Waiting";
        }
    }
}
