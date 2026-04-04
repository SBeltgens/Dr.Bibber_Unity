using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class UserApiClient : MonoBehaviour
{
    public WebClient webClient;
    public User user;

    public async Awaitable<IWebRequestReponse> UpdateUserSettings(UserSettings settings)
    {
        string route = "/account/usersettings";
        string data = JsonConvert.SerializeObject(settings, JsonHelper.CamelCaseSettings);
        return await webClient.SendPutRequest(route, data);
    }
    public async Awaitable<IWebRequestReponse> UpdateUserStats(UserStats stats)
    {
        string route = "/account/userstats";
        string data = JsonConvert.SerializeObject(stats, JsonHelper.CamelCaseSettings);
        return await webClient.SendPutRequest(route, data);
    }
    public async Awaitable<IWebRequestReponse> Register(User user)
    {
        string route = "/account/register";
        string data = JsonConvert.SerializeObject(user, JsonHelper.CamelCaseSettings);

        return await webClient.SendPostRequest(route, data);
    }

    public async Awaitable<IWebRequestReponse> Login(User user)
    {
        string route = "/account/login";
        string data = JsonConvert.SerializeObject(user, JsonHelper.CamelCaseSettings);

        IWebRequestReponse response = await webClient.SendPostRequest(route, data);
        return ProcessLoginResponse(response);
    }

    public async Awaitable<IWebRequestReponse> GetUserData()
    {
        string route = "/account/userdata";
        return await webClient.SendGetRequest(route);
    }

    private IWebRequestReponse ProcessLoginResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                string token = JsonHelper.ExtractToken(data.Data);
                webClient.SetToken(token);
                return new WebRequestData<string>("Succes");
            default:
                return webRequestResponse;
        }
    }

}

