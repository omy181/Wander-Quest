using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    [SerializeField] private GameObject _navigationArrow;

    private void Start() {
        //_navigationArrow.SetActive(false);
        print("b");
    }
    public void AddAsTarget(){
        print("a");
        _navigationArrow.SetActive(true);
        _chooseLocation((float)38.37694276669464,(float)26.88509838113758);
        //_navigationArrow.SetDestination(float targetLatitude, float targetLongitude)
    }

    private void _chooseLocation(float targetLatitude, float targetLongitude){
        _navigationArrow.GetComponent<NavigationArrow>().SetDestination(targetLatitude, targetLongitude);
       
    }
}
