using System;
using UnityEngine;

public class BeatEngine : MonoBehaviour
{
    public static BeatEngine instance = null;

    public delegate void OnBeatAction();
    public static event OnBeatAction BeatEvent;

    [SerializeField]
    private int _beatsPerMinute;
    private double _secondsPerBeat;
    private double _startTimeSec;
    private double _ellapsedTimeSinceLastBeatSec;
    private double _ellapsedTimeSinceStartSec;
    private double _lastUpdateTime;
    private int _beatId;

    private AudioSource _audioSource;

    public void SetBeatsPerMinute(int beatsPerMinute)
    {
        if (_startTimeSec != 0.0)
        {
            throw new ArgumentException("Beat was already started, cannot set beats per minute");
        }

        _beatsPerMinute = beatsPerMinute;
    }

    public int BeatsPerMinute()
    {
        return _beatsPerMinute;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void StartBeat(int beatsPerMinute)
    {
        _secondsPerBeat = 60.0f / _beatsPerMinute;

        // Add two seconds in dsp time to make sure sound is properly loaded
        _startTimeSec = AudioSettings.dspTime + 2.0f;
        _lastUpdateTime = _startTimeSec;
    }

    void Start()
    {
        StartBeat(_beatsPerMinute);

        _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayScheduled(_startTimeSec);
    }

    void Update()
    {
        if (AudioSettings.dspTime < _startTimeSec)
        {
            return;
        }

        double dspTime = AudioSettings.dspTime;
        double delta = dspTime - _lastUpdateTime;
        _lastUpdateTime = dspTime;

        _ellapsedTimeSinceStartSec += delta;
        _ellapsedTimeSinceLastBeatSec += delta;

        // This comparison makes sure that the event happens the closest to the beat
        // possible. For this we try to see in the future using the delta we just computed.
        // Basically the condition will be true only when either the time difference before
        // the beat is smaller that it would be when the next call to Update occure (current
        // time + delta) or when we past the beat time
        if (Math.Abs(_ellapsedTimeSinceLastBeatSec - _secondsPerBeat) < Math.Abs((_ellapsedTimeSinceLastBeatSec + delta) - _secondsPerBeat))
        {
            _beatId++;

            // Note: At this point, _ellapsedTimeSinceLastBeatSec can be negative
            // which is fine. It actually keeps the time on track as it will go right
            // back to positive when we go over the beat time.
            // CHECK ME: Make quadruple sure that all utility functions still work
            // UNIT TESTS ?
            _ellapsedTimeSinceLastBeatSec -= _secondsPerBeat;

            BeatEvent();
        }
    }

    public int BeatId()
    {
        return _beatId;
    }

    public double SecondsPerBeat()
    {
        return _secondsPerBeat;
    }

    public int ClosestBeatId()
    {
        double timeToClosestBeatSec = TimeToClosestBeatSec();

        if (timeToClosestBeatSec >= 0)
        {
            return _beatId;
        }
        else
        {
            return _beatId + 1;
        }
    }

    public double RemainingTimeUntilNextBeatSec()
    {
        return _secondsPerBeat - _ellapsedTimeSinceLastBeatSec;
    }

    public double TimeToBeatIdSec(int beatId)
    {
        double timeOnBeatId = _secondsPerBeat * beatId;

        return _ellapsedTimeSinceStartSec - timeOnBeatId;
    }

    // Returns the time to the closest beat.
    // If the returned time is negative, the closest beat is next
    // If the returned time is positive, the closest beat is passed
    public double TimeToClosestBeatSec()
    {
        double remaining = RemainingTimeUntilNextBeatSec();

        if (remaining > (_secondsPerBeat / 2.0f))
        {
            return _secondsPerBeat - remaining;
        }
        else
        {
            return -remaining;
        }
    }
}
