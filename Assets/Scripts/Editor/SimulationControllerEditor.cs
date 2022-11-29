using UnityEditor;
using UnityEngine;
using Simulation;
using Simulation.Fluid;
using System.Linq;

// namespace Simulation.Fluid
// {
    [CustomEditor(typeof(SimulationController))]
    public class SimulationControllerEditor : Editor
    {
        static protected SimulationStep IntToEnum(int index)
        {
            return System.Enum.GetValues(typeof(SimulationStep)).Cast<SimulationStep>().Last(e => (int)e <= index);
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var simulation = target as SimulationController;
            if (simulation == null) return;

            foreach(var p in simulation.SortedPlugins)
            {
                EditorGUILayout.LabelField(IntToEnum(p.Key).ToString(), EditorStyles.boldLabel);
                foreach(var plugin in p.Value)
                {
                    EditorGUILayout.LabelField(plugin.Identifier);
                }
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            }

        }
    }
// }