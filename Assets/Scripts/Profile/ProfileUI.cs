using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUI : Singleton<ProfileUI>
{
    [SerializeField] private Image[] _profilePictures;
    [SerializeField] private TMP_Text _profileName;
    [SerializeField] private TMP_Text _profileEmail;
    [SerializeField] protected Button _logOutButton;
    protected override void Awake()
    {
        base.Awake();
        _logOutButton.onClick.AddListener(()=> OnLogOutPressed?.Invoke());

    }

    public Action OnLogOutPressed;
    public void SetProfilePictures(Sprite picture)
    {
        foreach (var p in _profilePictures)
        {
            p.sprite = picture;
        }
    }

    public void SetProfileName(string name,string email)
    {
        _profileName.text = name;
        _profileEmail.text = email;
    }
}
