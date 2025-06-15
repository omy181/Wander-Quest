using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class LeaderboardManager : Singleton<LeaderboardManager>
{
    #region Logic

    [SerializeField] private Window _leaderboardWindow;

    private void Start()
    {
        _questDropDown.onValueChanged.AddListener(_selectQuest);
        _leaderboardWindow.OnWindowActivated += InitializeLeaderboard;
    }
    public void InitializeLeaderboard()
    {
        _clearContents();
        _setQuestDropdown();
        _selectQuest(QuestSelector.instance._activeQuest);
        _questDropDown.value = _questDropDown.options.FindIndex(o=>o.text == QuestSelector.instance._activeQuest.Title);
    }

    private void _selectQuest(int questIndex)
    {
        StartCoroutine(QuestManager.instance.LoadAQuestOfAllUsers(QuestManager.instance.GetActiveQuests()[questIndex], _refreshQuestList));
    }

    private void _selectQuest(Quest quest)
    {
        if(quest != null)
        {
            StartCoroutine(QuestManager.instance.LoadAQuestOfAllUsers(quest, _refreshQuestList));
        }
        
    }

    private void _refreshQuestList(List<QuestLeaderBoard> quests)
    {
        _listQuests(quests);
    }
    #endregion
    #region UI
    [Header("UI")]
    [SerializeField] private TMP_Dropdown _questDropDown;
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _leaderboardQuestPrefab;

    private void _setQuestDropdown()
    {
        _questDropDown.ClearOptions();
        var nameList = new List<string>();
        QuestManager.instance.GetActiveQuests().ForEach(q=> nameList.Add(q.Title));
        _questDropDown.AddOptions(nameList);
    }
    private void _clearContents()
    {
        foreach (Transform child in _content)
        {
            Destroy(child.gameObject);
        }
    }

    private void _listQuests(List<QuestLeaderBoard> questLBs)
    {
        _clearContents();

        int index = 0;
        questLBs.OrderByDescending(q=>q.Quest.TotalTraveledCount).ToList().ForEach(questLB => {
            index++;
            var leaderboardQuestPrefab = Instantiate(_leaderboardQuestPrefab, _content).GetComponent<LeaderboardQuestElement>();
            leaderboardQuestPrefab.Initialize(questLB.UserName,index, questLB.Quest.TotalTraveledCount);
        });
    }
    #endregion
}
