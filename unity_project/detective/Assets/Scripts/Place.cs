using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ScreenEffective
{
    Default,
    FadeIn,
    FadeOut,
    Highlight
}
public class Place : Interactable
{
    [SerializeField] private GameObject[] _uiElements; // 이 장소에 속한 UI 요소들 (비공개, 하이어라키에서 바인딩 가능)
    [SerializeField] private List<Place> _childPlaces; // 자식 장소 리스트 (비공개, 하이어라키에서 바인딩 가능)
    [SerializeField] private Place _parentPlace; // 부모 장소 (비공개, 하이어라키에서 바인딩 가능)

    // 상호작용을 처리하는 추상 메서드를 오버라이드
    public override void Interact()
    {
        // 여기에 장소의 상호작용 로직 구현
        // 예: 다른 장소로 이동, 특정 UI 요소 표시 등
    }

    // 장소의 활성화/비활성화를 제어하는 새로운 메서드
    public void SetOn(bool isOn, ScreenEffective screenEffective = ScreenEffective.Default)
    {
        base.PlaceImg.gameObject.SetActive(isOn);

        // ScreenEffective에 따른 화면 효과 적용
        ApplyScreenEffect(screenEffective);
    }

    private void ApplyScreenEffect(ScreenEffective screenEffective)
    {
        switch (screenEffective)
        {
            case ScreenEffective.FadeIn:
                // FadeIn 효과 구현
                break;
            case ScreenEffective.FadeOut:
                // FadeOut 효과 구현
                break;
            case ScreenEffective.Highlight:
                // Highlight 효과 구현
                break;
            default:
                // 기본 효과 구현
                break;
        }
    }
}
