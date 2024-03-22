using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Image characterImg; // 캐릭터 이미지 UI 컴포넌트

    private string _characaterID; // 캐릭터 이름
    public string CharacterID { get { return _characaterID; } } // 캐릭터 이름

    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;


    // 캐릭터의 감정 상태를 갱신합니다. emotionStyle과 index를 기반으로 캐릭터 이미지를 설정합니다.
    public void SetEmotionData(string emotionID)
    {
        Sprite newSprite = Resources.Load<Sprite>($"Characters/{_characaterID}/{emotionID}");
        if (newSprite != null)
        {
            characterImg.sprite = newSprite; // 적절한 스프라이트로 이미지 업데이트
            characterImg.SetNativeSize();
        }
        else
        {
            Debug.LogWarning($"Sprite not found: Characters/{_characaterID}/{emotionID}");
        }
    }
    public void Initialize(string characaterID)
    {
        _characaterID = characaterID;
    }
    public void FadeIn(float duration)
    {
        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
        }
        fadeInCoroutine = StartCoroutine(FadeInCoroutine(duration));
    }

    public void FadeOut(float duration)
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        fadeOutCoroutine = StartCoroutine(FadeOutCoroutine(duration));
    }
    public void FadeOutAndDestroy(float duration)
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        fadeOutCoroutine = StartCoroutine(FadeOutCoroutine(duration));
        Destroy(gameObject, duration);
    }
    private IEnumerator FadeInCoroutine(float duration)
    {
        float elapsedTime = 0f;
        Color startColor = characterImg.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);
        while (elapsedTime < duration)
        {
            characterImg.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        characterImg.color = targetColor; // Ensure alpha is set to 1 at the end
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float elapsedTime = 0f;
        Color startColor = characterImg.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        while (elapsedTime < duration)
        {
            characterImg.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        characterImg.color = targetColor; // Ensure alpha is set to 0 at the end
    }

}
