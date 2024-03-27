using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LinePanel : MonoBehaviour
{
    [SerializeField] private UIPositionSetter _uiPositionSetter;
    [SerializeField] private Image _background; // 배경 이미지
    [SerializeField] private TextMeshProUGUI _characterText; // 캐릭터 이름 등을 보여줄 텍스트
    [SerializeField] private TextMeshProUGUI _lineText; // 대화 내용을 보여줄 텍스트

    private bool _isActive = false; // 활성화 여부를 나타내는 변수

    // 대화 내용과 캐릭터 이름을 설정하는 메서드
    public void SetDialogue(string characterName, string dialogue)
    {
        // 캐릭터 이름과 대화 내용을 각각 텍스트 UI에 설정
        _characterText.text = characterName;
        _lineText.text = dialogue;
    }

    // 활성화 상태를 설정하는 메서드
    public void SetOn(bool isOn)
    {
        // 현재 상태와 변경하려는 상태가 같으면 더 이상 처리하지 않음
        if (_isActive == isOn)
            return;

        _isActive = isOn;

        // ArokaTransform 컴포넌트를 사용하여 활성화 여부를 설정
        _uiPositionSetter.SetOn(isOn, 3f);
    }
}
