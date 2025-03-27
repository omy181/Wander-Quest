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

    private Quest _currentQuest;

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
        _currentQuest = quest;
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
            placePrefab.SetPlaceData(place,()=> _goToPlace(place));
            //ReturnInfo(quest, placePrefab._titleText);
            //GetQuest(quest);
        });
    }

    public Quest GetQuest(){
        return _currentQuest;
    }

/*
     public QuestPlace ReturnInfo(Quest quest, TextMeshProUGUI textMeshPro)
    {
        foreach (var place in quest.GetPlaces())
        {
            if (place.Name == textMeshPro.text)
            {
                return place;
            }
        }
        return null;
    }
*/    
    private void _goToPlace(QuestPlace place)
    {
        WindowManager.instance.OpenPreviousWindow();
        MapModeChanger.instance.FocusOnPlace(place);
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
    }

    
}
