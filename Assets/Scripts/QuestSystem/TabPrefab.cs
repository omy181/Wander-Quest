using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TabPrefab : MonoBehaviour
{
    [SerializeField] GameObject _questUI;
    [SerializeField] QuestType _questType;
  

    // set Active false to other quests -> destroy all quests -> create new quests

    public void OpenQuestList(){
        _questUI.GetComponent<QuestUI>().DestroyQuests();
        _questUI.GetComponent<QuestUI>().CreateQuestToType(_questUI.GetComponent<QuestUI>().GetQuestTypeList(_questType));
    }

    
}
