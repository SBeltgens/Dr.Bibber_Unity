using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class AvatarApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> PostAvatar(UserAvatar avatar)
    {
        Debug.Log("📤 POST Avatar");

        string route = "/useravatars";
        string data = JsonConvert.SerializeObject(avatar, JsonHelper.CamelCaseSettings);

        Debug.Log("➡️ POST data: " + data);

        IWebRequestReponse response = await webClient.SendPostRequest(route, data);

        Debug.Log("⬅️ POST response: " + response);

        return ParseResponse(response);
    }

    public async Awaitable<IWebRequestReponse> UpdateAvatar(UserAvatar avatar)
    {
        Debug.Log("📤 PUT Avatar");

        string route = "/useravatars";
        string data = JsonConvert.SerializeObject(avatar, JsonHelper.CamelCaseSettings);

        Debug.Log("➡️ PUT data: " + data);

        IWebRequestReponse response = await webClient.SendPutRequest(route, data);

        Debug.Log("⬅️ PUT response: " + response);

        return ParseResponse(response);
    }

    public async Awaitable<IWebRequestReponse> GetAvatar()
    {
        Debug.Log("📥 GET Highscore");

        string route = "/useravatars";

        IWebRequestReponse response = await webClient.SendGetRequest(route);

        Debug.Log("⬅️ GET response: " + response);

        return ParseResponse(response);
    }

    private IWebRequestReponse ParseResponse(IWebRequestReponse response)
    {
        if (response == null)
        {
            Debug.LogError("❌ Response is NULL");
            return null;
        }

        switch (response)
        {
            case WebRequestData<string> data:
                Debug.Log("📦 JSON: " + data.Data);

                try
                {
                    // De API stuurt GEEN array meer, maar een direct object {}
                    UserAvatar avatar = JsonConvert.DeserializeObject<UserAvatar>(data.Data);

                    if (avatar != null)
                    {
                        return new WebRequestData<UserAvatar>(avatar);
                    }
                    else
                    {
                        Debug.Log("ℹ️ Geen avatar data gevonden. Gebruik default.");
                        return new WebRequestData<UserAvatar>(new UserAvatar());
                    }
                }
                catch (JsonException ex)
                {
                    // Als het tóch een lijst blijkt te zijn in sommige gevallen, 
                    // vangt deze catch dat op of kun je hier verder debuggen.
                    Debug.LogError("❌ JSON Parse Error in Avatar: " + ex.Message);
                    return new WebRequestError("Fout bij verwerken avatar data");
                }

            case WebRequestError error:
                Debug.LogWarning("⚠️ API error ontvangen in AvatarApiClient");
                return error;

            default:
                return response;
        }
    }
}