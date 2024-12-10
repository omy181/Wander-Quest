using Holylib.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestCreator : MonoBehaviour
{
    [SerializeField] private TMP_InputField _questSearchBar;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !HolyUtilities.isOnUI())  /// TODO:  ADD TOUCH SUPPORT
        {
            OpenQuestSearchBar(false);
        }
    }
    public bool AddNewQuest()
    {
        var questQuerry = _questSearchBar.text;

        if (questQuerry.Equals(string.Empty) || questQuerry.Length <= 2)
        {
            print("Quest name should be longer!");
            return false;
        }

        // find the name of the quest
        string questName = questQuerry;

        QuestManager.instance.CreateNewQuest(questName, QuestType.MainQuest, questQuerry);
        return true;
    }

    public void OpenQuestSearchBar(bool state)
    {
        _questSearchBar.gameObject.SetActive(state);

        _questSearchBar.text = "";
    }

    public void OnAddNewQuestButtonPressed()
    {
        if (_questSearchBar.gameObject.activeSelf)
        {
            if(AddNewQuest())
            OpenQuestSearchBar(false);
        }
        else
        {
            OpenQuestSearchBar(true);
        }
    }

}
