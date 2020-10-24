﻿using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(CustomNavMeshSurface))]
public class CustomNavMeshSurfaceInspector : Editor
{
    CustomNavMeshSurface surface;

    SerializedProperty mesh;

    private void OnEnable()
    {
        surface = target as CustomNavMeshSurface;
        surface.GetComponent<MeshFilter>().hideFlags = HideFlags.HideInInspector;

        mesh = serializedObject.FindProperty("mesh");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(mesh);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Surface Mesh");

            var meshFilter = surface.GetComponent<MeshFilter>();
            if(meshFilter != null) Undo.RecordObject(meshFilter, "");

            surface.Mesh = (Mesh) mesh.objectReferenceValue;

            serializedObject.ApplyModifiedProperties(); // needed to create a prefab override
        }
    }
}
