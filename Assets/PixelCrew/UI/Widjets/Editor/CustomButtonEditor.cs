using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
namespace Assets.PixelCrew.UI.Editor
{

    [CustomEditor(typeof(CustomButton), true)]
    [CanEditMultipleObjects]
    public class CustomButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_normal"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_pressed"));
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}

