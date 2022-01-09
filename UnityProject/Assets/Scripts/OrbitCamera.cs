using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class OrbitCamera : MonoBehaviour
{
    [SerializeField]
    Transform focus = default;

    [SerializeField, Range(1f, 40f)]
    float distance = 5f;

    [SerializeField, Min(0f)]
    float focusRadius = 1f;

    [SerializeField]
    float maxCameraRotation;

    Vector3 focusPoint;
    float ballHeight;
    float Angle;

    GameObject gameState;

    void Awake()
    {
        focusPoint = focus.position;
        gameState = GameObject.Find("Game State");
    }

    void LateUpdate()
    {
        if (gameState.GetComponent<GameState>().gameAlive)
        {
            UpdateFocusPoint();
            Vector3 lookDirection = transform.forward;
            transform.localPosition = focusPoint - lookDirection * distance;

            ballHeight = focus.localPosition.y;

            if (ballHeight < 0f)
            {
                //Angle = Mathf.Min(1f, Mathf.Abs(ballHeight)) * 45 + 45;
                Angle = 90f;
            }
            else
            {
                Angle = 30f;
            }

            float currentAngle = transform.localEulerAngles.x;
            currentAngle = Mathf.MoveTowards(currentAngle, Angle, maxCameraRotation * Time.deltaTime);

            transform.localEulerAngles = new Vector3(currentAngle, 0f, 0f);
        }
    }

    void UpdateFocusPoint()
    {
        Vector3 targetPoint = focus.position;

        if (focusRadius > 0f)
        {
            float distance = Vector3.Distance(targetPoint, focusPoint);

            if (distance > focusRadius)
                focusPoint = Vector3.Lerp(targetPoint, focusPoint, focusRadius / distance);
        }
        else
            focusPoint = targetPoint;
    }
}
