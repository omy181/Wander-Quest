using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenMovement : MonoBehaviour
{
    [SerializeField] private MapShower _mapShower;
    void Start()
    {
        
    }


    void Update()
    {
        if(Input.mouseScrollDelta.y > 0.1f)
        {
            _zoom(1);
        } else if(Input.mouseScrollDelta.y < -0.1f)
        {
            _zoom(-1);
        }
    }

    private void _zoom(int amount)
    {
        _mapShower.zoom = Mathf.Clamp(_mapShower.zoom+ amount * 0.1f, 0,14);
    }
}
