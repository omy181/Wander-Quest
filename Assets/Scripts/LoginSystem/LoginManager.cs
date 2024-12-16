using Firebase.Database;
using TMPro;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
	public TMP_InputField usernameInputField;
	public GameObject loginPanel;
	public GameObject gamePanel;

	private string username;
	private DatabaseReference dbReference;

	void Start()
	{
		dbReference = FirebaseDatabase.DefaultInstance.RootReference;
	}

	public void Login()
	{
		username = usernameInputField.text.Trim();

		// Validate that the username is not empty or invalid
		if (string.IsNullOrEmpty(username))
		{
			Debug.LogWarning("Username cannot be empty!");
			return;
		}

		// Disable the login UI to avoid multiple submissions while loading data
		loginPanel.SetActive(false);

		CheckUserExists();
	}

	private void CheckUserExists()
	{
		dbReference.Child("users").Child(username).GetValueAsync().ContinueWith(task =>
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
				ShowLoginPanel();
			}
		});
	}

	private void CreateUser()
	{
		// Create a new user object with a default email (you can modify this)
		User newUser = new User(username, "default_email@example.com");
		string json = JsonUtility.ToJson(newUser);

		dbReference.Child("users").Child(username).SetRawJsonValueAsync(json).ContinueWith(task =>
		{
			if (task.IsCompleted)
			{
				Debug.Log("New user created successfully!");

				// Create the quests branch for the new user
				dbReference.Child("users").Child(username).Child("quests").SetValueAsync("").ContinueWith(questTask =>
				{
					if (questTask.IsCompleted)
					{
						Debug.Log("Quest branch created successfully!");
						// Initialize quests after creating the user and quest branch
						QuestManager.instance.InitializeQuests(username, OnQuestsLoaded);
					}
					else
					{
						Debug.LogError("Failed to create quest branch.");
						ShowLoginPanel();
					}
				});
			}
			else
			{
				Debug.LogError("Failed to create user.");
				ShowLoginPanel();
			}
		});
	}

	private void CheckAndInitializeQuests()
	{
		// Check if the quests branch exists
		dbReference.Child("users").Child(username).Child("quests").GetValueAsync().ContinueWith(task =>
		{
			if (task.IsCompleted)
			{
				if (task.Result.Exists)
				{
					// Quests branch exists, initialize quests
					Debug.Log("Quests branch exists for user.");
					QuestManager.instance.InitializeQuests(username, OnQuestsLoaded);
				}
				else
				{
					// No quests branch, create one
					Debug.Log("No quests branch, creating one.");
					dbReference.Child("users").Child(username).Child("quests").SetValueAsync("").ContinueWith(questTask =>
					{
						if (questTask.IsCompleted)
						{
							Debug.Log("Quest branch created successfully!");
							// Initialize quests after creating the quest branch
							QuestManager.instance.InitializeQuests(username, OnQuestsLoaded);
						}
						else
						{
							Debug.LogError("Failed to create quest branch.");
							ShowLoginPanel();
						}
					});
				}
			}
			else
			{
				Debug.LogError("Failed to check quests branch.");
				ShowLoginPanel();
			}
		});
	}

	// Callback method when quests are loaded
	private void OnQuestsLoaded()
	{
		Debug.Log("Quests loaded for user: " + username);

		// Directly switch to the game panel after quests are loaded
		SwitchToGamePanel();
	}

	// Method to switch to the game panel
	private void SwitchToGamePanel()
	{
		// Ensure we are on the main thread when updating UI
		if (gamePanel != null)
		{
			print("hello");
			gamePanel.SetActive(true);
		}
	}

	// To show the login panel again if an error occurs
	private void ShowLoginPanel()
	{
		loginPanel.SetActive(true);
	}
}
