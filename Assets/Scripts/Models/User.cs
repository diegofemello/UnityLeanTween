using System;

[Serializable]
public class User
{
    public string uid;
    public string userName;
    public string fullName;
}

[Serializable]
public class Auth
{
    public string accessToken;
    public bool authenticated;
    public string created;
    public string expiration;
    public User user;
}