using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
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

    [Header("Test data")]
    public User user;

    [Header("Dependencies")]
    public UserApiClient userApiClient;

    #region Login

    [ContextMenu("User/Register")]
    public async void Register()
    {
        user.email = emailInput.text;
        user.password = passwordInput.text;
        if (!int.TryParse(ageInput.text, out int result))
        {
            Debug.Log("Invalid age input. Please enter a valid number.");
        }
        else
        {
            user.settings.age = result;
        }
        user.settings.firstName = firstNameInput.text;
        user.settings.lastName = lastNameInput.text;
        user.settings.nameDoctor = nameDoctorInput.text;
        user.settings.operationType = operationTypeDropDown.itemText.text;
        user.settings.operationDate = operationDateInput.text;

        IWebRequestReponse webRequestResponse = await userApiClient.Register(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Register succes!");
                IWebRequestReponse userInfoWebRequestResponse = await userApiClient.UpdateUserSettings(user.settings);
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
        user.email = emailInput.text;
        user.password = passwordInput.text;
        IWebRequestReponse webRequestResponse = await userApiClient.Login(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Login succes!");
                ReadUser();
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

    #endregion
    #region Read user
    [ContextMenu("User/Read user")]
    public async Task<IWebRequestReponse> ReadUser()
    {
        IWebRequestReponse webRequestResponse = await userApiClient.GetUserData();

        switch (webRequestResponse)
        {
            case WebRequestData<User> dataResponse:
                user = dataResponse.Data;
                Debug.Log("Gebruiker: " + user);
                PlayerPrefs.SetString("firstName", user.settings.firstName);
                PlayerPrefs.SetString("lastName", user.settings.lastName);
                PlayerPrefs.SetInt("age", user.settings.age);
                PlayerPrefs.SetInt("avatar", user.stats.avatar);
                PlayerPrefs.SetString("balanceMinigameHighscore", user.stats.balanceMinigameHighscore);
                // TODO: Succes scenario. Show the enviroments in the UI
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Read user error: " + errorMessage);
                // TODO: Error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
        return webRequestResponse;
    }
    //[ContextMenu("Object2D/Read all")]
    //public async void ReadObject2Ds()
    //{
    //    IWebRequestReponse webRequestResponse = await object2DApiClient.ReadObject2Ds(object2D.EnvironmentId);

    //    switch (webRequestResponse)
    //    {
    //        case WebRequestData<List<Object2D>> dataResponse:
    //            List<Object2D> object2Ds = dataResponse.Data;
    //            Debug.Log("List of object2Ds: " + object2Ds);
    //            object2Ds.ForEach(object2D => Debug.Log(object2D.Id));
    //            // TODO: Succes scenario. Show the enviroments in the UI
    //            break;
    //        case WebRequestError errorResponse:
    //            string errorMessage = errorResponse.ErrorMessage;
    //            Debug.Log("Read object2Ds error: " + errorMessage);
    //            // TODO: Error scenario. Show the errormessage to the user.
    //            break;
    //        default:
    //            throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
    //    }
    //}
    #endregion


}

