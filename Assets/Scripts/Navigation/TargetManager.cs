using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TargetManager : Singleton<TargetManager>
{
    [SerializeField] private GameObject _navigationArrow;
    [SerializeField] private Button _cancelTargetButton;

    void Start()
    {
        _cancelTargetButton.onClick.AddListener(CancelTarget);
    }

    public void SetTarget(QuestPlace place){
        print("c");
        _navigationArrow.SetActive(true);
        print("lat: " + place.Location.latitude + " lon: " + place.Location.longitude);
        _navigationArrow.GetComponent<NavigationArrow>().SetDestination((float)place.Location.latitude,(float)place.Location.longitude);
    }

    public void CancelTarget(){
        print("e");
        _navigationArrow.SetActive(false);
    }
}
