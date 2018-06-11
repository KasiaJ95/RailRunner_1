using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour {

    [SerializeField] private Fade _Fade;

    [SerializeField] private UnityEngine.UI.Image _HealthFill;
    [SerializeField] private UnityEngine.UI.Image _ProgressFill;
    [SerializeField] private GameObject _BoostButton;
    [SerializeField] private CanvasGroup _BloodLayer;
    [SerializeField] private Camera _Camera;
    [SerializeField] private Vector3 _CameraStartPos;
    [SerializeField] private Color[] _BackgroundColors;
    [SerializeField] private Color[] _GroundColors;
    [SerializeField] private MeshRenderer _GroundRenderer;

    private float _Health;
    private float _Progress;
    private Coroutine _HealthBarCoroutine;

    private float _BaseSpeed;

    private Coroutine _CameraShakeCoroutine;
    private bool _Win;

    public void RunGameOver() {
        if (_Win) { return; }
        Time.timeScale = 1.0f;
        _Fade.FadeIn(0.5f, () => {
            Time.timeScale = 1.0f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        });
    }
    private void UpdateHealthBar() {
        if (_HealthBarCoroutine != null) {
            StopCoroutine(_HealthBarCoroutine);
        }
        _HealthBarCoroutine = StartCoroutine(HealthBarFillUpdate(_Health));
    }
    private void UpdateProgressBar() {
        _ProgressFill.fillAmount = _Progress;
    }

    public void UseBoost() {
        _BoostButton.SetActive(false);
        StartCoroutine(BoostUpdate());
    }
    private IEnumerator BoostUpdate() {

        float finalSpeed = _BaseSpeed * 2.0f;

        float time = 0.0f;
        float maxTime = 1.0f;
        while (time < maxTime) {
            time += Time.fixedUnscaledDeltaTime;
            float lerp = time / maxTime;
            Time.timeScale = Mathf.Lerp(_BaseSpeed, finalSpeed, lerp);
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        time = 0.0f;
        maxTime = 1.0f;
        while (time < maxTime) {
            time += Time.fixedUnscaledDeltaTime;
            float lerp = time / maxTime;
            Time.timeScale = Mathf.Lerp(finalSpeed, _BaseSpeed, lerp);
            yield return null;
        }
        Time.timeScale = _BaseSpeed;
    }
    public void HitHandler() {
        float damage = Random.Range(0.1f, 0.4f);
        Health -= damage;
        if (_CameraShakeCoroutine != null) {
            StopCoroutine(_CameraShakeCoroutine);
        }
        _CameraShakeCoroutine = StartCoroutine(CameraShakeUpdate(damage * 50.0f));
        if (Health == 0.0f) {
            RunGameOver();
        }
    }

    private IEnumerator CameraShakeUpdate(float force) {
        float time = 0.0f;
        float maxTime = 0.5f;
        while (time < maxTime) {
            float dt = Time.deltaTime;
            time += dt;
            _Camera.transform.position += new Vector3(
                Random.Range(-force, force) * dt,
                Random.Range(-force, force) * dt,
                Random.Range(-force, force) * dt
                );
            yield return null;
        }

        Vector3 camPos = _Camera.transform.position;
        time = 0.0f;
        maxTime = 0.3f;
        while (time < maxTime) {
            float dt = Time.deltaTime;
            time += dt;
            float lerp = time / maxTime;
            _Camera.transform.position = Vector3.Lerp(camPos, _CameraStartPos, lerp);
            yield return null;
        }

        _Camera.transform.position = _CameraStartPos;
    }
    private IEnumerator HealthBarFillUpdate(float finalValue) {
        float startValue = _HealthFill.fillAmount;
        float time = 0.0f;
        float maxTime = 0.5f;
        while(time < maxTime) {
            time += Time.deltaTime;
            float lerp = time / maxTime;
            _HealthFill.fillAmount = Mathf.Lerp(startValue, finalValue, lerp);
            _BloodLayer.alpha = 1.0f - Mathf.Lerp(startValue, finalValue, lerp);
            yield return null;
        }
        _HealthFill.fillAmount = finalValue;
        _BloodLayer.alpha = 1.0f - finalValue;
    }
    private void Update() {
        Progress += Time.deltaTime * 0.02f;
    }

    private void ApplyMap() {

        int mapID = MenuController.DifficultySelected;
        _BaseSpeed = new float[] { 1.0f, 1.5f, 2.0f }[mapID];

        Color bgColor = _BackgroundColors[mapID];
        RenderSettings.fogColor = bgColor;
        _Camera.backgroundColor = bgColor;

        Color groundColor = _GroundColors[mapID];
        _GroundRenderer.material.color = groundColor;
    }

    private IEnumerator Start() {

        ApplyMap();

        Health = 1.0f;
        Progress = 0.0f;
        yield return new WaitForSeconds(3.0f);
        _Fade.FadeOut(1.0f);

        Time.timeScale = _BaseSpeed;
        StopAllCoroutines();
    }

    public float Health {
        get { return _Health; }
        set {
            value = Mathf.Clamp(value, 0.0f, 1.0f);
            _Health = value;
            UpdateHealthBar();
        }
    }
    public float Progress {
        get { return _Progress; }
        set {
            value = Mathf.Clamp(value, 0.0f, 1.0f);
            _Progress = value;
            UpdateProgressBar();
            if (_Progress == 1.0f) {
                if (!_Win) {
                    _Win = true;
                    Time.timeScale = 1.0f;
                    _Fade.FadeIn(0.5f, () => {
                        Time.timeScale = 1.0f;
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Win");
                    });
                }
            }
        }
    }
}
