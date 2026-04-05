using System;
using JetBrains.Annotations;

[Serializable]
public class User
{
    public UserCredentials credentials;
    public UserSettings settings;
    public UserAvatar avatar;
    public UserHighScores highScores;
}
[Serializable]
public class UserCredentials
{
    public string email;
    public string password;
}

[Serializable]
public class UserSettings
{
    public string UserId;
    public string KindVoornaam;
    public string KindAchternaam;
    public int KindLeeftijd = 9;
    public string ArtsNaam;
    public string BehandelingType;
    public string BehandelDatum;
}

[Serializable]
public class UserAvatar
{
    public string UserId;
    public int AvatarId = 0;
}

[Serializable]
public class UserHighScores
{
    public string UserId;
    public float Score;
}