using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.U2D;

public class LoginManager : Singleton<LoginManager>
{
	[SerializeField] private LoginUI _loginUI;
	[SerializeField] private LoginWithGoogle _loginWithGoogle;

	public string UserID { get {
            
            return _user != null ? _user.UserId : "EditorUserID";
        }
    }
	private Sprite _profilePhoto { set { ProfileUI.instance.SetProfilePictures(value); } }
	private Firebase.Auth.FirebaseUser _user;

	public string UserName { get {	
            return _user != null ? _user.DisplayName : "Editor User";
        }
    }

    public string UserEmail
    {
        get
        {
            return _user != null ? _user.Email : "EditorEmail@gmail.com";
        }
    }
    public DatabaseReference DbReference { get; private set; }
	private FirebaseAuth _auth;

	protected override void Awake()
	{
		base.Awake();

		DbReference = FirebaseDatabase.DefaultInstance.RootReference;
		_auth = FirebaseAuth.DefaultInstance;

        _loginUI.OnTestLoginButtonPressed += _testLogin;
        _loginUI.OnLoginButtonPressed += _handleLogin;
		_loginUI.OnSignUpButtonPressed += _handleSignUp;
		ProfileUI.instance.OnLogOutPressed += _logout;

    }

    private void Start()
    {
        if (_auth.CurrentUser != null)
        {

#if UNITY_EDITOR

#else
			_user = _auth.CurrentUser;

			string photoUrl = _user.PhotoUrl != null ? _user.PhotoUrl.ToString() : null;
            if (!string.IsNullOrEmpty(photoUrl))
                StartCoroutine(_loadImage(photoUrl, (sprite) => _profilePhoto = sprite));
#endif

            _loginUI.ShowLoginWindow(false);
            _checkAndInitializeQuests();

            ProfileUI.instance.SetProfileName(UserName, UserEmail);
        }
    }

    private void _logout()
    {
        _user = null;

		_loginWithGoogle.Logout();

		StartCoroutine(nameof(_logOutWait));

    }

	private IEnumerator _logOutWait()
	{
        _loginUI.ShowLoadingScreen(true);

        _loginUI.ShowLoginWindow(true);
		yield return new WaitForSecondsRealtime(1f);

        _loginUI.ShowLoadingScreen(false);
    }

	private void _testLogin()
	{
        _user = null;

        _checkAndInitializeQuests();
        _checkAndInitializeUserData();

        _profilePhoto = null;
        ProfileUI.instance.SetProfileName(UserName, UserEmail);
        _loginUI.ShowLoginWindow(false);
    }

    private void _handleLogin()
	{
		_loginUI.ShowLoadingScreen(true);

		try
		{
			_loginWithGoogle.Login((user) =>
			{
#if UNITY_EDITOR
				_user = user;

				_loginUI.ShowLoginWindow(false);
				_checkAndInitializeQuests();
				_checkAndInitializeUserData();


                ProfileUI.instance.SetProfileName(UserName, UserEmail);

				_loginUI.ShowLoadingScreen(false);
#else
         _user = user;

            _loginUI.ShowLoginWindow(false);
            _checkAndInitializeQuests();
			_checkAndInitializeUserData();

            ProfileUI.instance.SetProfileName(UserName, UserEmail);

            _loginUI.ShowLoadingScreen(false);
#endif


            }, (sprite) =>
			{
				_profilePhoto = sprite;

			}, () =>
			{
				_loginUI.ShowLoadingScreen(false);
			});
		}
		catch (Exception e)
		{
			Debug.LogException(e);
			_loginUI.ShowLoadingScreen(false);
			Debug.Log("Couln't log in via Google");
		}
	}

	private void _handleSignUp()
	{

	}

	private void _createUserInDatabase(string username, string password)
	{
		User newUser = new User(username, password);
		string json = JsonUtility.ToJson(newUser);

		DbReference.Child("users").Child(username).SetRawJsonValueAsync(json).ContinueWith(task =>
		{
			if (task.IsCompletedSuccessfully)
			{
				_initializeEmptyQuestBranch(username);
			}
			else
			{
				MainThreadDispatcher.Instance.Enqueue(() =>
				{
					_loginUI.GiveWarning("Failed to complete signup process.");
				});
			}
		});
	}

	private void _initializeEmptyQuestBranch(string username)
	{
		DbReference.Child("users").Child(username).Child("quests").SetValueAsync(new List<string>()).ContinueWith(task =>
		{
			if (task.IsCompletedSuccessfully)
			{
				MainThreadDispatcher.Instance.Enqueue(() =>
				{
					QuestManager.instance.InitializeQuests(_onQuestsLoaded);
				});
			}
			else
			{
				MainThreadDispatcher.Instance.Enqueue(() =>
				{
					_loginUI.GiveWarning("Failed to initialize user data.");
				});
			}
		});
	}

    private void _initializeEmptyUserDataBranch(string username)
    {
		string userData = JsonConvert.SerializeObject(new UserData());
        DbReference.Child("users").Child(username).Child("UserData").SetRawJsonValueAsync(userData).ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                MainThreadDispatcher.Instance.Enqueue(() =>
                {
                    LevelManager.instance.InitializeLevelData();
                });
                
            }
            else
            {
                MainThreadDispatcher.Instance.Enqueue(() =>
                {
                    _loginUI.GiveWarning("Failed to initialize user data.");
                });
            }
        });
    }

    private void _checkAndInitializeQuests()
	{
		DbReference.Child("users").Child(UserID).Child("quests").GetValueAsync().ContinueWith(task =>
		{
			if (task.IsCompletedSuccessfully)
			{
				if (task.Result.Exists)
				{
					MainThreadDispatcher.Instance.Enqueue(() =>
					{
						QuestManager.instance.InitializeQuests(_onQuestsLoaded);
					});
				}
				else
				{
					_initializeEmptyQuestBranch(UserID);
				}
			}
			else
			{
				MainThreadDispatcher.Instance.Enqueue(() =>
				{
					_loginUI.GiveWarning("Failed to load user data.");
				});
			}
		});
	}

    private void _checkAndInitializeUserData()
    {
        DbReference.Child("users").Child(UserID).Child("UserData").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                if (task.Result.Exists)
                {
                    MainThreadDispatcher.Instance.Enqueue(() =>
                    {
                        LevelManager.instance.InitializeLevelData();
                    });
                }
                else
                {
                    _initializeEmptyUserDataBranch(UserID);
                }
            }
            else
            {
                MainThreadDispatcher.Instance.Enqueue(() =>
                {
                    _loginUI.GiveWarning("Failed to load user data.");
                });
            }
        });
    }

    private void _onQuestsLoaded()
	{
		// Transition to the game panel after quests are loaded
		MainThreadDispatcher.Instance.Enqueue(() =>
		{
			QuestSelector.instance.SelectQuest(QuestManager.instance.GetMostRecentQuest());

			QuestManager.instance.AddDefaultSideQuests();
		});
	}

    private IEnumerator _loadImage(string imageUri, Action<Sprite> onImageLoaded)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUri);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            onImageLoaded?.Invoke(sprite);
        }
        else
        {
            Debug.Log("Error loading image: " + www.error);
        }
    }

}
