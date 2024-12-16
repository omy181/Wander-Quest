using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _usernameInputField;
    [SerializeField] private Window _loginWindow;
    [SerializeField] private Button _loginButton;

    private void Awake()
    {
        _loginButton.onClick.AddListener(()=>OnLoginButtonPressed());
    }
    public string GetUserName()
    {
        return _usernameInputField.text;
    }

    public void ShowLoginPanel(bool state)
    {
        if (state)
        {
            WindowManager.instance.OpenWindow(_loginWindow);
        }
        else
        {
            WindowManager.instance.OpenMainWindow();
        }
        
    }

    public Action OnLoginButtonPressed;
}
