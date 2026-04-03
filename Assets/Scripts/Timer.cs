using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    private void Start()
    {
        StartTimer();
    }
    [SerializeField] private TMP_Text timerText;

        private float elapsedTime = 0f;
        private bool isRunning = false;

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
            isRunning = true;
        }

        public void StopTimer()
        {
            SaveTime();
            isRunning = false;
        }

        public void ResetTimer()
        {
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
        void SaveTime()
        {
        float bestTime = PlayerPrefs.GetFloat("bestTime");


        if (bestTime < elapsedTime)
        {
            PlayerPrefs.SetFloat("bestTime", elapsedTime);
            //stuur naar database
        }
                       
        }
    }
