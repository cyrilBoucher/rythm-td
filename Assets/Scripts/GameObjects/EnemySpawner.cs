using UnityEngine;

public class EnemySpawner : MonoBehaviour, IBeatActor
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

    public int GetNumberOfEnemiesToSpawn()
    {
        return enemyWaves * enemiesToSpawn;
    }

    // Start is called before the first frame update
    void Start()
    {
        _spawnCooldownBeat = spawnIntervalBeat;
        _waveCooldownBeat = 0;

        EnemyController enemyController = enemyGameObject.GetComponent<EnemyController>();
        enemyController.resourcesController = resourcesController;

        BeatEngine.BeatEvent += OnBeat;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnBeat()
    {
        if (_spawnedWaves >= enemyWaves)
        {
            return;
        }

        if (_spawnedEnemies >= enemiesToSpawn)
        {
            _waveCooldownBeat++;

            if (_waveCooldownBeat >= waveIntervalBeat)
            {
                _spawnedEnemies = 0;

                _waveCooldownBeat = 0;

                _spawnedWaves++;
            }

            return;
        }

        _spawnCooldownBeat++;

        if (_spawnCooldownBeat >= spawnIntervalBeat)
        {
            Instantiate(enemyGameObject, transform.position, Quaternion.identity);

            _spawnedEnemies++;

            _spawnCooldownBeat = 0;

            return;
        }
    }

    void OnDestroy()
    {
        BeatEngine.BeatEvent -= OnBeat;
    }
}
