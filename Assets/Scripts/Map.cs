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

    public static bool LoadMap()
    {
        enemyRoute = new EnemyRoute();

        List<Vector3> decodedEnemyRoute = new List<Vector3>();

        YamlDotNet.Serialization.Deserializer deserializer = new YamlDotNet.Serialization.DeserializerBuilder().Build();

        TextAsset textAsset = Resources.Load<TextAsset>("map1");

        Dictionary<String, List<int[]>> parsed = deserializer.Deserialize<Dictionary<String, List<int[]>>>(textAsset.text);

        List<int[]> decodedBasePosition = parsed["base"];
        basePosition = new Vector3(decodedBasePosition[0][0], decodedBasePosition[0][1], decodedBasePosition[0][2]);

        List<int[]> decodedEnemySpawnPosition = parsed["enemySpawn"];
        enemySpawnPosition = new Vector3(decodedEnemySpawnPosition[0][0], decodedEnemySpawnPosition[0][1], decodedEnemySpawnPosition[0][2]);

        List<int[]> deserializedEnemyRoute = parsed["enemyRoute"];

        foreach(int[] deserializedPosition in deserializedEnemyRoute)
        {
            decodedEnemyRoute.Add(new Vector3(deserializedPosition[0], deserializedPosition[1], deserializedPosition[2]));
        }

        enemyRoute.Set(decodedEnemyRoute);

        return true;

        //return LoadEnemyRoute();
    }

    private static bool LoadEnemyRoute()
    {
        enemyRoute = new EnemyRoute();

        List<Vector3> decodedEnemyRoute = new List<Vector3>();

        TextAsset textAsset = Resources.Load<TextAsset>("map1");

        string[] vectors = textAsset.text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        foreach(string vector in vectors)
        {
            string[] vectorElements = vector.Split(' ');

            if (vectorElements.Length != 3)
            {
                Debug.LogError("Wrong format used in map1.txt");

                return false;
            }

            decodedEnemyRoute.Add(new Vector3(float.Parse(vectorElements[0]), float.Parse(vectorElements[1]), float.Parse(vectorElements[2])));
        }

        enemyRoute.Set(decodedEnemyRoute);

        return true;
    }
}
