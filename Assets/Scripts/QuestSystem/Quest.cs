using System.Collections.Generic;
using System.Linq;

public class Quest
{
    public string Title { get; private set; }
    public string MapsQuerry { get; private set; }
    public QuestType QuestType { get; private set; }

    public string ID => MapsQuerry;
    public int TotalTraveledCount => _places.Count(p=>p.IsTraveled);
    public int TotalPlaceCount => _places.Count;
    private List<QuestPlace> _places;

    public Quest(QuestType questType, string mapsQuerry, List<QuestPlace> places)
    {
        QuestType = questType;
        MapsQuerry = mapsQuerry;
        _places = places;
        _createTitle();
    }

    private void _createTitle()
    {
        string[] words = MapsQuerry.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
        }
        Title = string.Join(" ", words) + " List";
    }

    public bool AddPlace(QuestPlace place)
    {
        if (_places.Any(p=>p.ID == place.ID))
        {
            return false;
        }
        else
        {
            _places.Add(place);
            return true;
        }

    }

    public List<QuestPlace> GetPlaces()
    {
        return new(_places);
    }
}

public class QuestPlace
{
    public GPSLocation Location;
    public Address Address;
    public string Name;
    public string ID;
    public bool IsTraveled;

    public QuestPlace(GPSLocation location, string name,string id, Address address,bool isTraveled)
    {
        Location = location;
        Name = name;
        ID = id;
        Address = address;
        IsTraveled = isTraveled;
    }
}

public enum QuestType
{
    MainQuest,SideQuest,DailyQuest
}
