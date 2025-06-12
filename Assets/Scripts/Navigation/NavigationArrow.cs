using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class NavigationArrow : MonoBehaviour
{
    [SerializeField] private TMP_Text _destinyText;
    [SerializeField] private Button _destinyFocusButton;
    [SerializeField] private GameObject _arrow;
    private float _targetLatitude;
    private float _targetLongitude;

    private QuestPlace _activelastPlace;
    private float _smoothedAngle;
    private float _smoothingFactor = 0.1f;

    private void Start()
    {
        _destinyFocusButton.onClick.AddListener(_focus);
        StartCoroutine(_initializeSensorsCoroutine());
    }

    private IEnumerator _initializeSensorsCoroutine()
    {
        yield return StartCoroutine(Gyroscope.InitializeSensors());
    }

    private void _focus()
    {
        MapModeChanger.instance.FocusOnPlace(_activelastPlace);
    }
    private void _rotateArrow(){
        float angle = Gyroscope.FindAngleToTarget(_targetLatitude, _targetLongitude);
        if (angle == -1) return;
        _smoothedAngle = Mathf.LerpAngle(_smoothedAngle, angle, _smoothingFactor);
        _arrow.transform.localRotation = Quaternion.Euler(0, 0,-_smoothedAngle);
    }

    public void SetDestination(float targetLatitude, float targetLongitude, QuestPlace activelastPlace)
    {
        _activelastPlace = activelastPlace;
        _targetLatitude = targetLatitude;
        _targetLongitude = targetLongitude;
        _destinyText.text = _activelastPlace.Name;
        
        _smoothedAngle = 0f;
    }

    void Update()
    {
        _rotateArrow();
    }
}
