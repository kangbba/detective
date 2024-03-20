using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StoryBackground : MonoBehaviour
{
    public Image backgroundImage;
    private float fadeDuration;
    public AnimationCurve fadeInCurve;
    public AnimationCurve fadeOutCurve;
    private Coroutine currentFadeCoroutine;
    private Coroutine currentShakeCoroutine;
    private Coroutine currentZoomCoroutine;
    private Vector3 originalScale;

    // 스프라이트 및 기타 설정을 초기화합니다.
    public void Initialize(Sprite sprite, float duration = 1f, Vector3 scale = default, AnimationCurve fadeInCurv = null, AnimationCurve fadeOutCurv = null)
    {
        fadeDuration = duration;
        fadeInCurve = fadeInCurv ?? AnimationCurve.Linear(0, 0, 1, 1);
        fadeOutCurve = fadeOutCurv ?? AnimationCurve.Linear(0, 0, 1, 1);
        originalScale = scale == default ? Vector3.one : scale;

        backgroundImage.sprite = sprite;
        backgroundImage.color = new Color(1f, 1f, 1f, 0f);
        backgroundImage.transform.localScale = originalScale;
    }

    // 페이드 인 기능
    public void FadeIn()
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }
        currentFadeCoroutine = StartCoroutine(FadeImage(1.0f, fadeInCurve));
    }

    // 페이드 아웃 기능
    public void FadeOut()
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }
        currentFadeCoroutine = StartCoroutine(FadeImage(0.0f, fadeOutCurve));
    }

    // 실제로 페이드 인/아웃을 처리하는 코루틴
    private IEnumerator FadeImage(float targetAlpha, AnimationCurve curve)
    {
        float startTime = Time.time;
        float endTime = startTime + fadeDuration;
        float initialAlpha = backgroundImage.color.a;

        while (Time.time < endTime)
        {
            float timeFraction = (Time.time - startTime) / fadeDuration;
            float curveFraction = curve.Evaluate(timeFraction);
            float alpha = Mathf.Lerp(initialAlpha, targetAlpha, curveFraction);
            backgroundImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        backgroundImage.color = new Color(1f, 1f, 1f, targetAlpha);
        currentFadeCoroutine = null;
    }

#if UNITY_EDITOR
    // 테스트용 업데이트 함수
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) { Shake(30f, 1f); }
        if (Input.GetKeyDown(KeyCode.W)) { ZoomIn(3f * Vector3.one, Random.insideUnitCircle, 1f); }
        if (Input.GetKeyDown(KeyCode.E)) { ZoomRestore(1f); }
    }
#endif

    // 흔들기(shake) 기능
    public void Shake(float strength, float duration)
    {
        if (currentShakeCoroutine != null)
        {
            StopCoroutine(currentShakeCoroutine);
        }
        currentShakeCoroutine = StartCoroutine(ShakeCoroutine(strength, duration));
    }

    // 실제로 흔들기를 처리하는 코루틴
    private IEnumerator ShakeCoroutine(float strength, float duration)
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            backgroundImage.transform.localPosition = originalScale + (Vector3)Random.insideUnitCircle * strength;
            yield return null;
        }

        backgroundImage.transform.localPosition = Vector3.zero; // 위치를 원래대로 복원합니다.
        currentShakeCoroutine = null;
    }

    // 줌 인(zoom in) 기능
    public void ZoomIn(Vector3 targetScale, Vector2 targetFocusRatio, float duration, float zoomSpeed = 1.0f, float movingSpeed = 1.0f)
    {
        if (currentZoomCoroutine != null)
        {
            StopCoroutine(currentZoomCoroutine); // 이전 줌 코루틴이 있다면 중단
        }
        currentZoomCoroutine = StartCoroutine(ZoomCoroutine(targetScale, targetFocusRatio, duration, zoomSpeed, movingSpeed));
    }

    // 실제로 줌 인을 처리하는 코루틴
    private IEnumerator ZoomCoroutine(Vector3 targetScale, Vector2 targetFocusRatio, float duration, float zoomSpeed, float movingSpeed)
    {
        Vector3 startPosition = backgroundImage.transform.position; // 시작 위치
        Vector3 targetPosition = new Vector3(targetFocusRatio.x * Screen.width, targetFocusRatio.y * Screen.height, startPosition.z); // 목표 위치
        float startTime = Time.time;
        float endTime = startTime + duration;
        Vector3 initialScale = backgroundImage.transform.localScale; // 시작 스케일

        while (Time.time < endTime)
        {
            float timeFraction = (Time.time - startTime) / duration;
            backgroundImage.transform.localScale = Vector3.Lerp(initialScale, targetScale, timeFraction * zoomSpeed);
            backgroundImage.transform.position = Vector3.Lerp(startPosition, targetPosition, timeFraction * movingSpeed);
            yield return null;
        }

        backgroundImage.transform.localScale = targetScale; // 최종 스케일 설정
        backgroundImage.transform.position = targetPosition; // 최종 위치 설정
        currentZoomCoroutine = null;
    }

    public void ZoomRestore(float duration, float zoomSpeed = 1.0f, float movingSpeed = 1.0f)
    {
        if (currentZoomCoroutine != null)
        {
            StopCoroutine(currentZoomCoroutine); // 이전 줌 코루틴이 있다면 중단
        }
        currentZoomCoroutine = StartCoroutine(ZoomCoroutine(originalScale, Vector2.one / 2, duration, zoomSpeed, movingSpeed)); // 원래 스케일로 복원합니다.
    }
}
