using Newtonsoft.Json;
using UnityEngine;

public class HighscoreApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> PostHighscore(Highscore highscore)
    {
        Debug.Log("📤 POST Highscore");

        string route = "/highscores";
        string data = JsonConvert.SerializeObject(highscore, JsonHelper.CamelCaseSettings);

        Debug.Log("➡️ POST data: " + data);

        IWebRequestReponse response = await webClient.SendPostRequest(route, data);

        Debug.Log("⬅️ POST response: " + response);

        return ParseResponse(response);
    }

    public async Awaitable<IWebRequestReponse> UpdateHighscore(string gameName, Highscore highscore)
    {
        Debug.Log("📤 PUT Highscore");

        string route = "/highscores/" + gameName;
        string data = JsonConvert.SerializeObject(highscore, JsonHelper.CamelCaseSettings);

        Debug.Log("➡️ PUT data: " + data);

        IWebRequestReponse response = await webClient.SendPutRequest(route, data);

        Debug.Log("⬅️ PUT response: " + response);

        return ParseResponse(response);
    }

    public async Awaitable<IWebRequestReponse> GetHighscoreByGame(string gameName)
    {
        Debug.Log("📥 GET Highscore");

        string route = "/highscores/" + gameName;

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

                Highscore hs = JsonConvert.DeserializeObject<Highscore>(data.Data);
                return new WebRequestData<Highscore>(hs);

            case WebRequestError error:
                Debug.LogWarning("⚠️ API error ontvangen (waarschijnlijk niet ingelogd)");
                return error;

            default:
                Debug.LogWarning("⚠️ Onbekende response, maar geen crash");
                return response;
        }
    }
}