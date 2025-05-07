using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardQuestElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _order;
    [SerializeField] private Image _colorBack;

    public void Initialize(string userName,int order,int count)
    {
        _name.text = order + ". "+ userName;
        _order.text ="Found: "+ count;

        _colorBack.color = order switch
        {
            1=>Color.yellow,
            2=>Color.Lerp(Color.yellow,Color.red,0.5f),
            3=>Color.red,
            _=>Color.gray
        };
    }
}
