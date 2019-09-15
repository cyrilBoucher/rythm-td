using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public string mapResourceName;
    public int startResourcesNumber;
    public Camera mainCamera;
    public List<GameObject> enemySpawnerGameObjects;
    public GameObject defenseSpawnerGameObject;
    public GameObject playerBaseGameObject;
    public GameObject resourcesMinePrefab;
    public GameObject worldSpaceCanvasPrefab;
    public float timeAchievementMinutes;
    public int resourcesAchievement;

    public bool loadMap = true;
    static public GameObject worldSpaceCanvasInstance;

    private int _enemiesAlive;
    private float _timeToBeatLevel;
    private int _usedResources;

    void OnEnable()
    {
        EnemyController.DeathEvent += OnEnemyDeath;
        BaseController.DestroyedEvent += OnBaseDestroyed;
        ResourcesController.ResourcesTaken += OnResourcesTaken;
    }

    void OnDisable()
    {
        EnemyController.DeathEvent -= OnEnemyDeath;
        BaseController.DestroyedEvent -= OnBaseDestroyed;
        ResourcesController.ResourcesTaken -= OnResourcesTaken;
    }

    void OnDestroy()
    {
        ResourcesController.Destroy();
    }

    void Awake()
    {
        if (loadMap)
        {
            if (!string.IsNullOrEmpty(mapResourceName))
            {
                Map.LoadMap(mapResourceName);
            }
            else
            {
                Map.LoadMap(LevelSelectController.selectedLevel.levelName);
            }

            // Make sure we see all the map
            // -10 is used as z to see elements usually positioned at z = 0
            // add 1 to orthographic size to that the top and bottom of map show inside view
            mainCamera.transform.position = new Vector3(Map.mapDimensions.x / 2.0f, Map.mapDimensions.y / 2.0f, -10.0f);
            mainCamera.orthographicSize = ((Map.mapDimensions.x + 1.0f) / mainCamera.aspect) / 2.0f;
        }

        worldSpaceCanvasPrefab.GetComponent<Canvas>().worldCamera = mainCamera;
        worldSpaceCanvasInstance = Instantiate(worldSpaceCanvasPrefab, mainCamera.transform.position, Quaternion.identity);
    }

    // Use this for initialization
    void Start ()
    {
        if (loadMap)
        {
            foreach(GameObject enemySpawnerGameObject in enemySpawnerGameObjects)
            {
                _enemiesAlive += enemySpawnerGameObject.GetComponent<EnemySpawner>().GetNumberOfEnemiesToSpawn();

                Instantiate(enemySpawnerGameObject, Map.enemySpawnPosition, Quaternion.identity);
            }

            Instantiate(playerBaseGameObject, Map.basePosition, Quaternion.identity);

            foreach (Vector3 defenseSpawnerPosition in Map.defenseSpawnPositions)
            {
                Instantiate(defenseSpawnerGameObject, defenseSpawnerPosition, Quaternion.identity);
            }

            foreach (Vector3 resourcesMinePosition in Map.resourcesMinePositions)
            {
                Instantiate(resourcesMinePrefab, resourcesMinePosition, Quaternion.identity);
            }
        }

        SkillPointsController.Initialize(0);
        ResourcesController.Instance.Initialize(startResourcesNumber);
        UpgradesController.Initialize();

        _timeToBeatLevel = Time.time;
    }

    void OnEnemyDeath()
    {
        _enemiesAlive--;

        if (_enemiesAlive <= 0)
        {
            OutroData.outroState = OutroData.OutroState.Win;

            LevelWonSceneController.timeToBeatLevelSeconds = Time.time - _timeToBeatLevel;
            LevelWonSceneController.timeAchivementMinutes = timeAchievementMinutes;
            LevelWonSceneController.resourcesUsed = _usedResources;
            LevelWonSceneController.resourcesUsedAchievement = resourcesAchievement;

            LoadLevelWonAndDisable();
        }
    }

    void OnBaseDestroyed()
    {
        OutroData.outroState = OutroData.OutroState.Loose;
        LoadOutroAndDisable();
    }

    void OnResourcesTaken(int resourcesTaken)
    {
        _usedResources += resourcesTaken;
    }

    void LoadOutroAndDisable()
    {
        SceneManager.LoadSceneAsync("Outro");

        // This is to avoid receiving events in the
        // short time it takes to load the outro scene
        gameObject.SetActive(false);
    }

    void LoadLevelWonAndDisable()
    {
        SceneManager.LoadSceneAsync("LevelWon", LoadSceneMode.Additive);

        // This is to avoid receiving events in the
        // short time it takes to load the outro scene
        gameObject.SetActive(false);
    }
}
