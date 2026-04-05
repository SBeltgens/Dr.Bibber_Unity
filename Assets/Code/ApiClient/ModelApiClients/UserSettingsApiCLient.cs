using System.Collections.Generic;
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

                // De API stuurt een array [], dus we parsen als List
                var settingsList = JsonConvert.DeserializeObject<List<UserSettings>>(data.Data);

                if (settingsList != null && settingsList.Count > 0)
                {
                    // We pakken de eerste set instellingen uit de lijst
                    UserSettings settings = settingsList[0];
                    return new WebRequestData<UserSettings>(settings);
                }
                else
                {
                    Debug.LogWarning("⚠️ Geen settings gevonden in de array.");
                    return new WebRequestError("Geen data gevonden");
                }

            case WebRequestError error:
                Debug.LogWarning("⚠️ API error ontvangen (waarschijnlijk niet inlogd)");
                return error;

            default:
                return response;
        }
    }
}
