using UnityEngine;

public class TimerStopScript : MonoBehaviour
{
    public Timer timer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("💥 Trigger geraakt");

        if (timer == null)
        {
            Debug.LogError("❌ Timer is NULL!");
            return;
        }

        timer.StopTimer();
    }
}