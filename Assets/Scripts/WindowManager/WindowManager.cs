public class WindowManager : Singleton<WindowManager>
{
    private Window _previousWindow;
    private Window _currentWindow;
    public void OpenWindow(Window window)
    {
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
