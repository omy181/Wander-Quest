using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinObject : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _name;
    public void Initialize(Place place)
    {
        _name.text = place.displayName.text;
        transform.name = _name.text;
    }
}
