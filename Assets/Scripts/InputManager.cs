using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holylib.Utilities;

public class InputManager : Singleton<InputManager>
{
    public bool OutsideOfUI(){
        return Input.GetMouseButtonDown(0) && !HolyUtilities.isOnUI();
    }
}
