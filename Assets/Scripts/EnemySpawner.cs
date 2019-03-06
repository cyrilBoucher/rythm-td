﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyGameObject;
    public int enemiesToSpawn;
    public float spawnInterval;

    private float _spawnCooldownSec;
    private int _spawnedEnemies = 0;

    // Start is called before the first frame update
    void Start()
    {
        _spawnCooldownSec = spawnInterval;
        if (Map.enemySpawnPosition != null)
        {
            transform.position = Map.enemySpawnPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_spawnedEnemies >= enemiesToSpawn)
        {
            return;
        }

        if (_spawnCooldownSec >= spawnInterval)
        {
            Instantiate(enemyGameObject, transform.position, Quaternion.identity);
            _spawnedEnemies++;

            _spawnCooldownSec = 0.0f;

            return;
        }

        _spawnCooldownSec += Time.deltaTime;
    }
}
