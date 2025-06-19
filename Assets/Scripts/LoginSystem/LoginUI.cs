using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private Window _loginWindow;
    [SerializeField] private Window _loadingWindow;

    [Header("Log in")]
    [SerializeField] private GameObject _loginContainer;
    [SerializeField] private TMP_InputField _logInUsernameInputField;
    [SerializeField] private TMP_InputField _logInPassordInputField;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _testloginButton;
    [SerializeField] private Button _goToSignUpButton;

    [Header("Sign up")]
    [SerializeField] private GameObject _signupContainer;
    [SerializeField] private TMP_InputField _signUpUsernameInputField;
    [SerializeField] private TMP_InputField _signUpPassordInputField;
    [SerializeField] private Button _signUpButton;
    [SerializeField] private Button _goToLoginButton;

    [Space]
    [SerializeField] private TMP_Text _warningText;


    private void Awake()
    {
        _testloginButton.onClick.AddListener(()=>OnTestLoginButtonPressed());
        _loginButton.onClick.AddListener(()=> { GiveWarning(""); OnLoginButtonPressed();});
        _signUpButton.onClick.AddListener(()=> { GiveWarning(""); OnSignUpButtonPressed(); });

        _goToLoginButton.onClick.AddListener(()=> _showLoginContainer(true));
        _goToSignUpButton.onClick.AddListener(() => _showLoginContainer(false));

        GiveWarning("");
    }

    private void Start()
    {
        ShowLoginWindow(true);
    }
    public string GetLogInUserName()
    {
        return _logInUsernameInputField.text;
    }

    public string GetLogInPassword()
    {
        return _logInPassordInputField.text;
    }
    public string GetSignUpUserName()
    {
        return _signUpUsernameInputField.text;
    }

    public string GetSignUpPassword()
    {
        return _signUpPassordInputField.text;
    }

    public void ShowLoginWindow(bool state)
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

    public void ShowLoadingScreen(bool state)
    {
        if (state)
        {
            _loadingWindow.Activate();
        }
        else
        {
            _loadingWindow.Deactivate();
        }
    }

    private void _showLoginContainer(bool state)
    {
        _loginContainer.SetActive(state);
        _signupContainer.SetActive(!state);

        GiveWarning("");
    }

    // Bu fonksiyonu: This user already exists, username can't be this short falan diye kullaniciya uyari vermek icin kullan
    public void GiveWarning(string warning)
    {
        _warningText.text = warning;
    }

    public Action OnTestLoginButtonPressed;
    public Action OnLoginButtonPressed;
    public Action OnSignUpButtonPressed;
}
