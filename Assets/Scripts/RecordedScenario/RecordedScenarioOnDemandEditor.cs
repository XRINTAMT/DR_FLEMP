#if UNITY_EDITOR 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RecordedScenario
{
    [CustomEditor(typeof(PrerecordedAudioOnDemand))]
    public class RecordedScenarioOnDemandEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            PrerecordedAudioOnDemand _scenario = (PrerecordedAudioOnDemand)target;
            if (GUILayout.Button("Load Scenario"))
            {
                _scenario.InitiateSetup();
            }
            if (GUILayout.Button("Stop Loading"))
            {
                _scenario.BreakSetup();
            }
            DrawDefaultInspector();
        }
    }

    
}
#endif
