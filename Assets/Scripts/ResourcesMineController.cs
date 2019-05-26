using UnityEngine;

public class ResourcesMineController : MonoBehaviour
{
    public ResourcesController resourcesController;    
    public GameObject inputFeedbackTextPrefab;
    public GameObject worldSpaceCanvasGameObject;
    public int resourcesNumber;

    private BeatPatternResolver _beatPatternResolver;
    private InputFeedbackTextController _inputFeedbackTextController;

    // Start is called before the first frame update
    void Start()
    {
        _beatPatternResolver = new BeatPatternResolver();

        BeatPattern beatPattern = new BeatPattern();
        beatPattern.Add(BeatPattern.Input.Tap);

        _beatPatternResolver.SetPattern(beatPattern);

        GameObject inputFeedbackTextGameObjectInstance = Instantiate(inputFeedbackTextPrefab,
            transform.position + new Vector3(0.0f, 0.5f, 0.0f),
            Quaternion.identity,
            worldSpaceCanvasGameObject.transform);

        _inputFeedbackTextController = inputFeedbackTextGameObjectInstance.GetComponent<InputFeedbackTextController>();
    }

    // Update is called once per frame
    void Update()
    {
        BeatPattern.Input input = InputDetector.CheckForInput(GetComponent<BoxCollider2D>());

        BeatPatternResolver.ReturnType result = _beatPatternResolver.Run2(input);

        if (result == BeatPatternResolver.ReturnType.Validated)
        {
            if (resourcesNumber == 0)
            {
                _inputFeedbackTextController.ShowFeedback("Empty!");

                return;
            }

            resourcesController.resourcesNumber++;
            resourcesNumber--;

            _inputFeedbackTextController.ShowFeedback(BeatPatternResolver.EnumToString(BeatPatternResolver.ReturnType.Good));

            return;
        }

        if (result != BeatPatternResolver.ReturnType.Waiting)
        {
            _inputFeedbackTextController.ShowFeedback(BeatPatternResolver.EnumToString(result));
        }
    }
}
