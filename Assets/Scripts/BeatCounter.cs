using UnityEngine;
using UnityEngine.UI;

public class BeatCounter : MonoBehaviour
{
    private Image _image;
    private Text _text;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Color spriteColor = _image.color;
        spriteColor.a = BeatEngine.RemainingTimeUntilNextBeatSec() / BeatEngine.SecondsPerBeat();

        _image.color = spriteColor;

        _text.text = BeatEngine.BeatId().ToString();
    }
}
