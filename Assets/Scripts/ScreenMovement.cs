using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScreenMovement : MonoBehaviour
{
    [SerializeField] private MapCamera _cam;
    void Update()
    {
        _cam.Move(new Vector2(-Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical")));       
    }
}
