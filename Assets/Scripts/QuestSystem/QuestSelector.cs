using Holylib.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSelector : MonoBehaviour
{
    [SerializeField] private GameObject _questList;
    [SerializeField] private Transform _questContent;
    [SerializeField] private GameObject _questPrefab;
    [SerializeField] private TMP_Text _activeQuestText;
    [SerializeField] private TMP_Text _activeQuestProgressText;
    [SerializeField] private MapPinVisualiser _mapPinVisualiser;

    private Quest _activeQuest;

      private void Update()
      {
        CheckQuestProgression(_activeQuest);
      }

    public void ShowQuestSelectorToggle()
    {
        ShowQuestSelector(!_questList.activeSelf);
    }

    public void ShowQuestSelector(bool state)
    {
        if (state)
        {
            _questList.SetActive(true);
            _listQuests();
        }
        else
        {
            _questList.SetActive(false);
        }
    }

    private void _listQuests()
    {
        foreach (Transform q in _questContent)
        {
            Destroy(q.gameObject);
        }

        QuestManager.instance.GetActiveQuests().ForEach(quest => {
            var questPrefab = Instantiate(_questPrefab, _questContent).GetComponent<QuestPrefab>();
            questPrefab.SetQuestData(quest,()=> SelectQuest(quest));
        });
    }

    public void SelectQuest(Quest quest)
    {
        _activeQuestText.text = quest.Title;
        _activeQuestProgressText.text = quest.TotalTraveledCount.ToString();
        _activeQuest = quest;
        _mapPinVisualiser.CreatePins(quest.GetPlaces());
        _mapPinVisualiser.FocusPins(quest.GetPlaces());
        ShowQuestSelector(false);


    } 
    private void CheckQuestProgression(Quest quest)
    {
        if (quest == null) return;

        foreach (var place in quest.GetPlaces())
        {
            if (place.IsTraveled) continue;

            var gpsworldcord = MapUtilities.LatLonToWorld(GPS.instance.GetLastGPSLocation());
            var placeworldcord = MapUtilities.LatLonToWorld(place.Location);

            var dis = Vector2.Distance(gpsworldcord, placeworldcord);

            if(dis <= 0.0002055)
            {
                place.IsTraveled = true;
                SelectQuest(quest);
            }
        }
    }
}
