using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [System.Serializable]
    public class EmotionData
    {
        public string emotionStyle; // 감정 스타일
        public string[] emotionFileNames; // 해당 감정 스타일의 스프라이트 파일 이름 배열
    }

    [SerializeField]
    private Image characterImg; // 캐릭터 이미지 UI 컴포넌트

    [SerializeField]
    private string _characterID; // 캐릭터 이름
    public string CharacterID { get { return _characterID; } } // 캐릭터 이름
    public EmotionData[] emotionDatas; // 각각의 감정 데이터들

    private Coroutine fadeInCoroutine;
    private Coroutine fadeOutCoroutine;

    // 특정 감정 스타일에 맞는 EmotionData 객체를 찾아 반환합니다.
    public EmotionData GetEmotionData(string emotionStyle)
    {
        foreach (var data in emotionDatas)
        {
            if (data.emotionStyle.Equals(emotionStyle))
            {
                return data;
            }
        }
        return null; // 찾지 못한 경우 null 반환
    }

    // 캐릭터의 감정 상태를 갱신합니다. emotionStyle과 index를 기반으로 캐릭터 이미지를 설정합니다.
    public void SetEmotionData(string emotionStyle, int index)
    {
        EmotionData data = GetEmotionData(emotionStyle);

        if (data != null && index >= 0 && index < data.emotionFileNames.Length)
        {
            Sprite newSprite = Resources.Load<Sprite>($"Characters/{_characterID}/{data.emotionFileNames[index]}");
            if (newSprite != null)
            {
                characterImg.sprite = newSprite; // 적절한 스프라이트로 이미지 업데이트
                characterImg.SetNativeSize();
            }
            else
            {
                Debug.LogWarning($"Sprite not found: Characters/{_characterID}/{data.emotionFileNames[index]}");
            }
        }
        else
        {
            Debug.LogWarning($"Invalid emotion data: Style={emotionStyle}, Index={index}");
        }
    }
    public void Initialize()
    {
        SetEmotionData("Normal", 0);
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
