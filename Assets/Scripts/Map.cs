using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Map
{
    public static EnemyRoute enemyRoute;
    public static Vector3 enemySpawnPosition;
    public static Vector3 basePosition;
    public static List<Vector3> defenseSpawnPositions;
    private static string _mapResourceName;
    private const string WrongMapFormatErrorMessage = "Wrong format used in {0}";

    public static bool LoadMap(String mapResourceName)
    {
        YamlDotNet.Serialization.Deserializer deserializer = new YamlDotNet.Serialization.DeserializerBuilder().Build();

        TextAsset textAsset = Resources.Load<TextAsset>(mapResourceName);

        if (textAsset == null)
        {
            Debug.LogError(String.Format("Could not find any text asset with the name {0}", mapResourceName));

            return false;
        }

        _mapResourceName = mapResourceName;

        Dictionary<String, List<int[]>> parsed = deserializer.Deserialize<Dictionary<String, List<int[]>>>(textAsset.text);

        if (!DeserializePlayerBasePosition(parsed))
        {
            return false;
        }

        if (!DeserializeEnemySpawnPosition(parsed))
        {
            return false;
        }

        if (!DeserializeDefenseSpawnPositions(parsed))
        {
            return false;
        }

        return DeserializeEnemyRoute(parsed);
    }

    static private bool DeserializePlayerBasePosition(Dictionary<String, List<int[]>> deserializedMapData)
    {
        List<int[]> deserializedBasePosition = deserializedMapData["playerBase"];

        if (deserializedBasePosition.Count != 1)
        {
            Debug.LogError(String.Format(WrongMapFormatErrorMessage, _mapResourceName));

            return false;
        }

        int[] deserializedBasePositionVector2 = deserializedBasePosition[0];

        if (deserializedBasePositionVector2.Length != 2)
        {
            Debug.LogError(String.Format(WrongMapFormatErrorMessage, _mapResourceName));

            return false;
        }

        basePosition = new Vector3(deserializedBasePositionVector2[0], deserializedBasePositionVector2[1], 0.0f);

        return true;
    }

    static private bool DeserializeEnemySpawnPosition(Dictionary<String, List<int[]>> deserializedMapData)
    {
        List<int[]> deserializedSpawnPosition = deserializedMapData["enemySpawn"];

        if (deserializedSpawnPosition.Count != 1)
        {
            Debug.LogError(String.Format(WrongMapFormatErrorMessage, _mapResourceName));

            return false;
        }

        int[] deserializedSpawnPositionVector2 = deserializedSpawnPosition[0];

        if (deserializedSpawnPositionVector2.Length != 2)
        {
            Debug.LogError(String.Format(WrongMapFormatErrorMessage, _mapResourceName));

            return false;
        }

        enemySpawnPosition = new Vector3(deserializedSpawnPositionVector2[0], deserializedSpawnPositionVector2[1], 0.0f);

        return true;
    }

    static private bool DeserializeDefenseSpawnPositions(Dictionary<String, List<int[]>> deserializedMapData)
    {
        List<int[]> deserializedDefenseSpawnPositions = deserializedMapData["defenseSpawn"];

        defenseSpawnPositions = new List<Vector3>();

        foreach (int[] deserializedEnemyRoutePositionVector2 in deserializedDefenseSpawnPositions)
        {
            if (deserializedEnemyRoutePositionVector2.Length != 2)
            {
                Debug.LogError(String.Format(WrongMapFormatErrorMessage, _mapResourceName));

                return false;
            }

            defenseSpawnPositions.Add(new Vector3(deserializedEnemyRoutePositionVector2[0], deserializedEnemyRoutePositionVector2[1], 0.0f));
        }

        return true;
    }

    static private bool DeserializeEnemyRoute(Dictionary<String, List<int[]>> deserializedMapData)
    {
        enemyRoute = new EnemyRoute();

        List<int[]> deserializedEnemyRoute = deserializedMapData["enemyRoute"];

        List<Vector3> enemyRoutePositions = new List<Vector3>();
        foreach (int[] deserializedEnemyRoutePositionVector3 in deserializedEnemyRoute)
        {
            if (deserializedEnemyRoutePositionVector3.Length != 2)
            {
                Debug.LogError(String.Format(WrongMapFormatErrorMessage, _mapResourceName));

                return false;
            }

            enemyRoutePositions.Add(new Vector3(deserializedEnemyRoutePositionVector3[0], deserializedEnemyRoutePositionVector3[1], 0.0f));
        }

        enemyRoute.Set(enemyRoutePositions);

        return true;
    }
}
