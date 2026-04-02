using UnityEngine;

public class TimerStopScript : MonoBehaviour
{
    public Timer timer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        timer.StopTimer();
      
    }
}
