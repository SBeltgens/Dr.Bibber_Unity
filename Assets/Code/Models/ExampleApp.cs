using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ExampleApp : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField ageInput;
    public TMP_InputField firstNameInput;
    public TMP_InputField lastNameInput;
    public TMP_InputField nameDoctorInput;
    public TMP_InputField operationTypeInput;
    public TMP_InputField operationDateInput;

    public ExampleApp exampleApp;

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
            user.age = result;
        }
        user.firstName = firstNameInput.text;
        user.lastName = lastNameInput.text;
        user.nameDoctor = nameDoctorInput.text;
        user.operationType = operationTypeInput.text;
        user.operationDate = operationDateInput.text;

        IWebRequestReponse webRequestResponse = await userApiClient.Register(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Register succes!");

                // TODO: Handle succes scenario;
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
        user.email = usernameInput.text;
        user.password = passwordInput.text;
        IWebRequestReponse webRequestResponse = await userApiClient.Login(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Login succes!");

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
        IWebRequestReponse webRequestResponse = await exampleApp.ReadUser();

        switch (webRequestResponse)
        {
            case WebRequestData<User> dataResponse:
                user = dataResponse.Data;
                Debug.Log("Gebruiker: " + user);
                PlayerPrefs.SetString("firstName", user.firstName);
                PlayerPrefs.SetString("lastName", user.lastName);
                PlayerPrefs.SetInt("age", user.age);
                PlayerPrefs.SetInt("avatar", user.avatar);
                PlayerPrefs.SetString("balanceMinigameHighscore", user.balanceMinigameHighscore);
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
    #endregion


}

