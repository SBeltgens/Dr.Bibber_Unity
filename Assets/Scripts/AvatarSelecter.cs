using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSelecter : MonoBehaviour
{
    public AvatarApiClient avatarApiClient;
    public User user;
    [SerializeField] private Button selectButton;
    [SerializeField] private Button foxButton;
    [SerializeField] private Button racconButton;
    [SerializeField] private Button wolfButton;
    public int currentAvatarIndex;

    public void ToglleSelectButton()
    {
            selectButton.gameObject.SetActive(true);       
    }

    async public void SaveAvatar()
    {
        user.avatar.AvatarId = currentAvatarIndex;
        PlayerPrefs.SetInt("Avatar", currentAvatarIndex);
        IWebRequestReponse updateResponse = await avatarApiClient.UpdateAvatar(user.avatar);

        // 2. Controleer de response via Pattern Matching
        if (updateResponse is WebRequestData<UserAvatar> successData)
        {
            // Succes: De server heeft de wijziging geaccepteerd
            Debug.Log("<color=green>Avatar succesvol geüpdatet op de server!</color> Nieuwe AvatarID: " + successData.Data.AvatarId);
        }
        else if (updateResponse is WebRequestError errorData)
        {
            // Fout: Er is iets misgegaan (waarschijnlijk niet ingelogd of server error)
            Debug.LogError($"[Avatar Update Fout] Code: {errorData.ErrorMessage} | Bericht: {errorData.ErrorMessage}");
        }
        else
        {
            Debug.LogWarning("Onbekende response ontvangen tijdens avatar update.");
        }
    }


    public void ColorSelectedAvatar(int avatarIndex)
    {
        Color32 unselectedColor = new Color32(200, 200, 200, 160);
        currentAvatarIndex = avatarIndex;
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
