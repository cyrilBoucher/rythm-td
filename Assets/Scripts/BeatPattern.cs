using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatPattern : IEnumerable
{
    public enum Input
    {
        OnBeat,
        SkipBeat
    }

    public List<Input> pattern = new List<Input>();

    public IEnumerator GetEnumerator()
    {
        foreach(Input input in pattern)
        {
            yield return input;
        }
    }
}
