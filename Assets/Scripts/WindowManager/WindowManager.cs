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
        if (_previousWindow == _currentWindow) return;

        _previousWindow = _currentWindow;
        _currentWindow = window;

        _previousWindow?.Deactivate();
        _currentWindow?.Activate();
    }

    public void OpenPreviousWindow()
    {
        OpenWindow(_previousWindow);
    }
}
