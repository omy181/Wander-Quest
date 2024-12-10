using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalUI : MonoBehaviour
{
    [SerializeField] private GameObject _questPrefab;
    [SerializeField] private Window _journalWindow;
    [SerializeField] private Transform _content;
    public List<Quest> GetQuestTypeList(QuestType questType){
        List<Quest> questsList = new List<Quest>();
        QuestManager.instance.GetActiveQuests().ForEach(quest => {
            if(quest.QuestType == questType){
                questsList.Add(quest);
            }
        });
        return questsList;
    }

    public void CreateQuestPrefabs(List<Quest> quests){
        quests.ForEach(quest => {
            GameObject questPrefab = Instantiate(_questPrefab, _content);
            QuestPrefab questPrefabComponent = questPrefab.GetComponent<QuestPrefab>();
            questPrefabComponent.SetQuestData(quest.Title ,  $"{quest.Progress}");
        });
    }
    
    public void DestroyQuests(){
        foreach (Transform child in _content) {
            Destroy(child.gameObject);
        }
    }


    private void _addPlaceholderQuests(){
        Quest q1 = QuestManager.instance.CreateNewQuest("q1", QuestType.MainQuest, "aa");
        QuestManager.instance.AddPlaceToQuest(q1, new QuestPlace(GPS.instance.GetLastGPSLocation(), "Migros","12"));
        QuestManager.instance.CreateNewQuest("q2", QuestType.MainQuest, "cc");
        QuestManager.instance.CreateNewQuest("q3", QuestType.DailyQuest, "bb");
        QuestManager.instance.CreateNewQuest("q4", QuestType.DailyQuest, "dd");  
        QuestManager.instance.CreateNewQuest("q5", QuestType.SideQuest, "ee");
    }

    private void _refreshQuests()
    {
        DestroyQuests();
        CreateQuestPrefabs(GetQuestTypeList(QuestType.MainQuest));
    }

    void Awake()
    {     
        _addPlaceholderQuests();
        _journalWindow.OnWindowActivated += _refreshQuests;
    }

    
}
