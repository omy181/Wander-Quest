using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject _questPrefab;

    public List<Quest> GetQuestTypeList(QuestType questType){
        List<Quest> questsList = new List<Quest>();
        QuestManager.instance.GetActiveQuests().ForEach(quest => {
            if(quest.QuestType == questType){
                questsList.Add(quest);
            }
        });
        return questsList;
    }

    public void CreateQuestToType(List<Quest> quests){
        quests.ForEach(quest => {
            GameObject questPrefab = Instantiate(_questPrefab, transform);
            QuestPrefab questPrefabComponent = questPrefab.GetComponent<QuestPrefab>();
            questPrefabComponent.SetQuestData(quest.Title ,  $"{quest.Progress}");
        });
    }
    
    public void DestroyQuests(){
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }


    private void _showPlaceholderQuests(){
        Quest q1 = QuestManager.instance.CreateNewQuest("q1", QuestType.MainQuest, "aa");
        QuestManager.instance.AddPlaceToQuest(q1, new QuestPlace(GPS.instance.GetLastGPSLocation(), "Migros","12"));
        QuestManager.instance.CreateNewQuest("q2", QuestType.MainQuest, "cc");
        QuestManager.instance.CreateNewQuest("q3", QuestType.DailyQuest, "bb");
        QuestManager.instance.CreateNewQuest("q4", QuestType.DailyQuest, "dd");  
        QuestManager.instance.CreateNewQuest("q5", QuestType.SideQuest, "ee");
    }

    void Start()
    {
       
        _showPlaceholderQuests();

        DestroyQuests();
        CreateQuestToType(GetQuestTypeList(QuestType.MainQuest));

    }

    
}
