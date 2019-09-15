using System;
using System.Collections.Generic;

public class LevelController
{
    public static LevelController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LevelController();
            }

            return _instance;
        }
    }

    private static LevelController _instance = null;

    private Dictionary<string, LevelData> _levelDatasByName;

    private LevelController()
    {
        _levelDatasByName = new Dictionary<string, LevelData>()
        {
            {"maptest", new LevelData("maptest") }
        };
    }

    public LevelData GetLevelData(string levelName)
    {
        if (!_levelDatasByName.ContainsKey(levelName))
        {
            throw new NullReferenceException(string.Format("There is no level with the name {0}", levelName));
        }

        return _levelDatasByName[levelName];
    }
}