using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public static int DifficultySelected;
    [SerializeField] private string _GameplaySceneName;
    [SerializeField] private Fade _Fade;
    [SerializeField]
    private UnityEngine.UI.Text _LastLoginInfo;

    private void Start() {
        _Fade.FadeOut(1.0f);

        string bottomText = string.Empty;
        if (PlayfabHelper.NewUser) {
            bottomText = "WELCOME & HAVE FUN " + PlayfabHelper.UserName + "!";
        } else {
            bottomText = PlayfabHelper.LastLoginInfo.HasValue ?
                "WELCOME BACK " + PlayfabHelper.UserName + "!\nLAST LOGIN: " + PlayfabHelper.LastLoginInfo.ToString() :
                "WELCOME!";
        }
        _LastLoginInfo.text = bottomText;
    }
    public void PlayClickHandler(int difficulty) {
        _Fade.FadeIn(1.0f, 
            () => {
                DifficultySelected = difficulty;
                UnityEngine.SceneManagement.SceneManager.LoadScene(_GameplaySceneName);
            }
        );
    }

    public void ExitClickHandler() {
        Application.Quit();
    }
}
