using UnityEngine;

public class TabSelector : MonoBehaviour
{
    [SerializeField] private GameObject[] _tabs;

    private void _unselectAll()
    {
        foreach (var tab in _tabs)
        {
            tab.gameObject.SetActive(false);
        }
    }

    public void SelectTab(int index)
    {
        _unselectAll();
        _tabs[index].gameObject.SetActive(true);
    }
}
