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

    public string characterName; // 캐릭터 이름
    public EmotionData[] emotionDatas; // 각각의 감정 데이터들

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
            Sprite newSprite = Resources.Load<Sprite>($"Characters/{characterName}/{data.emotionFileNames[index]}");
            if (newSprite != null)
            {
                characterImg.sprite = newSprite; // 적절한 스프라이트로 이미지 업데이트
            }
            else
            {
                Debug.LogError($"Sprite not found: Characters/{characterName}/{data.emotionFileNames[index]}");
            }
        }
        else
        {
            Debug.LogError($"Invalid emotion data: Style={emotionStyle}, Index={index}");
        }
    }
}
