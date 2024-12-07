using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestPrefab : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI progressText;

    public void SetQuestData(string title, string progress)
    {
        titleText.text = title;
        progressText.text = progress;
    }
}
