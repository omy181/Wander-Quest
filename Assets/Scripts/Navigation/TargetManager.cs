using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TargetManager : Singleton<TargetManager>
{
    [SerializeField] private NavigationArrow _navigationArrow;
    [SerializeField] private Button _cancelTargetButton;
    private QuestPlace _currentTarget;

    void Start()
    {
        _cancelTargetButton.onClick.AddListener(CancelTarget);
        _cancelTargetButton.gameObject.SetActive(false);
        _navigationArrow.gameObject.SetActive(false);
    }

    public void SetTarget(QuestPlace place){
        _currentTarget = place;
        _cancelTargetButton.gameObject.SetActive(true);
        _navigationArrow.gameObject.SetActive(true);
        _navigationArrow.SetDestination((float)_currentTarget.Location.latitude,(float)_currentTarget.Location.longitude,place);
    }

    public void CancelTarget(){
        _navigationArrow.gameObject.SetActive(false);
        _currentTarget = null;
        _cancelTargetButton.gameObject.SetActive(false);
    }
}
