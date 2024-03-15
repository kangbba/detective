using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Kit.Editor;

namespace Calcatz.VNDetectiveGUI {
    [CustomEditor(typeof(Dialogue))]
    public class DialogueEditor : Editor {

        private ReorderableListExtend choiceList;
        private ReorderableListExtend messageList;

        private void OnEnable() {
            choiceList = new ReorderableListExtend(serializedObject, "choices", true, true, true, true);
            messageList = new ReorderableListExtend(serializedObject, "messages", true, true, true, true);
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Reorderable List", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            choiceList.DoLayoutList();
            messageList.DoLayoutList();
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
                Undo.RecordObject(serializedObject.targetObject, "Change list");
            }
        }

    }
}
