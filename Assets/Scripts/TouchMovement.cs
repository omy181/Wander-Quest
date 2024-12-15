using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TouchMovement : MonoBehaviour
{

    private Vector3 startTouchPosition;
    private Vector3 currentTouchPosition;
    private System.DateTime currentTime;
    private System.DateTime startTime;
    private bool isTouching = false;
    public Transform[] layers; 
    public float[] parallaxFactors; 

    public float movementSpeed = 1000f;
    void Update(){
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began){
                isTouching = true;
                startTouchPosition = touch.position;
                startTime = System.DateTime.Now;
            }

            if(touch.phase == TouchPhase.Moved && isTouching){ 
                currentTouchPosition = touch.position;
                
                Vector3 deltaPosition = (currentTouchPosition - startTouchPosition);
                //transform.Translate(new Vector3(deltaPosition.x, 0, deltaPosition.z), Space.World);
                transform.Translate(deltaPosition, Space.World);
                for (int i = 0; i < layers.Length; i++){
                    Vector3 parallaxMovement = new Vector3(deltaPosition.x,  0, deltaPosition.y) * parallaxFactors[i];
                    layers[i].Translate(parallaxMovement, Space.World);
                }
                //startTouchPosition = currentTouchPosition;
                startTouchPosition = touch.position;
                Debug.Log($"Delta Position: {deltaPosition}");
                
                
                /*
                currentTime = System.DateTime.Now;
                float timeDifference = (float)(currentTime - startTime).TotalSeconds;
                Vector3 velocity = (currentTouchPosition - startTouchPosition) / timeDifference;
                Debug.Log($"Velocity: {velocity}");
                transform.position = new Vector3(velocity.x,0,velocity.y); 
                */

                
                
                /*
                    Vector2 direction = touch.deltaPosition.normalized; 
                    float speed = touch.deltaPosition.magnitude;
                    transform.position += new Vector3(direction.x * speed, 0, direction.y * speed);
                */
                

            }
        }
    }
}