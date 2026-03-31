using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSelecter : MonoBehaviour
{
    [SerializeField] private Button selectButton;
    [SerializeField] private Button foxButton;
    [SerializeField] private Button racconButton;
    [SerializeField] private Button wolfButton;

    public void ToglleSelectButton()
    {
            selectButton.gameObject.SetActive(true);       
    }



public void ColorSelectedAvatar(int avatarIndex)
    {
        Color32 unselectedColor = new Color32(200, 200, 200, 160);
        PlayerPrefs.SetInt("avatar", avatarIndex);
        if (avatarIndex == 0)
        {
            
            foxButton.GetComponent<Image>().color = Color.white;
            racconButton.GetComponent<Image>().color = unselectedColor;
            wolfButton.GetComponent<Image>().color = unselectedColor;
        }
        else if (avatarIndex == 1)
        {
                racconButton.GetComponent<Image>().color = Color.white;
                foxButton.GetComponent<Image>().color = unselectedColor;
                wolfButton.GetComponent<Image>().color = unselectedColor;
        }
        else if (avatarIndex == 2)
        {
            wolfButton.GetComponent<Image>().color = Color.white;
            foxButton.GetComponent<Image>().color = unselectedColor;
            racconButton.GetComponent<Image>().color = unselectedColor;
        }
    }
}
