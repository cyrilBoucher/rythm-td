using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public string mapResourceName;
    public GameObject resourcesController;
    public GameObject enemySpawnerGameObject;
    public GameObject defenseSpawnerGameObject;
    public GameObject playerBaseGameObject;
    public GameObject worldSpaceCanvas;
    public Text beatCountText;
    public bool loadMap = true;

	// Use this for initialization
	void Start () {
        beatCountText.text = "0";

        if (loadMap)
        {
            Map.LoadMap(mapResourceName);

            enemySpawnerGameObject.GetComponent<EnemySpawner>().resourcesController = resourcesController.GetComponent<ResourcesController>();

            Instantiate(enemySpawnerGameObject, Map.enemySpawnPosition, Quaternion.identity);
            Instantiate(playerBaseGameObject, Map.basePosition, Quaternion.identity);

            defenseSpawnerGameObject.GetComponent<DefenseSpawner>().worldSpaceCanvasGameObject = worldSpaceCanvas;
            defenseSpawnerGameObject.GetComponent<DefenseSpawner>().resourcesController = resourcesController.GetComponent<ResourcesController>();
            foreach (Vector3 defenseSpawnerPosition in Map.defenseSpawnPositions)
            {
                Instantiate(defenseSpawnerGameObject, defenseSpawnerPosition, Quaternion.identity);
            }
        }

        BeatEngine.StartBeat(120);
    }

	// Update is called once per frame
	void Update () {
        beatCountText.text = BeatEngine.BeatId().ToString();
	}
}
