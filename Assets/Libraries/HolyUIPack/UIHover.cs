using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class UIHover : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private Vector3 _scale;

    [SerializeField] private float _modifier = 1.1f;
    private void Awake()
    {
        _scale = transform.localScale;
    }

    private void OnDisable()
    {
        transform.localScale = _scale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExit();
    }

    public void OnEnter()
    {
        transform.localScale = _scale;
        transform.LeanScale(_scale* _modifier, 0.1f).setIgnoreTimeScale(true);
    }

    public void OnExit()
    {
        transform.localScale = _scale * _modifier;
        transform.LeanScale(_scale * 1f, 0.1f).setIgnoreTimeScale(true);
    }
}
