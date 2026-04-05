using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class HighscoreApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> PostHighscore(UserHighScores highscore)
    {
        Debug.Log("📤 POST Highscore");

        string route = "/highscores";
        string data = JsonConvert.SerializeObject(highscore, JsonHelper.CamelCaseSettings);

        Debug.Log("➡️ POST data: " + data);

        IWebRequestReponse response = await webClient.SendPostRequest(route, data);

        Debug.Log("⬅️ POST response: " + response);

        return ParseResponse(response);
    }

    public async Awaitable<IWebRequestReponse> UpdateHighscore(UserHighScores highscore)
    {
        Debug.Log("📤 PUT Highscore");

        string route = "/highscores";
        string data = JsonConvert.SerializeObject(highscore, JsonHelper.CamelCaseSettings);

        Debug.Log("➡️ PUT data: " + data);

        IWebRequestReponse response = await webClient.SendPutRequest(route, data);

        Debug.Log("⬅️ PUT response: " + response);

        return ParseResponse(response);
    }

    public async Awaitable<IWebRequestReponse> GetHighscore()
    {
        Debug.Log("📥 GET Highscore");

        string route = "/highscores";

        IWebRequestReponse response = await webClient.SendGetRequest(route);

        Debug.Log("⬅️ GET response: " + response);

        return ParseResponse(response);
    }

    private IWebRequestReponse ParseResponse(IWebRequestReponse response)
    {
        if (response == null) return null;

        switch (response)
        {
            case WebRequestData<string> data:
                try
                {
                    // 1. Parse als de tijdelijke DO lijst (met string scores)
                    var rawList = JsonConvert.DeserializeObject<List<HighscoreDTO>>(data.Data);

                    if (rawList != null && rawList.Count > 0)
                    {
                        HighscoreDTO firstRaw = rawList[0];

                        // 2. Maak het echte UserHighScores object aan
                        UserHighScores hs = new UserHighScores();
                        hs.UserId = firstRaw.UserId;

                        // 3. Converteer de string naar float
                        hs.Score = ParseTimeToFloat(firstRaw.Score);

                        return new WebRequestData<UserHighScores>(hs);
                    }

                    return new WebRequestData<UserHighScores>(new UserHighScores());
                }
                catch (JsonException ex)
                {
                    Debug.LogError("❌ JSON Parse Error: " + ex.Message);
                    return new WebRequestError("Fout bij verwerken data");
                }

            case WebRequestError error:
                return error;

            default:
                return response;
        }
    }

    // De logica om de tijd-string om te zetten
    private float ParseTimeToFloat(string timeString)
    {
        if (string.IsNullOrEmpty(timeString) || !timeString.Contains(":")) return 0f;

        try
        {
            string[] parts = timeString.Split(':');
            float minutes = float.Parse(parts[0]);
            // Gebruik InvariantCulture voor de punt/komma scheiding
            float seconds = float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);

            return (minutes * 60f) + seconds;
        }
        catch
        {
            return 0f;
        }
    }
}
[System.Serializable]
public class HighscoreDTO
{
    public string UserId;
    public string Score;
}
