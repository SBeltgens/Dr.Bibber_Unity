using Newtonsoft.Json;
using UnityEngine;

public class UserSettingsApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> PostSettings(UserSettings settings)
    {
        Debug.Log("📤 POST Settings");

        string route = "/usersettings";
        string data = JsonConvert.SerializeObject(settings, JsonHelper.CamelCaseSettings);

        Debug.Log("➡️ POST data: " + data);

        IWebRequestReponse response = await webClient.SendPostRequest(route, data);

        Debug.Log("⬅️ POST response: " + response);

        return ParseResponse(response);
    }

    public async Awaitable<IWebRequestReponse> UpdateSettings(UserSettings settings)
    {
        Debug.Log("📤 PUT Settings");

        string route = "/usersettings";
        string data = JsonConvert.SerializeObject(settings, JsonHelper.CamelCaseSettings);

        Debug.Log("➡️ PUT data: " + data);

        IWebRequestReponse response = await webClient.SendPutRequest(route, data);

        Debug.Log("⬅️ PUT response: " + response);

        return ParseResponse(response);
    }

    public async Awaitable<IWebRequestReponse> GetSettings()
    {
        Debug.Log("📥 GET Settings");

        string route = "/usersettings";

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

                UserHighScores hs = JsonConvert.DeserializeObject<UserHighScores>(data.Data);
                return new WebRequestData<UserHighScores>(hs);

            case WebRequestError error:
                Debug.LogWarning("⚠️ API error ontvangen (waarschijnlijk niet ingelogd)");
                return error;

            default:
                Debug.LogWarning("⚠️ Onbekende response, maar geen crash");
                return response;
        }
    }
}