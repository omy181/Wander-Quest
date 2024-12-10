using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabPrefab : MonoBehaviour
{
    [SerializeField] private GameObject _questUI;
    [SerializeField] private QuestType _questType;
  

    public void OpenQuestList(){
        _questUI.GetComponent<QuestUI>().DestroyQuests();
        _questUI.GetComponent<QuestUI>().CreateQuestToType(_questUI.GetComponent<QuestUI>().GetQuestTypeList(_questType));
    }

    
}
