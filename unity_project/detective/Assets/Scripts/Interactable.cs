using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private Image _placeImg;
    public Image PlaceImg { get; }
    private UnityEvent _onClick = new UnityEvent(); // 클릭 시 발생할 이벤트 (비공개)

    // 상호작용을 처리하는 추상 메서드
    public abstract void Interact();

    // 클릭 이벤트를 추가하는 메서드
    public void AddClickListener(UnityAction action)
    {
        _onClick.AddListener(action);
    }

    // 상호작용을 시작하는 메서드
    public void OnClick()
    {
        _onClick.Invoke();
        Interact();
    }

}
