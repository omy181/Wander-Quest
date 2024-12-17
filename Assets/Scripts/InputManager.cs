using System.Collections;
using System.Collections.Generic;
using Holylib.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Singleton<InputManager>
{
    
    public bool IsTouchOnUI(){
        return Input.touchCount > 0 
            ? EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) 
            : HolyUtilities.isOnUI();
    }
    
    public bool TouchOutsideOfUI(){
        return Input.GetMouseButtonDown(0) && !IsTouchOnUI();
    }
}
