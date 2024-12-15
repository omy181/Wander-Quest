using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public Action OnWindowActivated;
    public void Activate()
    {
        gameObject.SetActive(true);
        OnWindowActivated?.Invoke();
    }

    public Action OnWindowDeactivated;
    public void Deactivate()
    {
        gameObject.SetActive(false);
        OnWindowDeactivated?.Invoke();
    }
}
