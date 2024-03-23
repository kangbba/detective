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
    public StoryBackground backgroundImagePrefab; 
    private StoryBackground curStoryBackground;

    private List<Character> inst_characters = new List<Character>();
    private List<string> sectionCharacterNames = new List<string>(); 
 
    public void Initialize(StoryManager storyManager)
    {
        this.storyManager = storyManager; 
        StartCoroutine(DisplaySectionsCoroutine()); 
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
                var characterID = conversation.characterID;
                var characterLocation = conversation.characterLocation;
                var emotionID = conversation.emotionID;
                var backgroundID = conversation.backgroundID;
                var lines = conversation.lines;
                CharacterData characterData = storyManager.GetCharacterData(characterID);
                // 배경 갱신
                if (!string.IsNullOrEmpty(backgroundID))
                {
                    Debug.Log($"{backgroundID} 로 배경 갱신");
                    SetBackgroundImage(backgroundID);
                }
                else
                {

                }
                if (!string.IsNullOrEmpty(characterID))
                {
                    Character prevCharacter = inst_characters.FirstOrDefault(character => character.CharacterID == characterID);
                    if (prevCharacter != null)
                    {
                        inst_characters.Remove(prevCharacter);
                        prevCharacter.FadeOutAndDestroy(.3f);
                    }
                    Character inst_character = InstantiateCharacter(characterID);
                    if (inst_character != null)
                    {
                        inst_character.transform.position = GetCharacterLocation(characterLocation);
                        inst_character.Initialize(characterID);
                        inst_character.FadeOut(0f);
                        inst_character.SetEmotionData(emotionID);
                        inst_character.FadeIn(.5f);
                        inst_characters.Add(inst_character);
                    }
                }
                else
                {
                    Debug.LogError($"{conversation.characterID} 없음 ");
                }
                // Instantiate new characters that are entering the scene
                SetCharacterText(characterData);
                string[] linesArray = lines.Split(';'); // 세미콜론을 기준으로 문자열을 쪼갭니다.
                yield return StartCoroutine(TypeLines(linesArray));
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

    private IEnumerator TypeLines(string[] lines)
    {
        lineText.text = ""; // 초기 텍스트를 비웁니다.
        foreach (string line in lines)
        {
            yield return StartCoroutine(TypeLine(line)); // 현재 줄을 타이핑합니다.

            // 사용자가 클릭할 때까지 대기합니다.
            // 마우스 버튼을 클릭하거나 특정 키를 누르는 등 원하는 입력 조건을 추가할 수 있습니다.
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            // 다음 줄을 출력하기 전에 텍스트를 비울 수도 있습니다.
            // lineText.text = "";
        }
    }

    private IEnumerator TypeLine(string line)
    {
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
