using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public StoryPanel storyPanel;
    public List<Section> sections = new List<Section>();
    public TextAsset xmlFile;

    [System.Serializable]
    public class Section
    {
        public int sectionIndex;
        public List<ConversationData> conversationDatas;
        public List<string> sectionCharacterNames;
    }

    [System.Serializable]
    public class ConversationData
    {
        public string backgroundID;
        public string characterID;
        public string emotionID;
        public string characterLocation;
        public string lines;
        public string command;
    }

    [System.Serializable]
    public class CharacterData
    {
        public string character_id;
        public string characterName_ko;
        public Color color;
        public Character characterPrefab;
    }

    public List<CharacterData> characterDatas = new List<CharacterData>();

    void Start()
    {
        LoadConversationsFromXML();
    }

    void LoadConversationsFromXML()
    {
        XDocument xmlDoc = XDocument.Parse(xmlFile.text);
        var dialogues = xmlDoc.Element("data").Element("__1").Elements("_");

        Section currentSection = null;

        foreach (var dialogue in dialogues)
        {
            var sectionIndexStr = dialogue.Element("section_index")?.Value;

            if (!string.IsNullOrEmpty(sectionIndexStr) && int.TryParse(sectionIndexStr, out int sectionIndex))
            {
                // If new section starts, save the old section and start a new one
                if (currentSection != null && currentSection.sectionIndex != sectionIndex)
                {
                    sections.Add(currentSection);
                }
                if (currentSection == null || currentSection.sectionIndex != sectionIndex)
                {
                    currentSection = new Section
                    {
                        sectionIndex = sectionIndex,
                        conversationDatas = new List<ConversationData>(),
                        sectionCharacterNames = new List<string> { }
                    };
                }
            }

            if (currentSection != null)
            {
                var conversationData = new ConversationData
                {
                    backgroundID = dialogue.Element("background_id")?.Value,
                    characterID = dialogue.Element("character_id")?.Value,
                    characterLocation = dialogue.Element("character_location")?.Value,
                    emotionID = dialogue.Element("emotion_id")?.Value,
                    lines = dialogue.Element("lines")?.Value,
                    command = dialogue.Element("command")?.Value
                };

                currentSection.conversationDatas.Add(conversationData);
                currentSection.sectionCharacterNames = currentSection.conversationDatas.Select(d => d.characterID).Distinct().ToList();
            }
        }

        // Add the last section to the list
        if (currentSection != null)
        {
            sections.Add(currentSection);
        }
        storyPanel.Initialize(this);

        Debug.Log($"Finished loading {sections.Count} sections from XML.");
    }

    public CharacterData GetCharacterData(string characterID)
    {
        for(int i = 0; i < characterDatas.Count; i++)
        {
            if (characterDatas[i].character_id == characterID)
            {
                return characterDatas[i];
            }
        }
        return null;
    }
}
