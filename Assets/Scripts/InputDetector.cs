using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDetector
{
    private static Vector3 _mousePositionOnButtonDown;
    private static BoxCollider2D _currentCollider;

    public static BeatPattern.Input CheckForInput(BoxCollider2D collider)
    {
        BeatPattern.Input input = BeatPattern.Input.Skip;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldMousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldMousPos, -Vector2.up);

            if (hit.collider != null)
            {
                if (hit.transform.GetComponent<BoxCollider2D>() == collider)
                {
                    _mousePositionOnButtonDown = Input.mousePosition;
                    _currentCollider = collider;
                }

            }

            return BeatPattern.Input.Skip;
        }

        if (collider == _currentCollider && Input.GetMouseButtonUp(0))
        {
            if ((Input.mousePosition - _mousePositionOnButtonDown).magnitude > 10.0f)
            {
                Vector3 mouseDragVector = Input.mousePosition - _mousePositionOnButtonDown;
                mouseDragVector.Normalize();

                float angle = Vector3.Angle(Vector3.up, mouseDragVector);

                if (angle < 45.0f)
                {
                    input = BeatPattern.Input.SlideUp;

                    Debug.Log("Side up");
                }

                angle = Vector3.Angle(Vector3.right, mouseDragVector);

                if (angle < 45.0f)
                {
                    input = BeatPattern.Input.SlideRight;

                    Debug.Log("Side right");
                }

                angle = Vector3.Angle(Vector3.left, mouseDragVector);

                if (angle < 45.0f)
                {
                    input = BeatPattern.Input.SlideLeft;

                    Debug.Log("Side left");
                }

                angle = Vector3.Angle(Vector3.down, mouseDragVector);

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

            _currentCollider = null;
        }

        return input;
    }
}
