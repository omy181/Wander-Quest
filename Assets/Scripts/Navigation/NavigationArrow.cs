using UnityEngine;


public class NavigationArrow : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    private float _targetLatitude;
    private float _targetLongitude;


    private void Start() {
        SetDestination(90, 90); // always point at north
    }
    private void _rotateArrow(){
        float angle = Gyroscope.FindAngleToTarget(_targetLatitude, _targetLongitude);
        if (angle == -1) return;
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
