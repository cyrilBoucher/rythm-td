using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FillButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public class ButtonValidatedEvent : UnityEvent
    {
    }

    public ButtonValidatedEvent onValidated { get; set; } = new ButtonValidatedEvent();

    public Image targetImage;
    public Color fillColor;
    public float fillTimeSeconds = 0.5f;

    // This boolean is used to make sure user
    // has to release and repress after one validation
    private bool _canValidate = true;
    private bool _pointerDown = false;

    void OnValidate()
    {
        fillTimeSeconds = Mathf.Clamp(fillTimeSeconds, 0.0f, float.MaxValue);
    }

    void OnEnable()
    {
        _canValidate = true;
        _pointerDown = false;
    }

    void Start()
    {
        targetImage.color = fillColor;
    }

    void Update()
    {
        if (!_canValidate || !_pointerDown)
        {
            return;
        }

        targetImage.fillAmount += Time.deltaTime / fillTimeSeconds;

        if (targetImage.fillAmount >= 1.0f)
        {
            _canValidate = false;

            targetImage.fillAmount = 0.0f;

            onValidated.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetImage.fillAmount = 0.0f;

        _pointerDown = false;
        _canValidate = true;
    }
}
