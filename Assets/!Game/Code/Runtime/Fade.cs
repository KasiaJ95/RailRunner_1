using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Fade : MonoBehaviour {
    [SerializeField] UnityEngine.CanvasGroup _CanvasGroup;

    public void FadeIn(float time, UnityAction onFinish = null) {
        StopAllCoroutines();
        StartCoroutine(FadeUpdate(1.0f, time, onFinish));
    }
    public void FadeOut(float time, UnityAction onFinish = null) {
        StopAllCoroutines();
        StartCoroutine(FadeUpdate(0.0f, time, onFinish));
    }

    private IEnumerator FadeUpdate(float finalAlpha, float time, UnityAction onFinish = null) {
        float startAlpha = _CanvasGroup.alpha;
        float currentTime = 0.0f;
        while(currentTime < time) {
            currentTime += Time.deltaTime;
            float lerp = currentTime / time;
            _CanvasGroup.alpha = Mathf.Lerp(startAlpha, finalAlpha, lerp);
            yield return null;
        }
        _CanvasGroup.alpha = finalAlpha;

        if (onFinish != null) {
            onFinish();
        }
    }
}
