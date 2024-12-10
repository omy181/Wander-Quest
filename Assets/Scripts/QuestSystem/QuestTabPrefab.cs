using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTabPrefab : MonoBehaviour
{
    [SerializeField] private JournalUI _questUI;
    [SerializeField] private QuestType _questType;
  

    public void OpenQuestList(){
        _questUI.DestroyQuests();
        _questUI.CreateQuestPrefabs(_questUI.GetQuestTypeList(_questType));
    }

    
}
