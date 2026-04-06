using UnityEngine;

[CreateAssetMenu(fileName = "NieuweSceneInfo", menuName = "MRI-App/SceneInfo")]
public class SceneInformatie : ScriptableObject
{
    public string titel;
    [TextArea(5, 10)]
    public string beschrijving;
    [TextArea(3, 5)]
    public string leukWeetje;
}
