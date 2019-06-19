using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public string mapResourceName;
    public Camera mainCamera;
    public GameObject resourcesController;
    public List<GameObject> enemySpawnerGameObjects;
    public GameObject defenseSpawnerGameObject;
    public GameObject playerBaseGameObject;
    public GameObject resourcesMinePrefab;
    public GameObject worldSpaceCanvasPrefab;
    public bool loadMap = true;
    static public GameObject worldSpaceCanvasInstance;

    private int _enemiesAlive;

    void OnEnable()
    {
        EnemyController.DeathEvent += OnEnemyDeath;
        BaseController.DestroyedEvent += OnBaseDestroyed;
    }

    void OnDisable()
    {
        EnemyController.DeathEvent -= OnEnemyDeath;
        BaseController.DestroyedEvent -= OnBaseDestroyed;
    }

    void Awake()
    {
        if (loadMap)
        {
            Map.LoadMap(mapResourceName);

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
                enemySpawnerGameObject.GetComponent<EnemySpawner>().resourcesController = resourcesController.GetComponent<ResourcesController>();
                _enemiesAlive += enemySpawnerGameObject.GetComponent<EnemySpawner>().GetNumberOfEnemiesToSpawn();

                Instantiate(enemySpawnerGameObject, Map.enemySpawnPosition, Quaternion.identity);
            }

            Instantiate(playerBaseGameObject, Map.basePosition, Quaternion.identity);

            defenseSpawnerGameObject.GetComponent<DefenseSpawner>().resourcesController = resourcesController.GetComponent<ResourcesController>();
            foreach (Vector3 defenseSpawnerPosition in Map.defenseSpawnPositions)
            {
                Instantiate(defenseSpawnerGameObject, defenseSpawnerPosition, Quaternion.identity);
            }

            foreach (Vector3 resourcesMinePosition in Map.resourcesMinePositions)
            {
                resourcesMinePrefab.GetComponent<ResourcesMineController>().resourcesController = resourcesController.GetComponent<ResourcesController>();
                Instantiate(resourcesMinePrefab, resourcesMinePosition, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update () {
    }

    void OnEnemyDeath()
    {
        _enemiesAlive--;

        if (_enemiesAlive <= 0)
        {
            OutroData.outroState = OutroData.OutroState.Win;
            SceneManager.LoadSceneAsync("Outro");
        }
    }

    void OnBaseDestroyed()
    {
        OutroData.outroState = OutroData.OutroState.Loose;
        SceneManager.LoadSceneAsync("Outro");
    }
}
