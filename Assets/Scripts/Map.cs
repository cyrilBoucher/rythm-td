using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Map
{
    public static EnemyRoute enemyRoute;

    public static bool LoadMap()
    {
        return LoadEnemyRoute();
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
