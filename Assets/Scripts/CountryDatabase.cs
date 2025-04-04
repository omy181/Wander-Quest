using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CountryDatabase : Singleton<CountryDatabase> 
{
    [SerializeField] private TextAsset _csvFile;
    private char _lineSep = '\n';
    private char[] _fieldSepchars = { '"', ';', '"' };
    private string _fieldSep = "\";\"";

    public Dictionary<string, GPSLocation> CountryToLocation;

    protected override void Awake()
    {
        base.Awake();
        CountryToLocation = _initializeDict();
    }

    private Dictionary<string, GPSLocation> _initializeDict()
    {
        Dictionary<string, GPSLocation> dict = new Dictionary<string, GPSLocation>();
        string[] lines = _csvFile.text.Split(_lineSep);

        // trim quotion
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Remove(0, 1);
            int lastcahr = char.IsWhiteSpace(lines[i][lines[i].Length - 1]) ? 2 : 1;
            lines[i] = lines[i].Remove(lines[i].Length - lastcahr);
        }

        string[] headers = lines[0].Split(_fieldSep, StringSplitOptions.None);


        for (int i = 1; i < lines.Length; i++)
        {
            string[] columns = lines[i].Split(_fieldSep);

            string location = columns[1];

            var locations = location.Split(',');
            var gps = new GPSLocation(double.Parse(locations[0]), double.Parse(locations[1]));

            dict.Add(columns[0], gps);
        }

        return dict;
    }
}
