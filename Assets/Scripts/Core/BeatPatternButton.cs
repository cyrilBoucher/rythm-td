using System;
using System.Collections.Generic;
using UnityEngine;

public class BeatPatternButton : MonoBehaviour
{
    private class BeatPatternResolutionData
    {
        public BeatPatternResolver beatPatternResolver;
        public BeatPatternResolver.OnResolvedAction onResolvedFunction;
        public BeatPatternResolver.OnInputAction onInputFunction;

    }

    private Dictionary<BeatPattern, BeatPatternResolutionData> _resolutionDatas = new Dictionary<BeatPattern, BeatPatternResolutionData>();
    private BoxCollider2D _collider;

    // Not the best but needed in case RemovePattern is called during
    // an event thrown while we go through the dictionnary
    private List<BeatPattern> _patternsToRemove = new List<BeatPattern>();

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();

        if (_collider == null)
        {
            throw new NullReferenceException("Game object which would like to act on beat pattern resolution need a BoxCollider2D attached");
        }
    }

    // Update is called once per frame
    void Update()
    {
        BeatPattern.Input input = InputDetector.CheckForInput(_collider);
        foreach (BeatPatternResolutionData resolutionData in _resolutionDatas.Values)
        {
            BeatPatternResolver.ReturnType result = resolutionData.beatPatternResolver.Run2(input);
        }

        if (_patternsToRemove.Count == 0)
        {
            return;
        }

        foreach (BeatPattern pattern in _patternsToRemove)
        {
            _resolutionDatas.Remove(pattern);
        }

        _patternsToRemove.Clear();
    }

    public void AddPattern(BeatPattern pattern, BeatPatternResolver.OnResolvedAction onPatternResolvedFunction, BeatPatternResolver.OnInputAction onPatternInputFunction)
    {
        if (pattern == null)
        {
            throw new NullReferenceException("Must provide a valid beat pattern to resolve!");
        }

        if (pattern.pattern.Count == 0)
        {
            throw new ArgumentException("Pattern cannot be empty!");
        }

        BeatPatternResolver resolver = new BeatPatternResolver();
        resolver.SetPattern(pattern);

        if (onPatternResolvedFunction == null ||
            onPatternInputFunction == null)
        {
            throw new NullReferenceException("Both delegate must be valid!");
        }

        resolver.ResolvedEvent += onPatternResolvedFunction;
        resolver.InputEvent += onPatternInputFunction;

        BeatPatternResolutionData resolutionData = new BeatPatternResolutionData();
        resolutionData.beatPatternResolver = resolver;
        resolutionData.onResolvedFunction = onPatternResolvedFunction;
        resolutionData.onInputFunction = onPatternInputFunction;
        _resolutionDatas.Add(pattern, resolutionData);
    }

    public void RemovePattern(BeatPattern pattern)
    {
        BeatPatternResolutionData resolutionData = _resolutionDatas[pattern];

        resolutionData.beatPatternResolver.ResolvedEvent -= resolutionData.onResolvedFunction;
        resolutionData.beatPatternResolver.InputEvent -= resolutionData.onInputFunction;

        _patternsToRemove.Add(pattern);
    }

    void Reset()
    {
        if (_collider == null)
        {
            _collider = GetComponent<BoxCollider2D>();
        }
    }
}
