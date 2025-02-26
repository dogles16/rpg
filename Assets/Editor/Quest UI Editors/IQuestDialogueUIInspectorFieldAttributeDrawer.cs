﻿// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;

namespace PixelCrushers.QuestMachine
{

    /// <summary>
    /// Custom drawer that allows assignment of the C# interface IQuestDialogueUI.
    /// </summary>
    [CustomPropertyDrawer(typeof(IQuestDialogueUIInspectorFieldAttribute), true)]
    public class IQuestDialogueUIInspectorFieldAttributeDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var newValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(UnityEngine.Object), true);
            TryAssignNewValue(property, newValue);
        }

        public static void DoLayout(SerializedProperty property, GUIContent label)
        {
            if (property == null) return;
            var newValue = EditorGUILayout.ObjectField(label, property.objectReferenceValue, typeof(UnityEngine.Object), true);
            TryAssignNewValue(property, newValue);
        }

        protected static void TryAssignNewValue(SerializedProperty property, UnityEngine.Object newValue)
        {
            if (newValue != property.objectReferenceValue)
            {
                if (newValue == null)
                {
                    property.objectReferenceValue = null;
                }
                else
                {
                    IQuestDialogueUI newUI = null;
                    if (newValue is GameObject)
                    {
                        newUI = (newValue as GameObject).GetComponent(typeof(IQuestDialogueUI)) as IQuestDialogueUI;
                    }
                    else if (newValue is Component)
                    {
                        var go = (newValue as Component).gameObject;
                        newUI = go.GetComponent(typeof(IQuestDialogueUI)) as IQuestDialogueUI;
                    }
                    if (newUI != null)
                    {
                        property.objectReferenceValue = newUI as Component;
                    }
                }
            }
        }

    }
}
