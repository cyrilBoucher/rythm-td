using UnityEngine;

public class ResourcesMineController : MonoBehaviour
{
    public GameObject inputFeedbackTextPrefab;
    public int resourcesNumber;

    private BeatPattern _beatPattern = new BeatPattern();
    private BeatPatternButton _beatPatternButton;
    private InputFeedbackTextController _inputFeedbackTextController;

    // Start is called before the first frame update
    void Start()
    {
        _beatPatternButton = GetComponent<BeatPatternButton>();

        _beatPattern.pattern.Add(BeatPattern.Input.Tap);

        _beatPatternButton.AddPattern(_beatPattern, OnMiningBeatPatternResolved, OnMiningBeatPatternInput);

        GameObject inputFeedbackTextGameObjectInstance = Instantiate(inputFeedbackTextPrefab,
            transform.position + new Vector3(0.0f, 0.5f, 0.0f),
            Quaternion.identity,
            GameController.worldSpaceCanvasInstance.transform);

        _inputFeedbackTextController = inputFeedbackTextGameObjectInstance.GetComponent<InputFeedbackTextController>();
    }

    void OnMiningBeatPatternResolved()
    {
        if (resourcesNumber == 0)
        {
            _inputFeedbackTextController.ShowFeedback("Empty!");

            return;
        }

        ResourcesController.Instance.AddResources(1);
        resourcesNumber--;

        _inputFeedbackTextController.ShowFeedback(BeatPatternResolver.EnumToString(BeatPatternResolver.ReturnType.Good));
    }

    void OnMiningBeatPatternInput(BeatPatternResolver.ReturnType result)
    {
        _inputFeedbackTextController.ShowFeedback(BeatPatternResolver.EnumToString(result));
    }

    void OnDisable()
    {
        _beatPatternButton.RemovePattern(_beatPattern);
    }
}
