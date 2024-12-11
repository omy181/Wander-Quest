using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TouchMovement : MonoBehaviour
{

    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool isTouching = false;

    public float movementSpeed = 1000f;
    public float sensitivity = 100f;

    void Update(){
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began){
                isTouching = true;
                startTouchPosition = touch.position;
            }
            if(touch.phase == TouchPhase.Moved && isTouching){ 
                currentTouchPosition = touch.position;
                Vector2 deltaPosition = (currentTouchPosition - startTouchPosition) * sensitivity;
                transform.Translate(new Vector3(deltaPosition.x, 0, deltaPosition.y) * movementSpeed, Space.World);
                startTouchPosition = currentTouchPosition;
                Debug.Log($"Delta Position: {deltaPosition}");

            }
        }
    }
}