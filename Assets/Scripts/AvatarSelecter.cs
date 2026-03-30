using UnityEngine;
using UnityEngine.UI;

public class AvatarSelecter : MonoBehaviour
{
    [SerializeField] private Button selectButton;
    public void toglleSelectButton()
    {
        if (selectButton.gameObject.activeSelf == false)
        {
            selectButton.gameObject.SetActive(!selectButton.gameObject.activeSelf);
        }
        
    }
}
