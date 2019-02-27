using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatCounter : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color spriteColor = _spriteRenderer.color;
        spriteColor.a = BeatEngine.RemainingTimeUntilNextBeatSec() / BeatEngine.SecondsPerBeat();

        _spriteRenderer.color = spriteColor;
    }
}
