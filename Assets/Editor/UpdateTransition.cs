using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NextTransition))]
public class UpdateTransition : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Next Transition"))
        {
            TransitionManager.Instance._moveTransition = true;
        }

    }
}
