using System;

[Serializable]
public class User
{
    public string email;
    public string password;

    public UserSettings settings;
    public UserStats stats;
}

[Serializable]
public class UserSettings
{
    public string firstName;
    public string lastName;
    public int age = 9;
    public string nameDoctor;
    public string operationType;
    public string operationDate;
}

[Serializable]
public class UserStats
{
    public int avatar = 0;
    public string balanceMinigameHighscore = "00:00.00";
}