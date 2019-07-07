using UnityEngine;
using UnityEngine.UI;

public class InputFeedbackTextController : MonoBehaviour
{
    private Text _text;

    public float feedbackDisplayTimeSecond = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();

        Color textColor = _text.color;
        textColor.a = 0.0f;

        _text.color = textColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (_text.color.a > 0.0f)
        {
            Color textColor = _text.color;
            textColor.a -= Time.deltaTime / feedbackDisplayTimeSecond;
            _text.color = textColor;
        }
    }

    public void ShowFeedback(string feedback)
    {
        Color textColor = _text.color;
        textColor.a = 1.0f;
        _text.color = textColor;

        _text.text = feedback;
    }
}
