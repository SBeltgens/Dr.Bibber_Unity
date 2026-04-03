using Newtonsoft.Json;
using UnityEngine;
using System.Collections.Generic;

public class HighscoreApiClient : MonoBehaviour
{
    public WebClient webClient;

    // POST methode om highscore te sturen
    public async Awaitable<IWebRequestReponse> PostHighscore(Highscore highscore)
    {
        string route = "/highscores";
        string data = JsonConvert.SerializeObject(highscore, JsonHelper.CamelCaseSettings);

        IWebRequestReponse response = await webClient.SendPostRequest(route, data);
        return ParseResponse(response);
    }

    // GET methode om highscore te krijgen
    public async Awaitable<IWebRequestReponse> GetHighscores(string gameName)
    {
        string route = "/highscores"; 
        IWebRequestReponse response = await webClient.SendGetRequest(route);

        switch (response)
        {
            case WebRequestData<string> data:

                List<Highscore> highscores = JsonConvert.DeserializeObject<List<Highscore>>(data.Data);
                if (highscores.Count > 0)
                {
                    highscores.Sort((a, b) => a.score.CompareTo(b.score)); 
                    return new WebRequestData<Highscore>(highscores[0]);
                }
                return null;

            default:
                return response;
        }
    }

    private IWebRequestReponse ParseResponse(IWebRequestReponse response)
    {
        switch (response)
        {
            case WebRequestData<string> data:
                Highscore hs = JsonConvert.DeserializeObject<Highscore>(data.Data);
                return new WebRequestData<Highscore>(hs);
            default:
                return response;
        }
    }
}