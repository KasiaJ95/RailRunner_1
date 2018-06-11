using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public static int DifficultySelected;
    [SerializeField] private string _GameplaySceneName;
    [SerializeField] private Fade _Fade;

    private void Start() {
        _Fade.FadeOut(1.0f);
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
