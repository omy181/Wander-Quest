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


    void Start()
    {
        CreateQuestPrefab();
    }


}
