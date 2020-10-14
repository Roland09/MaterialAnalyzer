using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// Pick a gameobject and perform the analyzation on it
/// </summary>
[CustomEditor(typeof(MaterialPicker))]
public class MaterialPickerEditor : Editor
{
    SerializedProperty analyzedObjectProperty;

    MaterialPicker editorTarget;
    MaterialPickerEditor editor;

    void OnEnable()
    {
        editor = this;
        editorTarget = target as MaterialPicker;

        analyzedObjectProperty = serializedObject.FindProperty("analyzedObject");
    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        EditorGUILayout.PropertyField(analyzedObjectProperty, new GUIContent("Analyzed Object"));

        if (GUILayout.Button("Perform Action"))
        {
            PerformAction();
        }

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }

    void PerformAction()
    {
        Debug.Log("Analyzing material of: " + editorTarget.analyzedObject);

        GameObject go = editorTarget.analyzedObject;
        
        Material mat = go.GetComponent<Renderer>().sharedMaterial;

        DeveloperTools.MaterialAnalysis.LogMaterial(mat);
    }
}
