using System.Collections.Generic;
using System.Linq;

public class Quest
{
    public string Title { get; private set; }
    public string MapsQuerry { get; private set; }
    public QuestType QuestType { get; private set; }

    public string ID => MapsQuerry;
    public int Progress => _places.Count; // Total traveled migros count, not percentage
    private List<QuestPlace> _places;

    public Quest(string title, QuestType questType, string mapsQuerry, List<QuestPlace> places)
    {
        Title = title;
        QuestType = questType;
        MapsQuerry = mapsQuerry;
        _places = places;
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

    public QuestPlace(GPSLocation location, string name,string id, Address address)
    {
        Location = location;
        Name = name;
        ID = id;
        Address = address;
    }
}

public enum QuestType
{
    MainQuest,SideQuest,DailyQuest
}
