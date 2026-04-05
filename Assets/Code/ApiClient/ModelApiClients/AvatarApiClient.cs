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

                UserAvatar avatar = JsonConvert.DeserializeObject<UserAvatar>(data.Data);
                return new WebRequestData<UserAvatar>(avatar);

            case WebRequestError error:
                Debug.LogWarning("⚠️ API error ontvangen (waarschijnlijk niet ingelogd)");
                return error;

            default:
                Debug.LogWarning("⚠️ Onbekende response, maar geen crash");
                return response;
        }
    }
}