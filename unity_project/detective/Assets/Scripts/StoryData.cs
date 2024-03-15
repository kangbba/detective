using UnityEngine;
using static StoryManager;

[CreateAssetMenu(fileName = "NewStoryData", menuName = "StoryData")]
public class StoryData : ScriptableObject
{
    // 씬 정보
    [System.Serializable]
    public class SceneInfo
    {
        public Sprite sceneSprite; // 스프라이트 변수 추가
        public DialogueInfo[] dialogueInfos; // 대화 정보 배열
    }

    // 스토리 대사 정보
    [System.Serializable]
    public class DialogueInfo
    {
        public ECharacter character;
        public ECharacterExpression expression;
        public string[] dialogueText; // 대화 텍스트 배열로 변경
    }

    // 스토리 대사를 저장할 리스트
    public SceneInfo[] scenes;
}
