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
    [SerializeField] private Image _background;
    [SerializeField] private GameObject _sponsorIcon;

    public Quest _quest;
    public void SetQuestData(Quest quest,Action onClick)
    {
        _quest = quest;
        _titleText.text = quest.Title;
        _progressText.text = $"Traveled {quest.TotalTraveledCount}\n Found {quest.TotalPlaceCount}" ;

        _questButton.onClick.AddListener(()=>onClick());

        if(quest.QuestType == QuestType.SideQuest)
        {
            _sponsorIcon.SetActive(true);
            _background.color = Color.Lerp(_background.color,Color.yellow,0.5f);
        }
        else
        {
            _sponsorIcon.SetActive(false);
        }

        if (quest.QuestType == QuestType.DailyQuest)
        {
            _background.color = Color.Lerp(_background.color, Color.green, 0.5f);
        }
    }
}
