using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ExampleApp : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField ageInput;
    public TMP_InputField firstNameInput;
    public TMP_InputField lastNameInput;
    public TMP_InputField nameDoctorInput;
    public TMP_Dropdown operationTypeDropDown;
    public TMP_InputField operationDateInput;
    public TMP_Text responseText;

    [Header("Test data")]
    public User user;

    [Header("Dependencies")]
    public UserApiClient userApiClient;
    public UserSettingsApiClient userSettingsApiClient;
    public HighscoreApiClient highscoreApiClient;
    public AvatarApiClient avatarApiClient;

    #region Login

    [ContextMenu("User/Register")]
    public async void Register()
    {
        if(!SetUserInfo())
        {
            return;
        }

        if (!ValidateRegisterInput())
        {
            return;
        }
        IWebRequestReponse registerWebRequestRepsonse= await userApiClient.Register(user.credentials);
        IWebRequestReponse webRequestResponse = await userApiClient.Login(user.credentials);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Register succes!");
                IWebRequestReponse userInfoWebRequestResponse = await userSettingsApiClient.PostSettings(user.settings);
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Register error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("User/Login")]
    public async void Login()
    {
        user.credentials.email = emailInput.text;
        user.credentials.password = passwordInput.text;
        IWebRequestReponse webRequestResponse = await userApiClient.Login(user.credentials);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Login succes!");
                await ReadUser();
                SaveUserToPrefs();
                // TODO: Todo handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Login error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }
    private bool SetUserInfo()
    {
        user.credentials.email = emailInput.text;
        user.credentials.password = passwordInput.text;
        if (!ValidatePasswordInput())
        {
            return false;
        }
            
        if (!int.TryParse(ageInput.text, out int result))
        {
            responseText.text = "Voer een geldige leeftijd in.";
            return false;
        }
        else
        {
            user.settings.KindLeeftijd = result;
        }
        user.settings.KindVoornaam = firstNameInput.text;
        user.settings.KindAchternaam = lastNameInput.text;
        user.settings.ArtsNaam = nameDoctorInput.text;
        user.settings.BehandelingType = operationTypeDropDown.itemText.text;
        user.settings.BehandelDatum = operationDateInput.text;
        return true;

    }
    private bool ValidateRegisterInput()
    {
        TMP_InputField[] requiredFields = {
        emailInput,
        ageInput,
        firstNameInput,
        lastNameInput,
        nameDoctorInput,
        operationDateInput
    };

        foreach (TMP_InputField field in requiredFields)
        {
            if (string.IsNullOrWhiteSpace(field.text))
            {
                responseText.text = "Vul alle velden in.";
                return false;
            }
        }

        responseText.text = "";
        return true;
    }
    private bool ValidatePasswordInput()
    {
        string password = passwordInput.text;

        if (string.IsNullOrWhiteSpace(password))
        {
            responseText.text = "Vul alle velden in.";
            return false;
        }

        bool hasDigit = false;
        bool hasSpecial = false;

        foreach (char c in password)
        {
            if (char.IsDigit(c)) hasDigit = true;
            if (!char.IsLetterOrDigit(c)) hasSpecial = true;
        }

        if (password.Length < 8)
        {
            responseText.text = "Wachtwoord moet minimaal 8 tekens lang zijn.";
            return false;
        }
        if (!hasDigit)
        {
            responseText.text = "Wachtwoord moet minimaal één cijfer bevatten.";
            return false;
        }
        if (!hasSpecial)
        {
            responseText.text = "Wachtwoord moet een speciaal teken bevatten.";
            return false;
        }
        responseText.text = "";
        return true;
    }
    #endregion
    #region Read user
    [ContextMenu("User/Read user")]
    public async Task<IWebRequestReponse> ReadUser()
    {
        IWebRequestReponse webRequestResponseSettings = await userSettingsApiClient.GetSettings();

        IWebRequestReponse webRequestResponseAvatar = await avatarApiClient.GetAvatar();
        IWebRequestReponse webRequestResponseHighscore = await highscoreApiClient.GetHighscore();


        if (webRequestResponseSettings is WebRequestData<UserSettings> settingsData)
        {
            user.settings = settingsData.Data;
            Debug.Log("User settings retrieved: " + user.settings.KindVoornaam);
        }
        else
        {
            Debug.LogError("Settings response was niet van het type WebRequestData of bevat een fout.");
        }

        if (webRequestResponseAvatar is WebRequestData<UserAvatar> avatarData)
        {
            user.avatar = avatarData.Data;
            Debug.Log("Avatar retrieved: " + user.avatar.AvatarId);
        }
        else
        {
            Debug.LogError("Avatar response was niet van het type WebRequestData.");
        }


        if (webRequestResponseHighscore is WebRequestData<UserHighScores> highscoreData)
        {
            user.highScores = highscoreData.Data;
            Debug.Log("Highscores retrieved: " + user.highScores.Score);
        }
        else
        {
            Debug.LogError("Highscore response was niet van het type WebRequestData.");
        }
        return webRequestResponseSettings;
    }
    public void SaveUserToPrefs()
    {
        if (user == null) return;

        if (user.settings != null)
        {
            PlayerPrefs.SetString("KindVoornaam", user.settings.KindVoornaam);
            PlayerPrefs.SetString("KindAchternaam", user.settings.KindAchternaam);
            PlayerPrefs.SetInt("KindLeeftijd", user.settings.KindLeeftijd);
            PlayerPrefs.SetString("ArtsNaam", user.settings.ArtsNaam);
            PlayerPrefs.SetString("BehandelingType", user.settings.BehandelingType);
            PlayerPrefs.SetString("BehandelDatum", user.settings.BehandelDatum);
        }

        // --- UserAvatar ---
        if (user.avatar != null)
        {
            PlayerPrefs.SetInt("AvatarId", user.avatar.AvatarId);
        }

        if (user.highScores != null)
        {
            PlayerPrefs.SetFloat("Score", user.highScores.Score);
        }
        PlayerPrefs.Save();

        Debug.Log("User data succesvol opgeslagen met class-variabele namen.");
    }
}
        #endregion