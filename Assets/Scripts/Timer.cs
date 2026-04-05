using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text bestTimeText;
    public HighscoreApiClient apiClient;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    private void Start()
    {
        Debug.Log("⏱ Timer gestart");
        StartTimer();

        
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay(elapsedTime);
        }
    }

    public void StartTimer()
    {
        Debug.Log("▶️ Timer loopt");
        isRunning = true;
    }

    public void StopTimer()
    {
        Debug.Log("⏹ Timer gestopt");
        isRunning = false;
        SaveTime();
    }

    public void ResetTimer()
    {
        Debug.Log("🔄 Timer reset");
        elapsedTime = 0f;
        UpdateTimerDisplay(elapsedTime);
    }

    void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    async void SaveTime()
    {
        Debug.Log("💾 SaveTime gestart");
        
        float bestTime = PlayerPrefs.GetFloat("Score");

        bool isNewHighscore = elapsedTime > bestTime;

        if (!isNewHighscore)
        {
            Debug.Log("❌ Geen nieuwe highscore");
            return;
        }

        Debug.Log("✅ Nieuwe highscore!");

        PlayerPrefs.SetFloat("bestTime", elapsedTime);

        // 👉 Als apiClient niet bestaat → gewoon stoppen (GEEN CRASH)
        if (apiClient == null)
        {
            Debug.LogWarning("⚠️ apiClient niet ingesteld → skip API");
            return;
        }

        UserHighScores hs = new UserHighScores
        {
            Score = Mathf.FloorToInt(elapsedTime * 1000)
        };

        bestTimeText.text = hs.Score.ToString();

        try
        {
            Debug.Log("➡️ Probeer highscore op te halen...");

            var response = await apiClient.GetHighscore();

            if (response is WebRequestData<UserHighScores>)
            {
                Debug.Log("🔄 Update highscore");
                await apiClient.UpdateHighscore(hs);
            }
            else
            {
                Debug.Log("🆕 Nieuwe highscore posten");
                await apiClient.PostHighscore(hs);
            }

            Debug.Log("✅ API call klaar");
        }
        catch (System.Exception e)
        {
            // 🔥 BELANGRIJK: hier voorkom je crash
            Debug.LogError("❌ API ERROR (maar game gaat door): " + e.Message);
        }
    }
}