using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class UserApiClient : MonoBehaviour
{
    public WebClient webClient;
    public User user;
    public async Awaitable<IWebRequestReponse> Register(UserCredentials credentials)
    {
        string route = "/account/register";
        string data = JsonConvert.SerializeObject(credentials, JsonHelper.CamelCaseSettings);

        return await webClient.SendPostRequest(route, data);
    }

    public async Awaitable<IWebRequestReponse> Login(UserCredentials credentials)
    {
        string route = "/account/login";
        string data = JsonConvert.SerializeObject(credentials, JsonHelper.CamelCaseSettings);

        IWebRequestReponse response = await webClient.SendPostRequest(route, data);
        return ProcessLoginResponse(response);
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

