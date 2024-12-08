using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject _questPrefab;
    public bool a = false;

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
            GameObject questPrefab = Instantiate(_questPrefab, transform);
            QuestPrefab questPrefabComponent = questPrefab.GetComponent<QuestPrefab>();
            questPrefabComponent.SetQuestData("Title",  "50/100");
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
    
    public void DestroyQuests(){
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }


    void Start()
    {
        CreateQuestPrefab();
    }

    void Update()
    {
        if(a){
            DestroyQuests(); // sahnede quest prefab kalıbını nasıl oluşturmalıyım (çünkü hepsi siliniyor)
        }
    }
    
}
