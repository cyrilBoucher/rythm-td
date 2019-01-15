using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BeatEngine {

    static private int _beatsPerMinute;
    static private float _secondsPerBeat;
    static private float _startTimeSec;

    // Use this for initialization
    public static void StartBeat (int beatsPerMinute) {
        _beatsPerMinute = beatsPerMinute;
        _secondsPerBeat = 60.0f / _beatsPerMinute;
        Debug.Log(string.Format("seconds per beat is {0}", _secondsPerBeat));
        _startTimeSec = Time.time;
    }

    public static int BeatId()
    {
        float ellapsedTime = Time.time - _startTimeSec;

        return (int)(ellapsedTime / _secondsPerBeat);
    }

    public static int ClosestBeatId()
    {
        float timeToClosestBeatSec = TimeToClosestBeatSec();

        if (timeToClosestBeatSec >= 0)
        {
            return BeatId();
        }
        else
        {
            return BeatId() + 1;
        }
    }

    public static float SecondsPerBeat()
    {
        return _secondsPerBeat;
    }

    public static float RemainingTimeUntilNextBeatSec()
    {
        float ellapsedTime = Time.time - _startTimeSec;

        if (ellapsedTime < _secondsPerBeat)
        {
            return _secondsPerBeat - ellapsedTime;
        }

        return (_secondsPerBeat - (ellapsedTime % _secondsPerBeat));
    }

    // Returns the time to the closest beat.
    // If the returned time is negative, the closest beat is next
    // If the returned time is positive, the closest beat is passed
    public static float TimeToClosestBeatSec()
    {
        float remaining = RemainingTimeUntilNextBeatSec();

        if (remaining > (_secondsPerBeat / 2.0f))
        {
            return -remaining;
        }
        else
        {
            return _secondsPerBeat - remaining;
        }
    }
}
