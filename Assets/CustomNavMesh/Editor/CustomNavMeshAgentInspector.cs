﻿using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CanEditMultipleObjects, CustomEditor(typeof(CustomNavMeshAgent))]
public class CustomNavMeshAgentInspector : Editor
{
    static readonly GUIContent s_Text = new GUIContent();

    CustomNavMeshAgent agent;
    NavMeshAgent navMeshAgent;

    SerializedProperty m_AgentTypeID;
    SerializedProperty m_Radius;
    SerializedProperty m_Height;
    SerializedProperty m_WalkableMask;
    SerializedProperty m_Speed;
    SerializedProperty m_Acceleration;
    SerializedProperty m_AngularSpeed;
    SerializedProperty m_StoppingDistance;
    SerializedProperty m_AutoTraverseOffMeshLink;
    SerializedProperty m_AutoBraking;
    SerializedProperty m_AutoRepath;
    SerializedProperty m_BaseOffset;
    SerializedProperty m_ObstacleAvoidanceType;
    SerializedProperty m_AvoidancePriority;

    private class Styles
    {
        public readonly GUIContent m_AgentSteeringHeader = EditorGUIUtility.TrTextContent("Steering");
        public readonly GUIContent m_AgentAvoidanceHeader = EditorGUIUtility.TrTextContent("Obstacle Avoidance");
        public readonly GUIContent m_AgentPathFindingHeader = EditorGUIUtility.TrTextContent("Path Finding");
    }

    static Styles s_Styles;

    private void OnEnable()
    {
        agent = target as CustomNavMeshAgent;
        navMeshAgent = agent.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            // prevent the user from changing the agent through the NavMeshAgent's inspector
            // navMeshAgent.hideFlags = HideFlags.HideInInspector;
        }

        m_AgentTypeID = serializedObject.FindProperty("m_AgentTypeID");
        m_Radius = serializedObject.FindProperty("m_Radius");
        m_Height = serializedObject.FindProperty("m_Height");
        m_WalkableMask = serializedObject.FindProperty("m_WalkableMask");
        m_Speed = serializedObject.FindProperty("m_Speed");
        m_Acceleration = serializedObject.FindProperty("m_Acceleration");
        m_AngularSpeed = serializedObject.FindProperty("m_AngularSpeed");
        m_StoppingDistance = serializedObject.FindProperty("m_StoppingDistance");
        m_AutoTraverseOffMeshLink = serializedObject.FindProperty("m_AutoTraverseOffMeshLink");
        m_AutoBraking = serializedObject.FindProperty("m_AutoBraking");
        m_AutoRepath = serializedObject.FindProperty("m_AutoRepath");
        m_BaseOffset = serializedObject.FindProperty("m_BaseOffset");
        m_ObstacleAvoidanceType = serializedObject.FindProperty("m_ObstacleAvoidanceType");
        m_AvoidancePriority = serializedObject.FindProperty("m_AvoidancePriority");
    }

    public override void OnInspectorGUI()
    {
        if (s_Styles == null)
            s_Styles = new Styles();

        serializedObject.Update();

        AgentTypePopupInternal("Agent Type", m_AgentTypeID);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_BaseOffset);
        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Agent Base Offset");
            Undo.RecordObject(navMeshAgent, "");

            agent.BaseOffset = m_BaseOffset.floatValue;

            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField(s_Styles.m_AgentSteeringHeader, EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_Speed);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Agent Speed");
            Undo.RecordObject(navMeshAgent, "");

            if (m_Speed.floatValue < 0.0f) m_Speed.floatValue = 0.0f;
            agent.Speed = m_Speed.floatValue;

            serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_AngularSpeed);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Agent Angular Speed");
            Undo.RecordObject(navMeshAgent, "");

            if (m_AngularSpeed.floatValue < 0.0f) m_AngularSpeed.floatValue = 0.0f;
            agent.AngularSpeed = m_AngularSpeed.floatValue;

            serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_Acceleration);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Agent Acceleration");
            Undo.RecordObject(navMeshAgent, "");

            if (m_Acceleration.floatValue < 0.0f) m_Acceleration.floatValue = 0.0f;
            agent.Acceleration = m_Acceleration.floatValue;

            serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_StoppingDistance);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Agent Stopping Distance");
            Undo.RecordObject(navMeshAgent, "");

            if (m_StoppingDistance.floatValue < 0.0f) m_StoppingDistance.floatValue = 0.0f;
            agent.StoppingDistance = m_StoppingDistance.floatValue;

            serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_AutoBraking);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Agent Auto Braking");
            Undo.RecordObject(navMeshAgent, "");

            agent.AutoBraking = m_AutoBraking.boolValue;

            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField(s_Styles.m_AgentAvoidanceHeader, EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_Radius);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Agent Radius");
            Undo.RecordObject(navMeshAgent, "");

            if (m_Radius.floatValue <= 0.0f) m_Radius.floatValue = 1e-05f;
            agent.Radius = m_Radius.floatValue;

            serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_Height);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Agent Height");
            Undo.RecordObject(navMeshAgent, "");

            if (m_Height.floatValue <= 0.0f) m_Height.floatValue = 1e-05f;
            agent.Height = m_Height.floatValue;

            serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_ObstacleAvoidanceType, Temp("Quality"));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Agent Obstacle Avoidance Type");
            Undo.RecordObject(navMeshAgent, "");

            agent.ObstacleAvoidanceType = (ObstacleAvoidanceType) m_ObstacleAvoidanceType.enumValueIndex;

            serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_AvoidancePriority, Temp("Priority"));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Agent Avoidance Priority");
            Undo.RecordObject(navMeshAgent, "");

            if (m_AvoidancePriority.intValue < 0) m_AvoidancePriority.intValue = 0;
            if (m_AvoidancePriority.intValue > 99) m_AvoidancePriority.intValue = 99;
            agent.AvoidancePriority = m_AvoidancePriority.intValue;

            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField(s_Styles.m_AgentPathFindingHeader, EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_AutoTraverseOffMeshLink);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Agent Auto Traverse Off Mesh Link");
            Undo.RecordObject(navMeshAgent, "");

            agent.AutoTraverseOffMeshLink = m_AutoTraverseOffMeshLink.boolValue;

            serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_AutoRepath);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Agent Auto Repath");
            Undo.RecordObject(navMeshAgent, "");

            agent.AutoRepath = m_AutoRepath.boolValue;

            serializedObject.ApplyModifiedProperties();
        }

        //Initially needed data
        var areaNames = GameObjectUtility.GetNavMeshAreaNames();
        var currentMask = m_WalkableMask.longValue;
        var compressedMask = 0;

        //Need to find the index as the list of names will compress out empty areas
        for (var i = 0; i < areaNames.Length; i++)
        {
            var areaIndex = GameObjectUtility.GetNavMeshAreaFromName(areaNames[i]);
            if (((1 << areaIndex) & currentMask) != 0)
                compressedMask = compressedMask | (1 << i);
        }

        //TODO: Refactor this to use the mask field that takes a label.
        float kSingleLineHeight = 18f;
        float kSpacing = 5;
        float kLabelFloatMinW = EditorGUIUtility.labelWidth + EditorGUIUtility.fieldWidth + kSpacing;
        float kLabelFloatMaxW = EditorGUIUtility.labelWidth + EditorGUIUtility.fieldWidth + kSpacing;

        float kH = kSingleLineHeight;
        var position = GUILayoutUtility.GetRect(kLabelFloatMinW, kLabelFloatMaxW, kH, kH, EditorStyles.layerMaskField);

        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = m_WalkableMask.hasMultipleDifferentValues;
        var areaMask = EditorGUI.MaskField(position, "Area Mask", compressedMask, areaNames, EditorStyles.layerMaskField);
        EditorGUI.showMixedValue = false;

        if (EditorGUI.EndChangeCheck())
        {
            if (areaMask == ~0)
            {
                m_WalkableMask.longValue = 0xffffffff;
            }
            else
            {
                uint newMask = 0;
                for (var i = 0; i < areaNames.Length; i++)
                {
                    //If the bit has been set in the compacted mask
                    if (((areaMask >> i) & 1) != 0)
                    {
                        //Find out the 'real' layer from the name, then set it in the new mask
                        newMask = newMask | (uint)(1 << GameObjectUtility.GetNavMeshAreaFromName(areaNames[i]));
                    }
                }
                m_WalkableMask.longValue = newMask;
            }

            Undo.RecordObject(target, "Changed Agent Area Mask");
            Undo.RecordObject(navMeshAgent, "");

            agent.AreaMask = m_WalkableMask.intValue;

            serializedObject.ApplyModifiedProperties();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AgentTypePopupInternal(string labelName, SerializedProperty agentTypeID)
    {
        var index = -1;
        var count = UnityEngine.AI.NavMesh.GetSettingsCount();
        var agentTypeNames = new string[count + 2];
        for (var i = 0; i < count; i++)
        {
            var id = UnityEngine.AI.NavMesh.GetSettingsByIndex(i).agentTypeID;
            var name = UnityEngine.AI.NavMesh.GetSettingsNameFromID(id);
            agentTypeNames[i] = name;
            if (id == agentTypeID.intValue)
                index = i;
        }
        agentTypeNames[count] = "";
        agentTypeNames[count + 1] = "Open Agent Settings...";

        bool validAgentType = index != -1;
        if (!validAgentType)
        {
            EditorGUILayout.HelpBox("Agent Type invalid.", MessageType.Warning);
        }

        var rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
        EditorGUI.BeginProperty(rect, GUIContent.none, agentTypeID);

        EditorGUI.BeginChangeCheck();
        index = EditorGUI.Popup(rect, labelName, index, agentTypeNames);
        if (EditorGUI.EndChangeCheck())
        {
            if (index >= 0 && index < count)
            {
                var id = UnityEngine.AI.NavMesh.GetSettingsByIndex(index).agentTypeID;
                agentTypeID.intValue = id;
            }
            else if (index == count + 1)
            {
                UnityEditor.AI.NavMeshEditorHelpers.OpenAgentSettings(-1);
            }

            Undo.RecordObject(target, "Changed Agent Type");
            Undo.RecordObject(navMeshAgent, "");

            agent.AgentTypeID = agentTypeID.intValue;

            serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.EndProperty();
    }

    static GUIContent Temp(string t)
    {
        s_Text.text = t;
        s_Text.tooltip = string.Empty;
        return s_Text;
    }
}
