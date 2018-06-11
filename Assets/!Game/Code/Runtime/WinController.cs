using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour {
    [SerializeField] private Fade _Fade;

    // Use this for initialization
    private IEnumerator Start () {
        _Fade.FadeOut(1.0f);
        yield return new WaitForSeconds(3.0f);
        _Fade.FadeIn(1.0f,
            ()=>{
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        );
    }
}
