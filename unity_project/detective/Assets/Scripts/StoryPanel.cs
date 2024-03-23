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

    private List<Character> inst_characters = new List<Character>();
    private List<string> sectionCharacterNames = new List<string>(); // 현재 화면에 표시된 캐릭터들을 추적

    public void Initialize(StoryManager storyManager)
    {
        this.storyManager = storyManager; // Assign the storyManager reference
        StartCoroutine(DisplaySectionsCoroutine()); // Start displaying sections
    }

    private IEnumerator DisplaySectionsCoroutine()
    {
        List<string> prevCharacterNames = new List<string>();
        foreach (var section in storyManager.sections)
        {

            sectionCharacterNames = section.sectionCharacterNames;
            sectionCharacterNames.RemoveAll(id => id == "Mono" || id == "Ryan");

            Debug.Log($"{section.sectionIndex}번째 섹션에 진입했습니다");
            Debug.Log($"화면에 등장할 새로운 캐릭터들: {string.Join(", ", sectionCharacterNames)}");
           
            int sectionCount = sectionCharacterNames.Count;

     
            for (int i = 0; i < inst_characters.Count; i++)
            {
                Character character = inst_characters[i];
                inst_characters[i].FadeOutAndDestroy(1f);
            }
            inst_characters.Clear();
            yield return new WaitForSeconds(1.1f);

            // Display conversation data for this section
            foreach (ConversationData conversation in section.conversationDatas)
            {
                // 배경 갱신
                if (!string.IsNullOrEmpty(conversation.backgroundID))
                {
                    Debug.Log($"{conversation.backgroundID} 로 배경 갱신");
                    SetBackgroundImage(conversation.backgroundID);
                }
                else
                {

                }

                var characterData = storyManager.GetCharacterData(conversation.characterID);
                // Instantiate new characters that are entering the scene

                if (!string.IsNullOrEmpty(conversation.characterID))
                {
                    Character prevCharacter = inst_characters.FirstOrDefault(character => character.CharacterID == conversation.characterID);
                    if (prevCharacter != null)
                    {
                        inst_characters.Remove(prevCharacter);
                        prevCharacter.FadeOutAndDestroy(.3f);
                    }
                    Character inst_character = InstantiateCharacter(conversation.characterID);
                    if (inst_character != null)
                    {
                        string characterLocation = conversation.characterLocation;
                        inst_character.transform.position = GetCharacterLocation(characterLocation);
                        inst_character.Initialize(characterData.character_id);
                        inst_character.FadeOut(0f);
                        inst_character.SetEmotionData(conversation.emotionID);
                        inst_character.FadeIn(.5f);
                        inst_characters.Add(inst_character);
                        Debug.Log($"{characterData.characterName_ko} Saying : ");
                        SetCharacterText(characterData);
                    }
                }
                else
                {
                    Debug.LogError($"{conversation.characterID} 없음 ");
                }

                yield return StartCoroutine(TypeLine(conversation.line));
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            }

            yield return new WaitForSeconds(1f);
            prevCharacterNames = section.sectionCharacterNames.ToList();
        }
    }
    private Vector2 GetCharacterLocation(string characterLocation)
    {
        if(characterLocation == "Middle")
        {
            return new Vector2(Screen.width * .5f, Screen.height * .5f);
        }
        else if(characterLocation == "Left")
        {
            return new Vector2(Screen.width * .25f, Screen.height * .5f);
        }
        else if(characterLocation == "Right")
        {
            return new Vector2(Screen.width * .75f, Screen.height * .5f);
        }
        else
        {
            return default;
        }
    }
    private Character InstantiateCharacter(string characterID)
    {
        var characterData = storyManager.GetCharacterData(characterID);
        if (characterData != null && characterData.characterPrefab != null)
        {
            var characterPrefab = characterData.characterPrefab.gameObject;
            Character character = Instantiate(characterPrefab, characterPanel.transform).GetComponent<Character>();
            return character;
        }
        else
        {
            return null;
        }
    }

    private void SetCharacterText(CharacterData characterData)
    {
        // Set the character name and apply the color based on characterData
        characterText.text = characterData == null ? "" : characterData.characterName_ko; // Use characterName for display
        characterText.color = characterData == null ? Color.white : characterData.color; // Use predefined color for character
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
