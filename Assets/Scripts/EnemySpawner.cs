using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public ResourcesController resourcesController;
    public GameObject enemyGameObject;
    public int enemiesToSpawn;
    public int spawnIntervalBeat;
    public int enemyWaves;
    public int waveIntervalBeat;

    private int _spawnCooldownBeat;
    private int _spawnedEnemies = 0;
    private int _waveCooldownBeat;
    private int _spawnedWaves = 0;
    private int _currentBeatId;

    // Start is called before the first frame update
    void Start()
    {
        _spawnCooldownBeat = spawnIntervalBeat;
        _waveCooldownBeat = 0;
        _currentBeatId = BeatEngine.BeatId();
    }

    // Update is called once per frame
    void Update()
    {
        if (_spawnedWaves >= enemyWaves)
        {
            return;
        }

        if (_spawnedEnemies >= enemiesToSpawn)
        {
            if (_currentBeatId != BeatEngine.BeatId())
            {
                _waveCooldownBeat++;

                _currentBeatId = BeatEngine.BeatId();
            }

            if (_waveCooldownBeat >= waveIntervalBeat)
            {
                _spawnedEnemies = 0;

                _waveCooldownBeat = 0;

                _spawnedWaves++;
            }

            return;
        }

        if (_spawnCooldownBeat >= spawnIntervalBeat)
        {
            EnemyController enemyController = enemyGameObject.GetComponent<EnemyController>();
            enemyController.resourcesController = resourcesController;
            Instantiate(enemyGameObject, transform.position, Quaternion.identity);

            _spawnedEnemies++;

            _spawnCooldownBeat = 0;

            return;
        }

        if (_currentBeatId != BeatEngine.BeatId())
        {
            _spawnCooldownBeat++;

            _currentBeatId = BeatEngine.BeatId();
        }
    }
}
