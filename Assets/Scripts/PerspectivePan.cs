using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePan : MonoBehaviour
{
    private Vector3 _touchStart; 
    public Camera cam; 
    public float groundZ = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _touchStart = _getWorldPosition(groundZ); 
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = _touchStart - _getWorldPosition(groundZ); 
            cam.transform.position += direction;
        }
    }

    private Vector3 _getWorldPosition(float zLevel)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(cam.transform.forward, new Vector3(0, 0, zLevel));
        if (ground.Raycast(ray, out float distance)){
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}
