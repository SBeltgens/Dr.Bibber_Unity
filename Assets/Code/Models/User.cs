using System;

[Serializable]
public class User
{
    public string email;
    public string password;
    public string? firstName;
    public string? lastName;
    public int age = 9;
    public string? nameDoctor;
    public string? operationType;
    public string? operationDate;
    public int avatar = 0;
    public string? balanceMinigameHighscore;
}