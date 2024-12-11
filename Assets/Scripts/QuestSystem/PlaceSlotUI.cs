using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaceSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _adressText;
    [SerializeField] private Button _placeButton;
    [SerializeField] private Image _background;

    public void SetPlaceData(QuestPlace place, Action onClick)
    {
        _titleText.text = place.Name;
        _adressText.text = $"{place.Address.Locality}\n{place.Address.Region}\n{place.Address.Country}";

        _background.color = place.IsTraveled ? Color.green * 1.2f : Color.red * 1.2f;

        if (onClick != null)
        _placeButton.onClick.AddListener(() => onClick());

    }
}
