using UnityEngine;
using TMPro;

public class SceneUitlegHandler : MonoBehaviour
{
    public SceneInformatie data; // Sleep hier je ScriptableObject in per scene

    [Header("UI Koppelingen")]
    public TextMeshProUGUI titelText;
    public TextMeshProUGUI beschrijvingText;
    public TextMeshProUGUI weetjeText;

    void Start()
    {
        if (data != null)
        {
            titelText.text = data.titel;
            beschrijvingText.text = data.beschrijving;
            weetjeText.text = "Wist je dat? " + data.leukWeetje;
        }
    }
}