using Holylib.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSelector : MonoBehaviour
{
    [SerializeField] private GameObject _questList;
    [SerializeField] private Transform _questContent;
    [SerializeField] private GameObject _questPrefab;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !HolyUtilities.isOnUI())  /// TODO:  ADD TOUCH SUPPORT
        {
            ShowQuestSelector(false);
        }
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
            Instantiate(_questPrefab, _questContent).GetComponent<QuestPrefab>().SetQuestData(quest.Title,quest.Progress.ToString());
            });
    }
}
