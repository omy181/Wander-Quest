using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class Quest
{
	[JsonIgnore] public string Title { get; private set; }
	[JsonProperty] public string MapsQuerry { get; private set; }
	[JsonProperty] public QuestType QuestType { get; private set; }

    [JsonIgnore] public string ID => MapsQuerry;
    [JsonIgnore] public int TotalTraveledCount => _places.Count(p=>p.IsTraveled);
    [JsonIgnore] public int TotalPlaceCount => _places.Count;
    [JsonProperty] private List<QuestPlace> _places;

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
    [JsonProperty] public GPSLocation Location;
    [JsonProperty] public Address Address;
    [JsonProperty] public string Name;
    [JsonProperty] public string ID;
    [JsonProperty] public bool IsTraveled;

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

public class QuestLeaderBoard
{
    public Quest Quest;
    public string UserName;

    public QuestLeaderBoard(Quest quest, string userName)
    {
        Quest = quest;
        UserName = userName;
    }
}
