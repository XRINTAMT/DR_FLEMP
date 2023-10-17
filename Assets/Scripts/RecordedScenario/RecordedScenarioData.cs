using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RecordedScenario
{
    [CustomEditor(typeof(RecordedScenarioText))]
    public class RecordedScenarioData : Editor
    {
        override public void OnInspectorGUI()
        {
            RecordedScenarioText _scenario = (RecordedScenarioText)target;
            if (GUILayout.Button("Load Scenario"))
            {
                _scenario.Setup();
            }
            DrawDefaultInspector();
        }
    }

    
}

