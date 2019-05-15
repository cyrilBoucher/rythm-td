using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatPattern : IEnumerable
{
    public enum Input
    {
        Tap,
        Skip,
        SlideDown,
        SlideUp,
        SlideLeft,
        SlideRight
    }

    public List<Input> pattern = new List<Input>();

    public IEnumerator GetEnumerator()
    {
        foreach(Input input in pattern)
        {
            yield return input;
        }
    }

    public void Add(Input input)
    {
        pattern.Add(input);
    }

    public Input At(int index)
    {
        return pattern[index];
    }
}
