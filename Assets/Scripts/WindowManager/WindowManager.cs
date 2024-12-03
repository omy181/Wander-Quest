using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : Singleton<WindowManager>
{
    private Window _previousWindow;
    private Window _currentWindow;
    public void OpenWindow(Window window)
    {
        _previousWindow = _currentWindow;
        _currentWindow = window;

        _previousWindow?.Deactivate();
        _currentWindow.Activate();
    }
}
