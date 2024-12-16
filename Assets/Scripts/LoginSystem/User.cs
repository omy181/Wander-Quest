using System.Collections.Generic;

public class User
{
	public string username;
	public string email;
	public List<string> quests; // Store quest IDs or data

	public User() { }

	public User(string username, string email)
	{
		this.username = username;
		this.email = email;
		this.quests = new List<string>(); // Initialize quests as empty list
	}
}
