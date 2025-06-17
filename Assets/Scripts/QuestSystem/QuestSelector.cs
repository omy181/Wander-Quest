using Holylib.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSelector : Singleton<QuestSelector>
{
    [SerializeField] private GameObject _questList;
    [SerializeField] private Transform _questContent;
    [SerializeField] private GameObject _questPrefab;
    [SerializeField] private TMP_Text _activeQuestText;
    [SerializeField] private TMP_Text _activeQuestProgressText;
    [SerializeField] private MapPinVisualiser _mapPinVisualiser;
    [SerializeField] private Button _scanButton;

    private bool _isQuestSelectorOpen => _questList.activeSelf;

    public Quest _activeQuest { get; private set; }

    private void Start()
    {
        _scanButton.onClick.AddListener(_scanArea);
        UIManager.instance.OnUICancel += ()=> ShowQuestSelector(false);
    }

    private void Update()
    {
        _checkQuestProgression(_activeQuest);
    }

    public void ShowQuestSelectorToggle()
    {
        ShowQuestSelector(!_isQuestSelectorOpen);
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
        if (quest == null) return;

        _activeQuestText.text = quest.Title;
        _activeQuestProgressText.text = quest.TotalTraveledCount.ToString();
        _activeQuest = quest;
        _mapPinVisualiser.FocusPins(quest);
        ShowQuestSelector(false);

        _scanArea();
    } 
    private void _checkQuestProgression(Quest quest)
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
                QuestManager.instance.UpdatePlaceData(quest,place);
                LevelManager.instance.CurrentLevel += 0.35f;
                NotificationManager.SendDiscoveryNotification(place.Name);
                SelectQuest(quest);
            }
        }
    }

    private void _scanArea()
    {
        if (_activeQuest != null)
        {
            StartCoroutine(PlacesAPI.instance.StartSearchPlaces(_activeQuest.MapsQuerry,5, (List<QuestPlace> places) =>
            {
                _mapPinVisualiser.CreatePins(places);

                places.ForEach(p => {
                    QuestManager.instance.AddPlaceToQuest(_activeQuest, p);
                });

                
                _mapPinVisualiser.FocusPins(_activeQuest);
            }));
        }
    }
}
