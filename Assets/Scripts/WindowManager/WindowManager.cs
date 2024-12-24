using System;
using UnityEngine;
public class WindowManager : Singleton<WindowManager>
{
    private Window _previousWindow;
    private Window _currentWindow;
    [SerializeField] private Window _mainWindow;

    public void OpenMainWindow()
    {
        OpenWindow(_mainWindow);
    }
    public void OpenWindow(Window window)
    {
        if (_currentWindow == window) return;

        _previousWindow = _currentWindow;
        _currentWindow = window;

        _previousWindow?.Deactivate();
        _currentWindow?.Activate();

        OnWindowChanged?.Invoke();
    }

    public void OpenPreviousWindow()
    {
        OpenWindow(_previousWindow);
    }

    public Action OnWindowChanged;
}
