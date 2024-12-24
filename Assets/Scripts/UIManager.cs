using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public Action OnUICancel; // On clicked outside of UI or window changed

    private void Start()
    {
        WindowManager.instance.OnWindowChanged += OnUICancel;
    }

    private void Update()
    {
        if (InputManager.instance.DidTouchOutsideOfUI())
        {
            OnUICancel?.Invoke();
        }
    }
}
