using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    
    private PlaceSlotUI _placeSlotUi;
    private void Start() {
        _placeSlotUi = GetComponent<PlaceSlotUI>();
    }
    public void AddAsTarget(){
        var place = _placeSlotUi.ReturnInfo();
        TargetManager.instance.SetTarget(place);
    }
  
}
