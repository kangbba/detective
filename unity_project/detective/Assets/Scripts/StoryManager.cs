using UnityEngine;

public class StoryManager : MonoBehaviour
{
    // 대화 주인공을 정의하는 Enum
    public enum ECharacter
    {
        Ryan,
        Brian,
        Kate,
        Lisa,
        Rachel,
        Joseph
    }

    // 대화 주인공의 표정을 정의하는 Enum
    public enum ECharacterExpression
    {
        Normal,
        Happy,
        Sad,
        Angry,
        Surprised
    }

    // 대화 주인공과 표정에 대한 정보를 저장하는 클래스
    [System.Serializable]
    public class CharacterInfo
    {
        public ECharacter character;
        public ECharacterExpression expression;
        public Sprite characterSprite;
    }

    // 대화 정보를 저장하는 클래스
    [System.Serializable]
    public class DialogueInfo
    {
        public CharacterInfo characterInfo;
        [TextArea(3, 10)]
        public string dialogueText;
    }

    // 대화 정보를 담는 배열
    public DialogueInfo[] dialogues;

    // 싱글톤 인스턴스
    private static StoryManager instance;

    // 싱글톤 인스턴스를 가져오는 프로퍼티
    public static StoryManager Instance
    {
        get
        {
            // 인스턴스가 없을 경우에는 새로 생성
            if (instance == null)
            {
                GameObject go = new GameObject("StoryManager");
                instance = go.AddComponent<StoryManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        // 인스턴스가 이미 존재하는 경우, 새로 생성하지 않고 기존 인스턴스를 사용
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
    }

    // 특정 대화를 가져오는 메서드
    public DialogueInfo GetDialogue(ECharacter character, ECharacterExpression expression)
    {
        foreach (DialogueInfo dialogue in dialogues)
        {
            if (dialogue.characterInfo.character == character && dialogue.characterInfo.expression == expression)
            {
                return dialogue;
            }
        }
        return null; // 해당 캐릭터와 표정에 대한 대화가 없을 경우
    }
}
