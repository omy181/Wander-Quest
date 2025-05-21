using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class NavigationArrow : MonoBehaviour
{
    [SerializeField] private TMP_Text _destinyText;
    [SerializeField] private Button _destinyFocusButton;
    [SerializeField] private Button _calibrateButton;
    [SerializeField] private GameObject _arrow;
    private float _targetLatitude;
    private float _targetLongitude;

    private QuestPlace _activelastPlace;

    private void Start()
    {
        _destinyFocusButton.onClick.AddListener(_focus);
        _calibrateButton.onClick.AddListener(() => Gyroscope.CalibrateGyro());
    }

    private void _focus()
    {
        MapModeChanger.instance.FocusOnPlace(_activelastPlace);
    }
    private void _rotateArrow(){
        float angle = Gyroscope.FindAngleToTarget(_targetLatitude, _targetLongitude);
        if (angle == -1) return;
        _arrow.transform.localRotation = Quaternion.Euler(0,0,-angle);
    }

    public void SetDestination(float targetLatitude, float targetLongitude,QuestPlace activelastPlace)
    {
        _activelastPlace= activelastPlace;
        _targetLatitude = targetLatitude;
        _targetLongitude = targetLongitude;
        _destinyText.text = _activelastPlace.Name;
    }

    void Update()
    {
        _rotateArrow();
    }
}
