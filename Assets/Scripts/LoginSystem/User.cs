using System.Collections.Generic;

public class User
{
	public string username;
	public string password;
	public List<string> quests; // Store quest IDs or data

	public User() { }

	public User(string username, string password)
	{
		this.username = username;
		this.password = password;
		this.quests = new List<string>(); // Initialize quests as empty list
	}
}
