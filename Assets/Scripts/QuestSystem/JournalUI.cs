using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalUI : MonoBehaviour
{
    [SerializeField] private GameObject _questPrefab;
    [SerializeField] private GameObject _placePrefab;
    [SerializeField] private Window _journalWindow;
    [SerializeField] private Transform _content;
    [SerializeField] private TMP_Text _header;
    [SerializeField] private Button _deleteQuestButton;
    [SerializeField] private Button _backButton;

    public void ShowQuestsOfType(QuestType questType)
    {
        _header.text = questType.ToString()+"s";
        _deleteQuestButton.gameObject.SetActive(false);

        _backButton.onClick.RemoveAllListeners();
        _backButton.onClick.AddListener(WindowManager.instance.OpenPreviousWindow);

        _clearContents();

        QuestManager.instance.GetActiveQuests().ForEach(quest => {
            if(quest.QuestType == questType)
            {
                var questPrefab = Instantiate(_questPrefab, _content).GetComponent<QuestPrefab>();
                questPrefab.SetQuestData(quest,()=> _openQuestPanel(quest));
            }
            
        });
    }

    private void _openQuestPanel(Quest quest)
    {
        _deleteQuestButton.gameObject.SetActive(true);
        _deleteQuestButton.onClick.RemoveAllListeners();
        _deleteQuestButton.onClick.AddListener(()=>_deleteQuest(quest));
        _header.text = quest.Title;
        _backButton.onClick.RemoveAllListeners();
        _backButton.onClick.AddListener(()=> ShowQuestsOfType(quest.QuestType));
        ListPlaces(quest);
    }

    private void _deleteQuest(Quest quest)
    {
        QuestManager.instance.DeleteQuest(quest);
        ShowQuestsOfType(quest.QuestType);
    }

    public void ListPlaces(Quest quest)
    {
        _clearContents();
        quest.GetPlaces().ForEach(place => {
            var placePrefab = Instantiate(_placePrefab, _content).GetComponent<PlaceSlotUI>();
            placePrefab.SetPlaceData(place,null);
        });
    }

    private void _clearContents(){
        foreach (Transform child in _content) {
            Destroy(child.gameObject);
        }
    }
    private void _refreshQuests()
    {
        ShowQuestsOfType(QuestType.MainQuest);
    }

    void Awake()
    {     
        _journalWindow.OnWindowActivated += _refreshQuests;

        var q = QuestManager.instance.CreateNewQuest(QuestType.MainQuest,"a-101");
        q.AddPlace(new QuestPlace(GPS.instance.GetLastGPSLocation(),"A-101","f",new Address("","","Guzelbahce","Izmir","Turkey"),true));
        q.AddPlace(new QuestPlace(GPS.instance.GetLastGPSLocation(), "A-101 Mega", "3", new Address("", "", "Maltepe", "Izmir", "Turkey"), false));
        q.AddPlace(new QuestPlace(GPS.instance.GetLastGPSLocation(), "A-101 Epic ", "4", new Address("", "", "Narlidere", "Izmir", "Turkey"), true));
    }

    
}
