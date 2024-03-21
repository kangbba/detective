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
        public List<string> sectionCharacters;
    }

    [System.Serializable]
    public class ConversationData
    {
        public string file_name_location;
        public bool showCharacters;
        public string characterID;
        public string emotionStyle;
        public int emotionIndex;
        public string line;
        public string command;
    }

    [System.Serializable]
    public class CharacterData
    {
        public string characterID;
        public string characterName;
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
            var sectionIndexString = dialogue.Element("section_index")?.Value;

            if (!string.IsNullOrEmpty(sectionIndexString) && int.TryParse(sectionIndexString, out int sectionIndex))
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
                        sectionCharacters = new List<string> { }
                    };
                }
            }

            if (currentSection != null)
            {
                var conversationData = new ConversationData
                {
                    showCharacters = bool.TryParse(dialogue.Element("show_characters")?.Value, out bool showCharacters) ? showCharacters : true,
                    file_name_location = dialogue.Element("file_name_location")?.Value,
                    characterID = dialogue.Element("character_id")?.Value,
                    emotionStyle = dialogue.Element("emotion_style")?.Value,
                    emotionIndex = int.TryParse(dialogue.Element("emotion_index")?.Value, out int index) ? index : 0,
                    line = dialogue.Element("line")?.Value,
                    command = dialogue.Element("command")?.Value
                };

                currentSection.conversationDatas.Add(conversationData);
                currentSection.sectionCharacters = currentSection.conversationDatas.Select(d => d.characterID).Distinct().ToList();
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
            if (characterDatas[i].characterID == characterID)
            {
                return characterDatas[i];
            }
        }
        return null;
    }
}
