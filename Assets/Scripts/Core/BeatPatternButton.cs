using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeatPatternButton : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private class BeatPatternResolutionData
    {
        public BeatPatternResolver beatPatternResolver;
        public BeatPatternResolver.OnResolvedAction onResolvedFunction;
        public BeatPatternResolver.OnInputAction onInputFunction;

    }

    private Dictionary<BeatPattern, BeatPatternResolutionData> _resolutionDatas = new Dictionary<BeatPattern, BeatPatternResolutionData>();

    // Not the best but needed in case RemovePattern is called during
    // an event thrown while we go through the dictionnary
    private List<BeatPattern> _patternsToRemove = new List<BeatPattern>();

    private BeatPattern.Input _currentInput = BeatPattern.Input.Skip;

    // Update is called once per frame
    void Update()
    {
        foreach (BeatPatternResolutionData resolutionData in _resolutionDatas.Values)
        {
            BeatPatternResolver.ReturnType result = resolutionData.beatPatternResolver.Run2(_currentInput);
        }

        if (_currentInput != BeatPattern.Input.Skip)
        {
            _currentInput = BeatPattern.Input.Skip;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        _currentInput = BeatPattern.Input.Tap;
    }

    public void OnBeginDrag(PointerEventData eventData) { /* Needed for OnEndDrag to be called */ }

    public void OnDrag(PointerEventData eventData) { /* Needed for OnEndDrag to be called */ }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 dragDirection = eventData.position - eventData.pressPosition;
        dragDirection.Normalize();

        float absoluteX = Mathf.Abs(dragDirection.x);
        float absoluteY = Mathf.Abs(dragDirection.y);

        if (absoluteX > absoluteY)
        {
            if (dragDirection.x > 0.0f)
            {
                _currentInput = BeatPattern.Input.SlideRight;
            }
            else
            {
                _currentInput = BeatPattern.Input.SlideLeft;
            }
        }
        else
        {
            if (dragDirection.y > 0.0f)
            {
                _currentInput = BeatPattern.Input.SlideUp;
            }
            else
            {
                _currentInput = BeatPattern.Input.SlideDown;
            }
        }

    }

}
