using System;
using UnityEngine;
using UnityEngine.UI;

public class DefenseSpawner : MonoBehaviour {

    private Text _inputFeedbackText;
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

        _inputFeedbackTextGameObjectInstance = Instantiate(inputFeedbackTextPrefab, worldSpaceCanvasGameObject.transform);
        _inputFeedbackText = _inputFeedbackTextGameObjectInstance.GetComponent<Text>();

        Color textColor = _inputFeedbackText.color;
        textColor.a = 0.0f;
        _inputFeedbackText.color = textColor;

        _inputFeedbackTextGameObjectInstance.transform.position = transform.position + new Vector3(0.0f, 0.5f, 0.0f);

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

            Destroy(this.gameObject);

            // TODO: Avoid doing that
            Destroy(_inputFeedbackText.gameObject);

            return;
        }

        Color textColor = _inputFeedbackText.color;
        if (result != BeatPatternResolver.ReturnType.Waiting)
        {
            textColor.a = 1.0f;

            _inputFeedbackText.color = textColor;
            _inputFeedbackText.text = _beatPatternResolver.EnumToString(result);
            Debug.Log(_beatPatternResolver.EnumToString(result));

            return;
        }

        if (textColor.a > 0.0f)
        {
            textColor.a -= Math.Max(0.5f * Time.deltaTime, 0.0f);
            _inputFeedbackText.color = textColor;
        }
    }
}
