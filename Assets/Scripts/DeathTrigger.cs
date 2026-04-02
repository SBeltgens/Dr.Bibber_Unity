using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] GameObject avatar;
    [SerializeField] Button retryButton;
    [SerializeField] GameObject recordTime;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        avatar.SetActive(false);
        recordTime.SetActive(true);
        retryButton.gameObject.SetActive(true);
    }
}
