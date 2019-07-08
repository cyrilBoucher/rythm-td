using UnityEngine;

public class DefenseSpawner : MonoBehaviour
{
    private GameObject _inputFeedbackTextGameObjectInstance;
    private InputFeedbackTextController _inputFeedbackTextController;
    private BeatPattern _beatPattern = new BeatPattern();
    private BeatPatternButton _beatPatternButton;

    public GameObject inputFeedbackTextPrefab;
    public GameObject defensePrefab;

    // Use this for initialization
    void Start()
    {
        _beatPattern.pattern.Add(BeatPattern.Input.Tap);
        _beatPattern.pattern.Add(BeatPattern.Input.Tap);

        _beatPatternButton = GetComponent<BeatPatternButton>();
        _beatPatternButton.AddPattern(_beatPattern, OnBeatPatternResolved, OnBeatPatternInput);

        _inputFeedbackTextGameObjectInstance = Instantiate(inputFeedbackTextPrefab,
            transform.position + new Vector3(0.0f, 0.5f, 0.0f),
            Quaternion.identity,
            GameController.worldSpaceCanvasInstance.transform);

        _inputFeedbackTextController = _inputFeedbackTextGameObjectInstance.GetComponent<InputFeedbackTextController>();
    }

    void OnBeatPatternResolved()
    {
        DefenseController defenseController = defensePrefab.GetComponent<DefenseController>();

        if (ResourcesController.GetResourcesNumber() < defenseController.price)
        {
            _inputFeedbackTextController.ShowFeedback("Not enough resources!");

            return;
        }

        Instantiate(defensePrefab, transform.position, Quaternion.identity);
        ResourcesController.TakeResources(defenseController.price);

        _beatPatternButton.RemovePattern(_beatPattern);

        Destroy(gameObject);
        Destroy(_inputFeedbackTextGameObjectInstance);
    }

    void OnBeatPatternInput(BeatPatternResolver.ReturnType result)
    {
        _inputFeedbackTextController.ShowFeedback(BeatPatternResolver.EnumToString(result));
    }

    void OnDisable()
    {
        _beatPatternButton.RemovePattern(_beatPattern);
    }
}
