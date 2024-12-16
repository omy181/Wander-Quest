using Firebase.Database;
using TMPro;
using UnityEngine;

public class LoginManager : Singleton<LoginManager>
{
	[SerializeField] private LoginUI _loginUI;

	public string Username { get; private set; }
	public DatabaseReference DbReference { get; private set; }

    protected override void Awake()
	{
		base.Awake();

		DbReference = FirebaseDatabase.DefaultInstance.RootReference;
		_loginUI.OnLoginButtonPressed += _login;
	}

	///Todo:									Sign up ve login kisimlarini ayirdim ben. log in kisminda eger kullanici yoksa yeni kullanici olusturmasin

	private void _login()
	{
		Username = _loginUI.GetLogInUserName();

		if (string.IsNullOrEmpty(Username))
		{
			Debug.LogWarning("Username cannot be empty!");
			return;
		}

        _loginUI.ShowLoginWindow(false);  /// TODO:              bu fonksiyonu burda cagirma, login islemi basarili bir sekilde tamamlandiginda cagir
			

		CheckUserExists();
	}

	private void CheckUserExists()
	{
		DbReference.Child("users").Child(Username).GetValueAsync().ContinueWith(task =>
		{
			if (task.IsCompleted)
			{
				if (task.Result.Exists)
				{
					// User exists, proceed to check for quests
					Debug.Log("User exists, checking quests branch.");
					CheckAndInitializeQuests();
				}
				else
				{
					// User doesn't exist, create a new user profile
					CreateUser();
				}
			}
			else
			{
				Debug.LogError("Failed to check if user exists.");
            }
		});
	}

	private void CreateUser()
	{
		// Create a new user object with a default email (you can modify this)
		User newUser = new User(Username, "default_email@example.com");
		string json = JsonUtility.ToJson(newUser);

		DbReference.Child("users").Child(Username).SetRawJsonValueAsync(json).ContinueWith(task =>
		{
			if (task.IsCompleted)
			{
				Debug.Log("New user created successfully!");

				// Create the quests branch for the new user
				DbReference.Child("users").Child(Username).Child("quests").SetValueAsync("").ContinueWith(questTask =>
				{
					if (questTask.IsCompleted)
					{
						Debug.Log("Quest branch created successfully!");
						// Initialize quests after creating the user and quest branch
						QuestManager.instance.InitializeQuests(OnQuestsLoaded);
					}
					else
					{
						Debug.LogError("Failed to create quest branch.");
                    }
				});
			}
			else
			{
				Debug.LogError("Failed to create user.");
            }
		});
	}

	private void CheckAndInitializeQuests()
	{
		// Check if the quests branch exists
		DbReference.Child("users").Child(Username).Child("quests").GetValueAsync().ContinueWith(task =>
		{
			if (task.IsCompleted)
			{
				if (task.Result.Exists)
				{
					// Quests branch exists, initialize quests
					Debug.Log("Quests branch exists for user.");
					QuestManager.instance.InitializeQuests(OnQuestsLoaded);
				}
				else
				{
					// No quests branch, create one
					Debug.Log("No quests branch, creating one.");
					DbReference.Child("users").Child(Username).Child("quests").SetValueAsync("").ContinueWith(questTask =>
					{
						if (questTask.IsCompleted)
						{
							Debug.Log("Quest branch created successfully!");
							// Initialize quests after creating the quest branch
							QuestManager.instance.InitializeQuests(OnQuestsLoaded);
						}
						else
						{
							Debug.LogError("Failed to create quest branch.");
                        }
					});
				}
			}
			else
			{
				Debug.LogError("Failed to check quests branch.");
            }
		});
	}

	// Callback method when quests are loaded
	private void OnQuestsLoaded()
	{
		Debug.Log("Quests loaded for user: " + Username);

        // Directly switch to the game panel after quests are loaded
    }
}
