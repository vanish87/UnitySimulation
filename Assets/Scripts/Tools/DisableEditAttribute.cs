using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.Attributes
{
    public class DisableEdit : PropertyAttribute
    {
    }
}
namespace Simulation.Editor
{
    #if UNITY_EDITOR
    using UnityEditor;
    using Simulation.Attributes;

    [CustomPropertyDrawer(typeof(DisableEdit))]
    public class DisableEditAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
    #endif
}