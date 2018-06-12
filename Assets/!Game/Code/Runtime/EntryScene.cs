using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
public class EntryScene : MonoBehaviour {

    [SerializeField] private UnityEngine.UI.InputField _LoginEmailField;
    [SerializeField] private UnityEngine.UI.InputField _LoginPasswordField;
    [SerializeField] private UnityEngine.UI.InputField _RegisterNameField;
    [SerializeField] private UnityEngine.UI.InputField _RegisterEmailField;
    [SerializeField] private UnityEngine.UI.InputField _RegisterPasswordField;
    private const string TitleID = "CD31";

    public void Login () {
        PlayFab.PlayFabSettings.TitleId = TitleID;

        string email = _LoginEmailField.text;
        string password = _LoginPasswordField.text;

        PlayFab.ClientModels.LoginWithEmailAddressRequest loginRequest = new LoginWithEmailAddressRequest();
        loginRequest.Email = email;
        loginRequest.Password = password;
        loginRequest.TitleId = TitleID;

        PlayFab.PlayFabClientAPI.LoginWithEmailAddress(
            loginRequest, 
            (LoginResult result) => {
                PlayfabHelper.LastLoginInfo = result.LastLoginTime;
                PlayfabHelper.NewUser = result.NewlyCreated;
                PlayfabHelper.PlayerID = result.PlayFabId;

                PlayFab.ClientModels.GetPlayerProfileRequest getProfileRequest = new GetPlayerProfileRequest();
                getProfileRequest.PlayFabId = result.PlayFabId;
                PlayFab.PlayFabClientAPI.GetPlayerProfile(
                    getProfileRequest,
                    (GetPlayerProfileResult result2) => {
                        PlayfabHelper.UserName = result2.PlayerProfile.DisplayName;


                        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                    },
                    (PlayFab.PlayFabError error) => {
                        Debug.LogError(error.GenerateErrorReport());
                    }
                    );
            },
            (PlayFab.PlayFabError error) => {
                Debug.LogError(error.GenerateErrorReport());
            }
        );
    }

    public void Create() {

        string userName = _RegisterNameField.text;
        string email = _RegisterEmailField.text;
        string password = _RegisterPasswordField.text;

        PlayFab.ClientModels.RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
        request.Email = email;
        request.Password = password;
        request.TitleId = TitleID;
        request.Username = userName;
        request.DisplayName = userName;

        PlayFab.PlayFabClientAPI.RegisterPlayFabUser(
            request,
            (RegisterPlayFabUserResult result) => {
                _RegisterNameField.text = string.Empty;
                _RegisterEmailField.text = string.Empty;
                _RegisterPasswordField.text = string.Empty;
            },
            (PlayFab.PlayFabError error) => {
                Debug.LogError(error.GenerateErrorReport());
            }
        );
    }
}
