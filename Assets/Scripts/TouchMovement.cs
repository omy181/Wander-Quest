using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TouchMovement : MonoBehaviour
{

    //private float touchSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved){
                //Vector2 touchDeltaPosition = touch.deltaPosition;
                //transform.Translate(-touchDeltaPosition.x * touchSpeed, -touchDeltaPosition.y * touchSpeed, 0);
            }
        }
    }
}
