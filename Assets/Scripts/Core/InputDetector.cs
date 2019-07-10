using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDetector
{
    private static Vector3 _inputPositionOnDownPress;
    private static BoxCollider2D _currentCollider;

    private static Vector3 InputPosition()
    {
#if (UNITY_ANDROID && !UNITY_EDITOR)
        if (Input.touchCount == 0)
        {
            return Vector3.zero;
        }

        Touch touch = Input.GetTouch(0);

        return touch.position;
#elif (UNITY_EDITOR || UNITY_STANDALONE)
        return Input.mousePosition;
#endif
    }

    public static BeatPattern.Input CheckForInput(BoxCollider2D collider)
    {
        BeatPattern.Input input = BeatPattern.Input.Skip;

#if (UNITY_ANDROID && !UNITY_EDITOR)
        if (Input.touchCount == 0)
        {
            return input;
        }

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
#elif (UNITY_EDITOR || UNITY_STANDALONE)
        if (Input.GetMouseButtonDown(0))
#endif
        {
            Vector2 worldInputPos = Camera.main.ScreenToWorldPoint(InputPosition());

            RaycastHit2D hit = Physics2D.Raycast(worldInputPos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider == collider)
                {
                    _inputPositionOnDownPress = InputPosition();
                    _currentCollider = collider;
                }

            }

            return BeatPattern.Input.Skip;
        }

#if (UNITY_ANDROID && !UNITY_EDITOR)
        if (collider == _currentCollider && touch.phase == TouchPhase.Ended)
#elif (UNITY_EDITOR || UNITY_STANDALONE)
        if (collider == _currentCollider && Input.GetMouseButtonUp(0))
#endif
        {
            if ((InputPosition() - _inputPositionOnDownPress).magnitude > 10.0f)
            {
                Vector3 dragVector = InputPosition() - _inputPositionOnDownPress;
                dragVector.Normalize();

                float angle = Vector3.Angle(Vector3.up, dragVector);

                if (angle < 45.0f)
                {
                    input = BeatPattern.Input.SlideUp;

                    Debug.Log("Side up");
                }

                angle = Vector3.Angle(Vector3.right, dragVector);

                if (angle < 45.0f)
                {
                    input = BeatPattern.Input.SlideRight;

                    Debug.Log("Side right");
                }

                angle = Vector3.Angle(Vector3.left, dragVector);

                if (angle < 45.0f)
                {
                    input = BeatPattern.Input.SlideLeft;

                    Debug.Log("Side left");
                }

                angle = Vector3.Angle(Vector3.down, dragVector);

                if (angle < 45.0f)
                {
                    input = BeatPattern.Input.SlideDown;

                    Debug.Log("Side down");
                }
            }
            else
            {
                input = BeatPattern.Input.Tap;

                Debug.Log("Tap");
            }
        }

        return input;
    }
}
