using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    
    private PlaceSlotUI _placeSlotUi;
    private void Start() {
        print("b");
        _placeSlotUi = GetComponent<PlaceSlotUI>();
    }
    public void AddAsTarget(){
        print("a");
        var place = _placeSlotUi.ReturnInfo();
        TargetManager.instance.SetTarget(place);
        //SetTargetmangaerfunc(place);
        print("lat1: " + place.Location.latitude + " lon1: " + place.Location.longitude);
        //_chooseLocation((float)place.Location.latitude,(float)place.Location.longitude);
        //_chooseLocation((float)38.37694276669464,(float)26.88509838113758);
        //_navigationArrow.SetDestination(float targetLatitude, float targetLongitude)
    }

    public void CloseTarget(){
        print("d");
        //TargetManager.instance.CancelTarget();
    }

// target manager
    private void _chooseLocation(float targetLatitude, float targetLongitude){ // questplace
        //_navigationArrow.GetComponent<NavigationArrow>().SetDestination(targetLatitude, targetLongitude);
       
    }



  
}
