using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static StoryManager;

public class StoryPanel : MonoBehaviour
{
    private StoryManager storyManager;
    public TextMeshProUGUI characterText;
    public TextMeshProUGUI lineText;
    private float textSpeed = 0.03f;

    public Image characterPanel;
    public Image backgroundPanel;
    public StoryBackground backgroundImagePrefab; // 배경 이미지 프리팹 참조
    private StoryBackground curStoryBackground; // 현재 배경 인스턴스

    private List<Character> curCharacters = new List<Character>();
    private List<string> curSectionCharactersIDs = new List<string>(); // 현재 화면에 표시된 캐릭터들을 추적

    public void Initialize(StoryManager storyManager)
    {
        this.storyManager = storyManager; // Assign the storyManager reference
        StartCoroutine(DisplaySectionsCoroutine()); // Start displaying sections
    }

    private IEnumerator DisplaySectionsCoroutine()
    {
        List<string> prevSectionCharactersIDs = new List<string>();
        foreach (var section in storyManager.sections)
        {

            curSectionCharactersIDs = section.sectionCharacters;
            curSectionCharactersIDs.RemoveAll(id => id == "Mono" || id == "Ryan");

            Debug.Log($"{section.sectionIndex}번째 섹션에 진입했습니다");
            Debug.Log($"이전 캐릭터들: {string.Join(", ", prevSectionCharactersIDs)}");
            Debug.Log($"새로운 캐릭터들: {string.Join(", ", curSectionCharactersIDs)}");

           
            int sectionCount = curSectionCharactersIDs.Count;

            // 등장하는 자리 계산
            List<Vector3> sectionPositions = new List<Vector3>();
            for (int i = 0; i < sectionCount; i++)
            {
                // Calculate the position based on the section index
                float xPos = Mathf.Lerp(Screen.width * .2f, Screen.width * .8f, (float)i / (sectionCount - 1));
                float yPos = Screen.height / 2; // You can adjust the y position as needed
                Vector3 position = new Vector3(xPos, yPos, 0);
                sectionPositions.Add(position);
            }

            // Instantiate new characters that are entering the scene
            for (int i = 0; i < curCharacters.Count; i++)
            {
                curCharacters[i].FadeOutAndDestroy(1f);
            }
            yield return new WaitForSeconds(1.1f);

            curCharacters = new List<Character>();
            for (int i = 0; i < curSectionCharactersIDs.Count; i++)
            {
                var id = curSectionCharactersIDs[i];
                var characterData = storyManager.GetCharacterData(id);
                if (characterData != null && characterData.characterPrefab != null)
                {
                    var characterPrefab = characterData.characterPrefab.gameObject;
                    Character character = Instantiate(characterPrefab, characterPanel.transform).GetComponent<Character>();
                    character.Initialize();

                    // Assign position based on section index
                    int sectionIndex = i;
                    character.transform.position = sectionPositions[sectionIndex];
                    character.FadeOut(0f);
                    curCharacters.Add(character);
                }
            }


            // Display conversation data for this section
            foreach (var conversation in section.conversationDatas)
            {
                bool showCharacters = conversation.showCharacters;
                // Instantiate new characters that are entering the scene
                Debug.Log($"ShowCharacters ? : {showCharacters}");
                if (showCharacters)
                {
                    Debug.Log($"현재 존재하는 curCharacters {curCharacters.Count}"); 
                    Character character = curCharacters.FirstOrDefault(character => character.CharacterID == conversation.characterID);
                    if (character != null)
                    {
                        character.FadeIn(1f);
                        character.SetEmotionData(conversation.emotionStyle, conversation.emotionIndex);
                    }
                }
                else
                {
                    for (int i = curCharacters.Count-1; i >= 0; i--)
                    {
                        Character character = curCharacters[i];
                        character.FadeOut(1f);
                    }
                }

                if (!string.IsNullOrEmpty(conversation.characterID))
                {
                    var characterData = storyManager.characterDatas.FirstOrDefault(c => c.characterID == conversation.characterID);
                    if (characterData != null)
                    {
                        SetBackgroundImage(conversation.file_name_location);
                        SetCharacterText(characterData);
                    }
                }

                yield return StartCoroutine(TypeLine(conversation.line));
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            }

            yield return new WaitForSeconds(1f);
            prevSectionCharactersIDs = section.sectionCharacters.ToList();
        }
    }

    private void SetCharacterText(CharacterData characterData)
    {
        // Set the character name and apply the color based on characterData
        characterText.text = characterData.characterName; // Use characterName for display
        characterText.color = characterData.color; // Use predefined color for character
    }
    private void SetBackgroundImage(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return;
        }
        Sprite backgroundSprite = Resources.Load<Sprite>("Backgrounds/" + fileName);
        if (!backgroundSprite)
        {
            Debug.LogWarning("해당 Background 에 해당하는 sprite 찾을 수 없음 ");
            return;
        }
        if (curStoryBackground != null)
        {
            curStoryBackground.FadeOut();
        }
        // 새 배경 이미지 생성
        StoryBackground newStoryBackground = Instantiate(backgroundImagePrefab.gameObject, backgroundPanel.transform).GetComponent<StoryBackground>();
        newStoryBackground.Initialize(backgroundSprite);
        newStoryBackground.FadeIn();
        curStoryBackground = newStoryBackground; // 현재 배경 인스턴스 업데이트

    }

    private IEnumerator TypeLine(string line)
    {
        lineText.text = ""; // 초기 텍스트를 비웁니다.
        foreach (char letter in line)
        {
            lineText.text += letter; // 현재 문자를 텍스트에 추가합니다.
            if (!char.IsWhiteSpace(letter)) // 문자가 공백이 아닌 경우에만 대기합니다.
            {
                yield return new WaitForSeconds(textSpeed); // 설정된 텍스트 속도만큼 대기합니다.
            }
        }
    }

}
