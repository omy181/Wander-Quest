using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationArrow : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    private float _targetLatitude;
    private float _targetLongitude;

    private void Start() {
        SetDestination(38.45180248754061f, 27.201164958903657f); // for debug
    }
    private void _rotateArrow(){
        float angle = Gyroscope.FindAngleToTarget(_targetLatitude, _targetLongitude);
        _arrow.transform.localRotation = Quaternion.Euler(0,0,angle);
    }

    public void SetDestination(float targetLatitude, float targetLongitude){
        _targetLatitude = targetLatitude;
        _targetLongitude = targetLongitude;
    }

    void Update()
    {
        _rotateArrow();
    }
}
