using UnityEngine;
using UnityEngine.UI;

public class AvatarSelecter : MonoBehaviour
{
    [SerializeField] private Button selectButton;
    public void toglleSelectButton()
    {
            selectButton.gameObject.SetActive(true);
        
    }
}
