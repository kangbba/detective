using UnityEditor;
using UnityEngine;

[System.Serializable]
public class UIState
{
    [SerializeField] private Vector2 _position;
    [SerializeField] private Vector2 _scale;
    [SerializeField] private Vector3 _rotation;

    public Vector2 Position => _position;
    public Vector2 Scale => _scale;
    public Vector3 Rotation => _rotation;

    public UIState(Vector2 position, Vector2 scale, Vector3 rotation)
    {
        _position = position;
        _scale = scale;
        _rotation = rotation;
    }
    public UIState(RectTransform rectTransform)
    {
        _position = rectTransform.anchoredPosition;
        _scale = rectTransform.localScale;
        _rotation = rectTransform.localEulerAngles;
    }
}


public class UIPositionSetter : MonoBehaviour
{
    [SerializeField] private UIState _onState;
    [SerializeField] private UIState _offState;

    private void Reset()
    {
        Debug.Log("기본 세팅으로 자동 등록되었습니다 ");
        UIState uiStateOn = new UIState(GetComponent<RectTransform>());
        RegisterState(true, uiStateOn);
        RegisterStateWithCurrent(true);
        UIState uiStateOff = new UIState(uiStateOn.Position, Vector2.zero, uiStateOn.Rotation);
        RegisterState(false, uiStateOff);
    }
    public void RegisterState(bool isOn, UIState uiState)
    {
        if (isOn)
        {
            _onState = uiState;
        }
        else
        {
            _offState = uiState;
        }
    }

    public void RegisterStateWithCurrent(bool isOn)
    {
        if (isOn)
        {
            _onState = new UIState(transform.GetComponent<RectTransform>());
        }
        else
        {
            _offState = new UIState(transform.GetComponent<RectTransform>());
        }
    }


    public void SetOn(bool isOn, float totalTime)
    {
        Debug.Log(isOn);
        if (isOn)
        {
            transform.ArokaTr().SetAnchoredPos(_onState.Position, totalTime);
            transform.ArokaTr().SetLocalScale(_onState.Scale, totalTime);
            transform.ArokaTr().SetRot(Quaternion.Euler(_onState.Rotation), totalTime);
        }
        else
        {
            transform.ArokaTr().SetAnchoredPos(_offState.Position, totalTime);
            transform.ArokaTr().SetLocalScale(_offState.Scale, totalTime);
            transform.ArokaTr().SetRot(Quaternion.Euler(_offState.Rotation), totalTime);
        }
    }
}
