using Holylib.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSelector : MonoBehaviour
{
    [SerializeField] private GameObject _questList;
    [SerializeField] private Transform _questContent;
    [SerializeField] private GameObject _questPrefab;
    [SerializeField] private TMP_Text _activeQuestText;
    [SerializeField] private TMP_Text _activeQuestProgressText;

    private Quest _activeQuest;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !HolyUtilities.isOnUI())  /// TODO:  ADD TOUCH SUPPORT
        {
            ShowQuestSelector(false);
        }
    }

    public void ShowQuestSelector(bool state)
    {
        if (state)
        {
            _questList.SetActive(true);
            _listQuests();
        }
        else
        {
            _questList.SetActive(false);
        }
    }

    private void _listQuests()
    {
        foreach (Transform q in _questContent)
        {
            Destroy(q.gameObject);
        }

        QuestManager.instance.GetActiveQuests().ForEach(quest => {
            var questPrefab = Instantiate(_questPrefab, _questContent).GetComponent<QuestPrefab>();
            questPrefab.SetQuestData(quest,()=> _selectQuest(quest));
        });
    }

    private void _selectQuest(Quest quest)
    {
        _activeQuestText.text = quest.Title;
        _activeQuestProgressText.text = quest.TotalTraveledCount.ToString();
        _activeQuest = quest;
        ShowQuestSelector(false);
    }
}
