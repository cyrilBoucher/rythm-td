using System.Collections.Generic;

public class BeatPattern
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

    public override string ToString()
    {
        string finalString = "{";
        for (int i = 0; i < pattern.Count; i++)
        {
            finalString += string.Format(" {0}", pattern[i].ToString());

            if (i != pattern.Count - 1)
            {
                finalString += ",";
            }
        }
        finalString += " }";

        return finalString;
    }
}
