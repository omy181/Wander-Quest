using Holylib.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestCreator : MonoBehaviour
{
    [SerializeField] private TMP_InputField _questSearchBar;
    [SerializeField] private QuestSelector _questSelector;

  /*  private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !HolyUtilities.isOnUI())  /// TODO:  ADD TOUCH SUPPORT
        {
            OpenQuestSearchBar(false);
        }
    }*/
    public bool AddNewQuest()
    {
        var questQuerry = _questSearchBar.text;

        if (questQuerry.Equals(string.Empty) || questQuerry.Length <= 2)
        {
            print("Quest querry should be longer!");
            OpenQuestSearchBar(false);
            return false;
        }

        //QuestManager.instance.CreateNewQuest(QuestType.MainQuest, questQuerry);



        // FOR TESTING  Search and create the quest

        StartCoroutine(PlacesAPI.instance.StartSearchPlaces(questQuerry, (List<QuestPlace> places) =>
        {
            FindObjectOfType<MapPinVisualiser>().CreatePins(places);
            var quest = QuestManager.instance.CreateNewQuest(QuestType.MainQuest, questQuerry);

            places.ForEach(p => {
                QuestManager.instance.AddPlaceToQuest(quest,p);
            });

            _questSelector.SelectQuest(quest);
        }));

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
