using UnityEngine;

public class DefenseSpawner : MonoBehaviour
{
    private GameObject _inputFeedbackTextGameObjectInstance;
    private InputFeedbackTextController _inputFeedbackTextController;
    private BeatPattern _beatPattern = new BeatPattern();
    private BeatPattern _testBeatPattern = new BeatPattern();
    private BeatPatternButton _beatPatternButton;

    public GameObject inputFeedbackTextPrefab;
    public GameObject defensePrefab;
    public GameObject defense2Prefab;

    // Use this for initialization
    void Start()
    {
        _beatPattern.pattern.Add(BeatPattern.Input.Tap);
        _beatPattern.pattern.Add(BeatPattern.Input.Tap);

        _testBeatPattern.pattern.Add(BeatPattern.Input.Tap);
        _testBeatPattern.pattern.Add(BeatPattern.Input.SlideLeft);
        _testBeatPattern.pattern.Add(BeatPattern.Input.SlideRight);

        _beatPatternButton = GetComponent<BeatPatternButton>();
        _beatPatternButton.AddPattern(_beatPattern, OnBeatPatternResolved, OnBeatPatternInput);
        _beatPatternButton.AddPattern(_testBeatPattern, OnTestBeatPatternResolved, OnBeatPatternInput);

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

    void OnTestBeatPatternResolved()
    {
        DefenseController2 defenseController = defense2Prefab.GetComponent<DefenseController2>();

        if (ResourcesController.GetResourcesNumber() < defenseController.price)
        {
            _inputFeedbackTextController.ShowFeedback("Not enough resources!");

            return;
        }

        Instantiate(defense2Prefab, transform.position, Quaternion.identity);
        ResourcesController.TakeResources(defenseController.price);

        _beatPatternButton.RemovePattern(_testBeatPattern);

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
