using System.Drawing;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIPositionSetter))]
public class UIPositionSetterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        // 버튼의 너비를 현재 에디터의 반으로 할당
        float buttonWidth = EditorGUIUtility.currentViewWidth / 2 - 10; // 10은 여유 공간
        UIPositionSetter uIPositionSetter = (UIPositionSetter)target;
        RectTransform rect = uIPositionSetter.GetComponent<RectTransform>();
        if (GUILayout.Button("Register On State", GUILayout.Width(buttonWidth)))
        {

            uIPositionSetter.RegisterStateWithCurrent(true);
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Register Off State", GUILayout.Width(buttonWidth)))
        {
            uIPositionSetter.RegisterStateWithCurrent(false);
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Preview On State", GUILayout.Width(buttonWidth)))
        {
            PreviewState(true);
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Preview Off State", GUILayout.Width(buttonWidth)))
        {
            PreviewState(false);
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

    private void PreviewState(bool isOn)
    {
        ((UIPositionSetter)target).SetOn(isOn, 0f);

        // 변경 사항을 마크
        EditorUtility.SetDirty(target);
        // 씬 뷰를 다시 그림
        SceneView.RepaintAll();
    }
}
