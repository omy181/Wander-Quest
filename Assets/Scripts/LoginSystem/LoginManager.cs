using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Networking;

public class LoginManager : Singleton<LoginManager>
{
	[SerializeField] private LoginUI _loginUI;
	[SerializeField] private LoginWithGoogle _loginWithGoogle;

	public string Username => User.DisplayName;
    private Sprite _profilePhoto { set { ProfileUI.instance.SetProfilePictures(value); } }
    public Firebase.Auth.FirebaseUser User { get; private set; }
    public DatabaseReference DbReference { get; private set; }
	private FirebaseAuth _auth;

	protected override void Awake()
	{
		base.Awake();

		DbReference = FirebaseDatabase.DefaultInstance.RootReference;
		_auth = FirebaseAuth.DefaultInstance;

		_loginUI.OnLoginButtonPressed += _handleLogin;
		_loginUI.OnSignUpButtonPressed += _handleSignUp;
		ProfileUI.instance.OnLogOutPressed += _logout;

    }

    private void Start()
    {
        if (_auth.CurrentUser != null)
        {
            User = _auth.CurrentUser;
            _loginUI.ShowLoginWindow(false);
            _checkAndInitializeQuests();

			ProfileUI.instance.SetProfileName(User.DisplayName,User.Email);

            string photoUrl = User.PhotoUrl != null ? User.PhotoUrl.ToString() : null;
            if (!string.IsNullOrEmpty(photoUrl))
                StartCoroutine(_loadImage(photoUrl, (sprite) => _profilePhoto = sprite));
        }
    }

    private void _logout()
    {
        User = null;

		_loginWithGoogle.Logout();

        _loginUI.ShowLoginWindow(true);
    }

    private void _handleLogin()
	{

		_loginWithGoogle.Login((user) =>
		{
			User = user;

            _loginUI.ShowLoginWindow(false);
            _checkAndInitializeQuests();

            ProfileUI.instance.SetProfileName(User.DisplayName, User.Email);

        }, (sprite) =>
		{
            _profilePhoto = sprite;

        });



        return;
		/*
        string username = _loginUI.GetLogInUserName();
		string password = _loginUI.GetLogInPassword();


        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
		{
			_loginUI.GiveWarning("Please enter both username and password.");
			return;
		}

		DbReference.Child("users").Child(username).GetValueAsync().ContinueWith(task =>
		{
			if (task.IsCompletedSuccessfully)
			{
				DataSnapshot snapshot = task.Result;

				if (snapshot.Exists && snapshot.Child("password").Value.ToString() == password)
				{
					Username = username;

					MainThreadDispatcher.Instance.Enqueue(() =>
					{
						_loginUI.ShowLoginWindow(false);
					});

					_checkAndInitializeQuests();
				}
				else
				{
					MainThreadDispatcher.Instance.Enqueue(() =>
					{
						_loginUI.GiveWarning("Invalid username or password.");
					});
				}
			}
			else
			{
				MainThreadDispatcher.Instance.Enqueue(() =>
				{
					_loginUI.GiveWarning("Login failed. Try again later.");
				});
			}
		});
		*/
	}

	private void _handleSignUp()
	{
		/*
		string username = _loginUI.GetSignUpUserName();
		string password = _loginUI.GetSignUpPassword();

		if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || password.Length < 6)
		{
			_loginUI.GiveWarning("Password must be at least 6 characters.");
			return;
		}

		// Check if the username already exists
		DbReference.Child("users").Child(username).GetValueAsync().ContinueWith(task =>
		{
			if (task.IsCompletedSuccessfully)
			{
				if (task.Result.Exists)
				{
					MainThreadDispatcher.Instance.Enqueue(() =>
					{
						_loginUI.GiveWarning("Username already exists. Please choose a different one.");
					});
				}
				else
				{
					// Create a new user
					_createUserInDatabase(username, password);
				}
			}
			else
			{
				MainThreadDispatcher.Instance.Enqueue(() =>
				{
					_loginUI.GiveWarning("Signup failed. Try again later.");
				});
			}
		});
		*/
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

	private void _checkAndInitializeQuests()
	{
		DbReference.Child("users").Child(Username).Child("quests").GetValueAsync().ContinueWith(task =>
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
					_initializeEmptyQuestBranch(Username);
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
		//Debug.Log("Quests loaded successfully.");
		// Transition to the game panel after quests are loaded
		MainThreadDispatcher.Instance.Enqueue(() =>
		{
			//Debug.Log("Transitioning to game panel...");
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
