using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;
    [Space]
    [SerializeField] private Sprite[] _icons;
    [SerializeField] private string[] _labels;
    private Action[] _actions;

    private int _stateCount;
    private int _currentState;
    private int _getcurrentState { get=> _currentState; set {
            _currentState = value % _stateCount; 
            
            _image.sprite = _icons[_currentState];
            _text.text = _labels[_currentState];
            _actions[_currentState]?.Invoke();
        } }

    public void Initialize(Action[] actions)
    {
        _stateCount = actions.Length;
        _actions = actions;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(_switch);
    }

    private void _switch()
    {
        _getcurrentState++;
    }
}
