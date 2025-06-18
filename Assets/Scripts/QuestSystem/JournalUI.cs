using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JournalUI : MonoBehaviour
{
    [SerializeField] private GameObject _questPrefab;
    [SerializeField] private GameObject _placePrefab;
    [SerializeField] private Window _journalWindow;
    [SerializeField] private Window _leaderboardWindow;
    [SerializeField] private Transform _content;
    [SerializeField] private TMP_Text _header;
    [SerializeField] private Button _deleteQuestButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _selectQuestButton;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private TMP_Dropdown _sortDropdown;
    [SerializeField] private GameObject _sortParent;

    private Quest _currentQuest;
    private QuestType _lastOpenedQuestTab = QuestType.SponsoredQuest;
    private void Start()
    {
        _sortDropdown.AddOptions(new List<string>
        {
            "Alphabetical","Total Count","Traveled Count"
        });

        _sortDropdown.onValueChanged.AddListener(_onSortOptionChanged);
        _selectQuestButton.onClick.AddListener(_selectQuest);
        _leaderboardButton.onClick.AddListener(_leaderboardOfQuest);
        _selectQuestButton.gameObject.SetActive(false);
        _leaderboardButton.gameObject.SetActive(false);
    }

    private void _onSortOptionChanged(int option)
    {
        _sortList(option+1);
    }

    private void _selectQuest()
    {
        WindowManager.instance.OpenPreviousWindow();
        QuestSelector.instance.SelectQuest(_currentQuest);
    }

    private void _leaderboardOfQuest()
    {
        WindowManager.instance.OpenPreviousWindow();
        WindowManager.instance.OpenWindow(_leaderboardWindow);
        LeaderboardManager.instance.ShowQuest(_currentQuest);
    }
    public void ShowQuestsOfType(QuestType questType)
    {

        _lastOpenedQuestTab = questType;
        _header.text = questType.ToString()+"s";
        _deleteQuestButton.gameObject.SetActive(false);

        _backButton.onClick.RemoveAllListeners();
        _backButton.onClick.AddListener(WindowManager.instance.OpenPreviousWindow);

        _clearContents();

        List<Quest> orderedQuestList = QuestManager.instance.GetActiveQuests().OrderByDescending(p => p.TotalPlaceCount).ToList();
        orderedQuestList.ForEach(quest => {
            if(quest.QuestType == questType)
            {
                var questPrefab = Instantiate(_questPrefab, _content).GetComponent<QuestPrefab>();
                questPrefab.SetQuestData(quest,()=> _openQuestPanel(quest));
            }
            
        });
        _selectQuestButton.gameObject.SetActive(false);
        _leaderboardButton.gameObject.SetActive(false);
        _sortParent.gameObject.SetActive(true);
        _sortList(_sortDropdown.value+1);
    }

    private void _openQuestPanel(Quest quest)
    {
        _currentQuest = quest;
        _deleteQuestButton.gameObject.SetActive(true);
        _selectQuestButton.gameObject.SetActive(true);
        _leaderboardButton.gameObject.SetActive(true);
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
        });

        _sortList(0);
    }

    private void _sortList(int option)
    {
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < _content.childCount; i++)
            children.Add(_content.GetChild(i));

        switch (option)
        {
            case 0: // Distance
                children.Sort((a, b) =>
                {
                    var placeA = a.GetComponent<PlaceSlotUI>();
                    var placeB = b.GetComponent<PlaceSlotUI>();

                    return placeA.PlayerDistance.CompareTo(placeB.PlayerDistance);
                });
                break;

            case 1: // Alphabetical (assumes alphabetical order by name)
                children.Sort((a, b) =>
                {
                    var placeA = a.GetComponent<QuestPrefab>()._quest.Title;
                    var placeB = b.GetComponent<QuestPrefab>()._quest.Title;
                    return placeA.CompareTo(placeB);
                });
                break;
            case 2: // Total Travel Count
                children.Sort((a, b) =>
                {
                    var placeA = a.GetComponent<QuestPrefab>()._quest.TotalPlaceCount;
                    var placeB = b.GetComponent<QuestPrefab>()._quest.TotalPlaceCount;
                    return placeB.CompareTo(placeA);
                });
                break;

            case 3: // Traveled Count (puts traveled ones first)
                children.Sort((a, b) =>
                {
                    var traveledA = a.GetComponent<QuestPrefab>()._quest.TotalTraveledCount;
                    var traveledB = b.GetComponent<QuestPrefab>()._quest.TotalTraveledCount;
                    return traveledB.CompareTo(traveledA); // true TotalTraveledCount
                });
                break;
        }

        // Reorder in hierarchy
        for (int i = 0; i < children.Count; i++)
            children[i].SetSiblingIndex(i);
    }

    public Quest GetQuest(){
        return _currentQuest;
    }
 
    private void _goToPlace(QuestPlace place)
    {
        WindowManager.instance.OpenPreviousWindow();
        QuestSelector.instance.SelectQuest(_currentQuest);
        MapModeChanger.instance.FocusOnPlace(place);
    }

    private void _clearContents(){
        var list = new List<Transform>();
        foreach (Transform child in _content) {
            list.Add(child);
        }
        foreach(var c in list)
        {
            c.transform.SetParent(null);
            Destroy(c.gameObject);
        }
        
        _sortParent.gameObject.SetActive(false);
    }
    private void _refreshQuests()
    {
        ShowQuestsOfType(_lastOpenedQuestTab);
    }

    void Awake()
    {     
        _journalWindow.OnWindowActivated += _refreshQuests;
    }

}
