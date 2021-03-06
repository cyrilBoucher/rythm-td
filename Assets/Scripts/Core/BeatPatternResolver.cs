﻿using System;
using UnityEngine;

public class BeatPatternResolver
{
    public delegate void OnResolvedAction();
    public event OnResolvedAction ResolvedEvent;

    public delegate void OnInputAction(ReturnType result);
    public event OnInputAction InputEvent;

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

    public BeatPattern GetPattern()
    {
        return _beatPattern;
    }

    public ReturnType Run2(BeatPattern.Input input)
    {
        if (_currentPatternIndex == 0)
        {
            _beatIdToValidate = BeatEngine.instance.ClosestBeatId();
            ReturnType result = Run(BeatEngine.instance.TimeToClosestBeatSec(), input);

            // In this particular case missed means we have not started the
            // pattern
            if (result == ReturnType.Missed)
            {
                return ReturnType.Waiting;
            }
            else
            {
                if (result == ReturnType.Validated)
                {
                    ResolvedEvent();
                }
                else if (result != ReturnType.Waiting)
                {
                    InputEvent(result);
                }

                return result;
            }
        }
        else
        {
            ReturnType result = Run(BeatEngine.instance.TimeToBeatIdSec(_beatIdToValidate), input);

            if (result == ReturnType.Validated)
            {
                ResolvedEvent();
            }
            else if (result != ReturnType.Waiting)
            {
                InputEvent(result);
            }

            return result;
        }
    }

    // This function can be called at any time during execution
    // to start validating the current beat pattern. It is the responsibility
    // of the caller to call it enough times to validate the whole pattern.
    // This function will return an enumerator describing the success or failure.
    public ReturnType Run(double timeToNextBeatIdToValidateSec, BeatPattern.Input input)
    {
        // TODO: Maybe find a better to do this
        float localValidationOffset = validationOffset;

        if (_beatPattern.pattern[_currentPatternIndex] == BeatPattern.Input.SlideDown ||
            _beatPattern.pattern[_currentPatternIndex] == BeatPattern.Input.SlideUp ||
            _beatPattern.pattern[_currentPatternIndex] == BeatPattern.Input.SlideLeft ||
            _beatPattern.pattern[_currentPatternIndex] == BeatPattern.Input.SlideRight)
        {
            localValidationOffset *= 2.0f;
        }

        // Success
        // Hit the beat correctly
        if (input != BeatPattern.Input.Skip &&
            Math.Abs(timeToNextBeatIdToValidateSec) <= localValidationOffset &&
            _beatPattern.pattern[_currentPatternIndex] == input)
        {
            return Success();
        }

        // Success
        // Skipped the beat correctly
        if (input == BeatPattern.Input.Skip &&
            timeToNextBeatIdToValidateSec > localValidationOffset &&
            _beatPattern.pattern[_currentPatternIndex] == BeatPattern.Input.Skip)
        {
            return Success();
        }

        // Fail
        // Either hit before or after the beat
        if (input != BeatPattern.Input.Skip &&
            Math.Abs(timeToNextBeatIdToValidateSec) > localValidationOffset &&
            _beatPattern.pattern[_currentPatternIndex] != BeatPattern.Input.Skip)
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
        if (input != BeatPattern.Input.Skip &&
            _beatPattern.pattern[_currentPatternIndex] == BeatPattern.Input.Skip)
        {
            Failure();

            return ReturnType.WrongNote;
        }

        // Fail
        // Did not hit the beat
        if (input == BeatPattern.Input.Skip &&
            timeToNextBeatIdToValidateSec > localValidationOffset &&
            _beatPattern.pattern[_currentPatternIndex] != BeatPattern.Input.Skip)
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

    public static string EnumToString(ReturnType returnType)
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
