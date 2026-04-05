using System;
using UnityEngine;
using UnityEngine.UI;

public class SetAvatar : MonoBehaviour
{
    public Image avatarImage;
    public Sprite[] avatars;
    private void Start()
    {
        LoadAvatar();
        
    }
    public void LoadAvatar()
    { 
        int avatar = PlayerPrefs.GetInt("Avatar");
        avatarImage.sprite = avatars[avatar];
    }
}
