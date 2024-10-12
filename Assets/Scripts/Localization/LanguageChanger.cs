using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holylib.Localization;

public class LanguageChanger : MonoBehaviour
{

    public void ChangeLanguage(int index)
    {
        TranslateManager.instance.ChangeLanguage((Languages)index);
    }

}
