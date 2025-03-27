using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    [SerializeField] private GameObject _navigationArrow;
    private PlaceSlotUI _placeSlotUi;
    private void Start() {
        print("b");
        _placeSlotUi = GetComponent<PlaceSlotUI>();
    }
    public void AddAsTarget(){
        print("a");
        _navigationArrow.SetActive(true);

        var place = _placeSlotUi.ReturnInfo();
        print("lat: " + place.Location.latitude + " lon: " + place.Location.longitude);
        _chooseLocation((float)place.Location.latitude,(float)place.Location.longitude);
        //_chooseLocation((float)38.37694276669464,(float)26.88509838113758);
        //_navigationArrow.SetDestination(float targetLatitude, float targetLongitude)
    }

    private void _chooseLocation(float targetLatitude, float targetLongitude){
        _navigationArrow.GetComponent<NavigationArrow>().SetDestination(targetLatitude, targetLongitude);
       
    }



  
}
