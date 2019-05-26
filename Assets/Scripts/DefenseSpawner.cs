using UnityEngine;

public class DefenseSpawner : MonoBehaviour {

    private GameObject _inputFeedbackTextGameObjectInstance;
    private InputFeedbackTextController _inputFeedbackTextController;
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

        _inputFeedbackTextController = _inputFeedbackTextGameObjectInstance.GetComponent<InputFeedbackTextController>();
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
                _inputFeedbackTextController.ShowFeedback("Not enough resources!");

                return;
            }

            defenseController.resourcesController = resourcesController;
            defenseController.worldSpaceCanvasGameObject = worldSpaceCanvasGameObject;
            Instantiate(defensePrefab, transform.position, Quaternion.identity);
            resourcesController.resourcesNumber -= defenseController.price;

            Destroy(gameObject);

            // CHECK ME: Maybe avoid doing that?
            Destroy(_inputFeedbackTextGameObjectInstance);

            return;
        }

        if (result != BeatPatternResolver.ReturnType.Waiting)
        {
            _inputFeedbackTextController.ShowFeedback(BeatPatternResolver.EnumToString(result));

            return;
        }
    }
}
