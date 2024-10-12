using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holylib.Localization
{
    public enum Languages
    {
        English, Turkish
    }

    public class TranslateManager : MonoBehaviour
    {
        public Languages CurrentLanguage;

        [SerializeField] TextAsset csvFile;
        char lineSep = '\n';
        char[] fieldSepchars = { '"', ';', '"' };
        string fieldSep;

        public Dictionary<string, string> CurrentLanguageDictionary;

        public static TranslateManager instance;
        private void Awake()
        {
            instance = this;

            fieldSep = string.Concat(fieldSepchars);


            if (PlayerPrefs.HasKey("Language"))
            {
                Languages lang = (Languages)Enum.Parse(typeof(Languages), PlayerPrefs.GetString("Language"));
                ChangeLanguage(lang);
            }
            else
            {
                ChangeLanguage(Languages.English);
            }

        }
         
        public Action OnChangeLanguage;
        public void ChangeLanguage(Languages lang)
        {
            PlayerPrefs.SetString("Language", lang.ToString());
            CurrentLanguage = lang;

            CurrentLanguageDictionary = InitializeDict(CurrentLanguage);

            if (OnChangeLanguage != null) OnChangeLanguage();
        }
        private Dictionary<string, string> InitializeDict(Languages lang)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] lines = csvFile.text.Split(lineSep);

            // trim quotion
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Remove(0, 1);
                int lastcahr = char.IsWhiteSpace(lines[i][lines[i].Length - 1]) ? 2 : 1;
                lines[i] = lines[i].Remove(lines[i].Length - lastcahr);
            }

            string[] headers = lines[0].Split(fieldSep, StringSplitOptions.None);

            int languagecolumn = -1;

            languagecolumn = (int)lang + 1;

            if (languagecolumn == -1)
            {
                Debug.LogError("Language not found");
                languagecolumn = 1;
            }


            for (int i = 1; i < lines.Length; i++)
            {
                string[] columns = lines[i].Split(fieldSep);

                if (columns[0] == string.Empty)
                {
                    continue;
                }

                string translatedword = "";
                if (languagecolumn < columns.Length)
                {
                    if (string.IsNullOrWhiteSpace(columns[languagecolumn]))
                    {
                        print(columns[0] + " is not translated to " + CurrentLanguage.ToString());
                        translatedword = columns[1];
                    }
                    else
                    {
                        translatedword = columns[languagecolumn];
                    }

                }
                else
                {
                    translatedword = columns[1];
                }

                translatedword = translatedword.Replace('\r', ' ');

                dict.Add(columns[0], translatedword);
            }

            return dict;
        }

        public string TranslateWord(string keyword)
        {
            if (!CurrentLanguageDictionary.ContainsKey(keyword)) return keyword;

            return CurrentLanguageDictionary[keyword];
        }
    }
}