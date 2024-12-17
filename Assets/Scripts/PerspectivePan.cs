using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
public class PerspectivePan : MonoBehaviour
{
    private Vector3 _touchStart; 
    public Camera cam; 
    public float groundZ = 0f;
    private bool _touched = false;

    void Update()
    {
        if(InputManager.instance.TouchOutsideOfUI()){
            _touchStart = _getWorldPosition(groundZ);
            _touched = true;
        }
        if(Input.GetMouseButton(0) && _touched){
            Vector3 direction = _touchStart - _getWorldPosition(groundZ); 
            cam.transform.position += direction;            
        }
        if(Input.GetMouseButtonUp(0)){
            _touched = false;
        }
        
    }

    private Vector3 _getWorldPosition(float zLevel){
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.down, new Vector3(0, 0, zLevel));
        if (ground.Raycast(ray, out float distance)){
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}
