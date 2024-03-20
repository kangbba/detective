using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryPanel : MonoBehaviour
{
    private StoryManager storyManager;
    public TextMeshProUGUI characterText;
    public TextMeshProUGUI lineText;
    public float textSpeed = 0.03f;

    private List<ConversationSection> conversationSections; // 대화 섹션들을 저장할 리스트

    public Image characterPanel;
    public Image backgroundPanel;
    public StoryBackground backgroundImagePrefab; // 배경 이미지 프리팹 참조
    private StoryBackground curStoryBackground; // 현재 배경 인스턴스

    private bool isTyping = false; // 타이핑 진행 중인지를 나타내는 플래그

    public void Initialize(List<ConversationSection> newConversationSections, StoryManager manager)
    {
        storyManager = manager;
        conversationSections = newConversationSections;
        StartCoroutine(DisplayConversationSectionsCoroutine());
    }

    private IEnumerator DisplayConversationSectionsCoroutine()
    {
        foreach (var section in conversationSections)
        {
            Debug.Log($"Starting section {section.sectionIndex} with characters: {string.Join(", ", section.sectionCharacters)}");

            foreach (var conversationData in section.conversationDatas)
            {
                SetBackgroundImage(conversationData.file_name_location);
                SetCharacterText(conversationData.characterID);

                isTyping = true;
                StartCoroutine(TypeLine(conversationData.line));

                bool waitForNextLine = false;
                while (!waitForNextLine)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (isTyping)
                        {
                            lineText.text = conversationData.line; // Immediately finish typing
                            isTyping = false;
                        }
                        else
                        {
                            waitForNextLine = true; // Move to the next line
                        }
                    }
                    yield return null;
                }
            }

            // Optionally add a delay or transition between sections
            yield return new WaitForSeconds(1f); // Wait before starting next section
        }
    }

    private void SetCharacterText(string characterID)
    {
        StoryManager.CharacterData characterData = storyManager.GetCharacterData(characterID);
        if (characterData != null)
        {
            characterText.text = characterData.characterName_korean;
            characterText.color = characterData.color;
        }
        else
        {
            characterText.text = "Unknown Character";
            characterText.color = Color.white;
        }
    }

    private void SetBackgroundImage(string fileName)
    {
        if (!string.IsNullOrEmpty(fileName))
        {
            Sprite newSprite = Resources.Load<Sprite>($"Backgrounds/{fileName}");
            if (newSprite)
            {
                if (curStoryBackground != null)
                {
                    Destroy(curStoryBackground.gameObject); // Remove the old background
                }
                StoryBackground newBackground = Instantiate(backgroundImagePrefab, backgroundPanel.transform).GetComponent<StoryBackground>();
                newBackground.Initialize(newSprite);
                curStoryBackground = newBackground;
            }
            else
            {
                Debug.LogWarning("Background image not found: " + fileName);
            }
        }
    }

    private IEnumerator TypeLine(string line)
    {
        lineText.text = ""; // Clear the text
        foreach (char letter in line)
        {
            if (!isTyping)
            {
                yield break; // Stop typing if isTyping turned off
            }
            lineText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false; // Finish typing
    }
}
