using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singleton<InputManager>
{
    
    public bool IsTouchOnUI(){
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }
    public bool TouchOutsideOfUI(){
        return Input.GetMouseButtonDown(0) && !IsTouchOnUI();
    }
}
