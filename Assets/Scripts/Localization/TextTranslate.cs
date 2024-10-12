using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Holylib.Localization
{
    public class TextTranslate : MonoBehaviour
    {
        [SerializeField] TMP_Text Text;
        [SerializeField] string Keyword;
        private void Start()
        {
            if (Keyword != string.Empty)
            {

                TranslateManager.instance.OnChangeLanguage += Translate;

                Translate();
            }
            else
            {
                AddTildatoText();
            }
        }

        void AddTildatoText()
        {

           Text.text = "~" + Text.text + "~";
            
        }

        void Translate()
        {
            Text.text = TranslateManager.instance.TranslateWord(Keyword);
        }
    }
}