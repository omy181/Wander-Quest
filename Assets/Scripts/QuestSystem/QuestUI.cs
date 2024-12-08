using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject _questPrefab;

    private void InfoQuest(){
        QuestManager.instance.GetActiveQuests().ForEach(quest => {
            Debug.Log(quest.Title);
            Debug.Log(quest.Progress);
            Debug.Log(quest.QuestType);
            quest.GetPlaces().ForEach(place => {
                Debug.Log(place.Name);
            });
        });
    }

    private void CreateQuestPrefab(){
        //QuestManager.instance.GetActiveQuests().ForEach(quest => {
            GameObject questPrefab = Instantiate(_questPrefab, transform);
            //questPrefab.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Title";//quest.Title;
            // questPrefab.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "50/100";//quest.Progress;
            QuestPrefab questPrefabComponent = questPrefab.GetComponent<QuestPrefab>();
            questPrefabComponent.SetQuestData("Title",  "50/100");
            //questPrefabComponent.SetQuestData(quest.Title ,  $"{quest.Progress}/{quest.GetPlaces().Count}");
        //});
    }

    private (List<Quest> MainQuests, List<Quest> SideQuests, List<Quest> DailyQuests) QuestTypeList(){
        List<Quest> MainQuests = new List<Quest>();
        List<Quest> SideQuests = new List<Quest>();
        List<Quest> DailyQuests = new List<Quest>();

        QuestManager.instance.GetActiveQuests().ForEach(quest => {
            if(quest.QuestType == QuestType.MainQuest){
                MainQuests.Add(quest);
            }
            else if(quest.QuestType == QuestType.SideQuest){
                SideQuests.Add(quest);
            }
            else if(quest.QuestType == QuestType.DailyQuest){
                DailyQuests.Add(quest);
            }  
        });
        return (MainQuests, SideQuests, DailyQuests);
    }

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
            questPrefabComponent.SetQuestData(quest.Title ,  $"{quest.Progress}/{quest.GetPlaces().Count}");
        });
    }
 


    void Start()
    {
        CreateQuestPrefab();
        CreateQuestToType(QuestTypeList().MainQuests);
        CreateQuestToType(GetQuestTypeList(QuestType.MainQuest));
        GetQuestTypeList(QuestType.SideQuest);
    }


}
