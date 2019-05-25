using System;
using UnityEngine;
using UnityEngine.UI;

public class DefenseSpawner : MonoBehaviour {

    private GameObject _inputFeedbackTextGameObjectInstance;
    private BeatPattern _beatPattern = new BeatPattern();
    private BeatPatternResolver _beatPatternResolver = new BeatPatternResolver();

    public ResourcesController resourcesController;
    public GameObject worldSpaceCanvasGameObject;
    public GameObject inputFeedbackTextPrefab;
    public GameObject defensePrefab;

    // Use this for initialization
    void Start () {
        _beatPattern.Add(BeatPattern.Input.Tap);
        _beatPattern.Add(BeatPattern.Input.Tap);

        _beatPatternResolver.SetPattern(_beatPattern);

        _inputFeedbackTextGameObjectInstance = Instantiate(inputFeedbackTextPrefab,
            transform.position + new Vector3(0.0f, 0.5f, 0.0f),
            Quaternion.identity,
            worldSpaceCanvasGameObject.transform);
    }

    // Update is called once per frame
    void Update () {

        BeatPattern.Input input = InputDetector.CheckForInput(GetComponent<BoxCollider2D>());

        BeatPatternResolver.ReturnType result = _beatPatternResolver.Run2(input);

        if (result == BeatPatternResolver.ReturnType.Validated)
        {
            DefenseController defenseController = defensePrefab.GetComponent<DefenseController>();

            if (resourcesController.resourcesNumber < defenseController.price)
            {
                return;
            }

            defenseController.resourcesController = resourcesController;
            Instantiate(defensePrefab, transform.position, Quaternion.identity);
            resourcesController.resourcesNumber -= defenseController.price;

            Destroy(gameObject);

            // TODO: Avoid doing that
            Destroy(_inputFeedbackTextGameObjectInstance);

            return;
        }

        if (result != BeatPatternResolver.ReturnType.Waiting)
        {
            InputFeedbackTextController controller = _inputFeedbackTextGameObjectInstance.GetComponent<InputFeedbackTextController>();
            controller.ShowFeedback(BeatPatternResolver.EnumToString(result));

            return;
        }
    }
}
