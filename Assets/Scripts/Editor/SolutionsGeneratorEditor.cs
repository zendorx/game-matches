using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SolutionsGenerator))]
public class SolutionsGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SolutionsGenerator myScript = (SolutionsGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            myScript.Generate();
        }
    }
}
