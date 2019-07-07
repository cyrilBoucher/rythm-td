using UnityEngine;
using UnityEngine.UI;

public class BeatCounter : MonoBehaviour, IBeatActor
{
    private Image _image;
    private Text _text;

    public void OnBeat()
    {
        _text.text = BeatEngine.instance.BeatId().ToString();

        Color spriteColor = _image.color;
        spriteColor.a = 1.0f;

        _image.color = spriteColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<Text>();

        BeatEngine.BeatEvent += OnBeat;
    }

    // Update is called once per frame
    void Update()
    {
        Color spriteColor = _image.color;
        spriteColor.a -= Time.deltaTime / (float)BeatEngine.instance.SecondsPerBeat();

        _image.color = spriteColor;
    }

    void OnDestroy()
    {
        BeatEngine.BeatEvent -= OnBeat;
    }
}
