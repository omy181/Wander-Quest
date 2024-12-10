using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _progressText;

    public void SetQuestData(string title, string progress)
    {
        _titleText.text = title;
        _progressText.text = progress;
    }
}
