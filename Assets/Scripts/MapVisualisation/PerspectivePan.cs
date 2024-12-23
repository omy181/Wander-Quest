using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
public class PerspectivePan : MonoBehaviour
{
    private Vector3 _touchStart; 
    [SerializeField] private Camera _cam; 
    [SerializeField] private float _groundZ = 0f;
    private bool _touched = false;

    void Update()
    {
        if(InputManager.instance.TouchOutsideOfUI()){
            _touchStart = _getWorldPosition(_groundZ);
            _touched = true;
        }
        if(Input.GetMouseButton(0) && _touched){
            Vector3 direction = _touchStart - _getWorldPosition(_groundZ); 
            _cam.transform.position += direction;            
        }
        if(Input.GetMouseButtonUp(0)){
            _touched = false;
        }
        
    }

    private Vector3 _getWorldPosition(float zLevel){
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.down, new Vector3(0, 0, zLevel));
        if (ground.Raycast(ray, out float distance)){
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}
