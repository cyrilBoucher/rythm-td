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
    private const string WrongMapFormatErrorMessage = "Wrong format used in map1.yaml";

    public static bool LoadMap()
    {
        YamlDotNet.Serialization.Deserializer deserializer = new YamlDotNet.Serialization.DeserializerBuilder().Build();

        TextAsset textAsset = Resources.Load<TextAsset>("map1");

        Dictionary<String, List<int[]>> parsed = deserializer.Deserialize<Dictionary<String, List<int[]>>>(textAsset.text);

        if (!DeserializeBasePosition(parsed))
        {
            return false;
        }

        if (!DeserializeSpawnPosition(parsed))
        {
            return false;
        }

        return DeserializeEnemyRoute(parsed);
    }

    static private bool DeserializeBasePosition(Dictionary<String, List<int[]>> deserializedMapData)
    {
        List<int[]> deserializedBasePosition = deserializedMapData["base"];

        if (deserializedBasePosition.Count != 1)
        {
            Debug.LogError(WrongMapFormatErrorMessage);

            return false;
        }

        int[] deserializedBasePositionVector3 = deserializedBasePosition[0];

        if (deserializedBasePositionVector3.Length != 3)
        {
            Debug.LogError(WrongMapFormatErrorMessage);

            return false;
        }

        basePosition = new Vector3(deserializedBasePositionVector3[0], deserializedBasePositionVector3[1], deserializedBasePositionVector3[2]);

        return true;
    }

    static private bool DeserializeSpawnPosition(Dictionary<String, List<int[]>> deserializedMapData)
    {
        List<int[]> deserializedSpawnPosition = deserializedMapData["enemySpawn"];

        if (deserializedSpawnPosition.Count != 1)
        {
            Debug.LogError(WrongMapFormatErrorMessage);

            return false;
        }

        int[] deserializedSpawnPositionVector3 = deserializedSpawnPosition[0];

        if (deserializedSpawnPositionVector3.Length != 3)
        {
            Debug.LogError(WrongMapFormatErrorMessage);

            return false;
        }

        enemySpawnPosition = new Vector3(deserializedSpawnPositionVector3[0], deserializedSpawnPositionVector3[1], deserializedSpawnPositionVector3[2]);

        return true;
    }

    static private bool DeserializeEnemyRoute(Dictionary<String, List<int[]>> deserializedMapData)
    {
        enemyRoute = new EnemyRoute();

        List<int[]> deserializedEnemyRoute = deserializedMapData["enemyRoute"];

        List<Vector3> enemyRoutePositions = new List<Vector3>();
        foreach (int[] deserializedEnemyRoutePositionVector3 in deserializedEnemyRoute)
        {
            if (deserializedEnemyRoutePositionVector3.Length != 3)
            {
                Debug.LogError(WrongMapFormatErrorMessage);

                return false;
            }

            enemyRoutePositions.Add(new Vector3(deserializedEnemyRoutePositionVector3[0], deserializedEnemyRoutePositionVector3[1], deserializedEnemyRoutePositionVector3[2]));
        }

        enemyRoute.Set(enemyRoutePositions);

        return true;
    }
}
