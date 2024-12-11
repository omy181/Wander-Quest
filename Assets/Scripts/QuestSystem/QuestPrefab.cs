using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private Button _questButton;


    public void SetQuestData(Quest quest,Action onClick)
    {
        _titleText.text = quest.Title;
        _progressText.text = $"Traveled {quest.TotalTraveledCount}\n Found {quest.TotalPlaceCount}" ;

        _questButton.onClick.AddListener(()=>onClick());

    }
}
