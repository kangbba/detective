using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

[System.Serializable]
public class ConversationData
{
    public string characterID;
    public string emotionStyle;
    public int emotionIndex;
    public string file_name_location;
    public string line;
    public string command;
}

[System.Serializable]
public class ConversationSection
{
    public int sectionIndex;
    public ConversationData[] conversationDatas; // 대화 데이터 배열 이름 변경
    public string[] sectionCharacters; // 섹션 내 고유 캐릭터 ID 목록
}

[System.Serializable]
public class CharacterData
{
    public string characterID;
    public string characterName_korean;
    public Color color;
    public Character characterPrefab; // 캐릭터 프리팹 참조
}

public class StoryManager : MonoBehaviour
{
    public StoryPanel storyPanel;
    public TextAsset xmlFile;
    public List<ConversationSection> conversationSections = new List<ConversationSection>(); // 모든 대화 섹션 데이터를 저장하는 리스트
    public CharacterData[] characterDatas;

    void Start()
    {
        LoadConversationsFromXML();
        InitializeStory();
    }

    void InitializeStory()
    {
        // 스토리 시작시 첫 번째 대화 섹션으로 초기화
        if (conversationSections.Any())
        {
            storyPanel.Initialize(conversationSections, this);
        }
    }

    void LoadConversationsFromXML()
    {
        XDocument xmlDoc = XDocument.Parse(xmlFile.text);
        var allElements = xmlDoc.Element("data").Elements();

        foreach (var sectionElement in allElements)
        {
            // 언더바 두 개를 가진 태그는 무시합니다.
            if (sectionElement.Name.ToString().StartsWith("__")) continue;

            int sectionIndex = int.Parse(sectionElement.Name.ToString().Substring(1)); // "_0", "_1" 등에서 숫자 추출

            List<ConversationData> conversationDataList = new List<ConversationData>();
            HashSet<string> characterIds = new HashSet<string>();

            foreach (var item in sectionElement.Elements())
            {
                var conversationData = new ConversationData
                {
                    characterID = item.Element("character_id")?.Value,
                    emotionStyle = item.Element("emotion_style")?.Value,
                    emotionIndex = (int?)item.Element("emotion_index") ?? 0,
                    file_name_location = item.Element("file_name_location")?.Value,
                    line = item.Element("line")?.Value,
                    command = item.Element("command")?.Value
                };

                conversationDataList.Add(conversationData);
                characterIds.Add(conversationData.characterID);
            }

            conversationSections.Add(new ConversationSection
            {
                sectionIndex = sectionIndex,
                conversationDatas = conversationDataList.ToArray(),
                sectionCharacters = characterIds.Where(id => !string.IsNullOrEmpty(id)).ToArray()
            });
        }
    }

    public CharacterData GetCharacterData(string characterID)
    {
        return characterDatas.FirstOrDefault(cd => cd.characterID == characterID);
    }
}
