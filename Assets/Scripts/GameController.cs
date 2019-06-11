using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public string mapResourceName;
    public Camera mainCamera;
    public GameObject resourcesController;
    public List<GameObject> enemySpawnerGameObjects;
    public GameObject defenseSpawnerGameObject;
    public GameObject playerBaseGameObject;
    public GameObject resourcesMinePrefab;
    public GameObject worldSpaceCanvas;
    public bool loadMap = true;

    // Use this for initialization
    void Start ()
    {
        if (loadMap)
        {
            Map.LoadMap(mapResourceName);

            foreach(GameObject enemySpawnerGameObject in enemySpawnerGameObjects)
            {
                enemySpawnerGameObject.GetComponent<EnemySpawner>().resourcesController = resourcesController.GetComponent<ResourcesController>();

                Instantiate(enemySpawnerGameObject, Map.enemySpawnPosition, Quaternion.identity);
            }

            Instantiate(playerBaseGameObject, Map.basePosition, Quaternion.identity);

            defenseSpawnerGameObject.GetComponent<DefenseSpawner>().worldSpaceCanvasGameObject = worldSpaceCanvas;
            defenseSpawnerGameObject.GetComponent<DefenseSpawner>().resourcesController = resourcesController.GetComponent<ResourcesController>();
            foreach (Vector3 defenseSpawnerPosition in Map.defenseSpawnPositions)
            {
                Instantiate(defenseSpawnerGameObject, defenseSpawnerPosition, Quaternion.identity);
            }

            foreach (Vector3 resourcesMinePosition in Map.resourcesMinePositions)
            {
                resourcesMinePrefab.GetComponent<ResourcesMineController>().resourcesController = resourcesController.GetComponent<ResourcesController>();
                resourcesMinePrefab.GetComponent<ResourcesMineController>().worldSpaceCanvasGameObject = worldSpaceCanvas;
                Instantiate(resourcesMinePrefab, resourcesMinePosition, Quaternion.identity);
            }

            // Make sure we see all the map
            // -10 is used as z to see elements usually positioned at z = 0
            // add 1 to orthographic size to that the top and bottom of map show inside view
            mainCamera.transform.position = new Vector3(Map.mapDimensions.x / 2.0f, Map.mapDimensions.y / 2.0f, -10.0f);
            mainCamera.orthographicSize = ((Map.mapDimensions.x + 1.0f) / mainCamera.aspect) / 2.0f;
        }
    }

    // Update is called once per frame
    void Update () {
    }
}
