using UnityEngine;
using UnityEditor;
using System;
using DeveloperTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

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

        string helpText = "Data are visualized in the console via logging.";
        helpText += "\n";
        helpText += "Drag a GameObject with a material into the 'Analyzed Object' slot";
        helpText += "\n";
        helpText += "\n";
        helpText += "Analysis:";
        helpText += "\n";
        helpText += "Click 'Log Material' to show the material data in the Console.";
        helpText += "\n";
        helpText += "\n";
        helpText += "Comparison:";
        helpText += "\n";
        helpText += "Drag in various materials and hit Store A and Store B to store the material data";
        helpText += "\n";
        helpText += "Click respective Diff buttons to get the material differences in the console";

        EditorGUILayout.HelpBox( helpText, MessageType.Info);

        EditorGUILayout.PropertyField(analyzedObjectProperty, new GUIContent("Analyzed Object"));

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Log Material"))
            {
                GetMaterialData();
            }

            if (GUILayout.Button("Store A"))
            {
                editorTarget.materialDataA = GetMaterialData();
            }

            if (GUILayout.Button("Store B"))
            {
                editorTarget.materialDataB = GetMaterialData();
            }

            EditorGUI.BeginDisabledGroup(editorTarget.materialDataA == null || editorTarget.materialDataB == null);
            {
                if (GUILayout.Button("Diff A -> B"))
                {
                    DiffMaterials("A -> B", editorTarget.materialDataA, editorTarget.materialDataB);
                }

                if (GUILayout.Button("Diff B -> A"))
                {
                    DiffMaterials("B -> A", editorTarget.materialDataB, editorTarget.materialDataA);
                }
            }
            EditorGUI.EndDisabledGroup();

        }
        EditorGUILayout.EndHorizontal();


        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }

    MaterialData GetMaterialData()
    {
        GameObject go = editorTarget.analyzedObject;
        
        Material mat = go.GetComponent<Renderer>().sharedMaterial;

        DeveloperTools.MaterialAnalysis.LogMaterial(mat);

        MaterialData data = new MaterialData();

        data.common = DeveloperTools.MaterialAnalysis.GetCommonSettings(mat);
        data.keywords = DeveloperTools.MaterialAnalysis.GetKeywords(mat);
        data.properties = DeveloperTools.MaterialAnalysis.GetProperties(mat);
        data.shaderPasses = DeveloperTools.MaterialAnalysis.GetShaderPasses(mat);

        return data;
    }

    void DiffMaterials( string label, MaterialData dataA, MaterialData dataB)
    {
        if( dataA == null)
        {
            Debug.Log("Material A is empty");
            return;
        }

        if (dataB == null)
        {
            Debug.Log("Material B is empty");
            return;
        }

        String text = "";
        text += ListToString("Common", Diff(dataA.common, dataB.common));
        text += ListToString("Keywords", Diff(dataA.keywords, dataB.keywords));
        text += ListToString("Properties", Diff(dataA.properties, dataB.properties));
        text += ListToString("ShaderPasses", Diff(dataA.shaderPasses, dataB.shaderPasses));

        Debug.Log("Diff " + label + ":\n" + text);

    }

    List<string> Diff( List<string> oldData, List<string> newData)
    {
        List<string> list = new List<string>();

        // add all of A
        list.AddRange(newData);

        // remove all of B
        list = list.Except(oldData).ToList();

        return list;
    }

    private static string ListToString(string label, List<string> list)
    {
        string text = label;
        text += "\n";

        for (int i = 0; i < list.Count; i++)
        {
            text += string.Format("{0}: {1}", i, list[i]);
            text += "\n";
        }

        return text;
    }
}
