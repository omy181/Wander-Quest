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
    }

    public void SetTarget(QuestPlace place){
        _currentTarget = place;
        _navigationArrow.gameObject.SetActive(true);
        _navigationArrow.SetDestination((float)_currentTarget.Location.latitude,(float)_currentTarget.Location.longitude);
    }

    public void CancelTarget(){
        _navigationArrow.gameObject.SetActive(false);
        _currentTarget = null;
    }
}
