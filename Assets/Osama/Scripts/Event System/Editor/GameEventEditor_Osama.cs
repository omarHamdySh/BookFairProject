using UnityEngine;
using UnityEditor;

/// <summary>
/// This scrips creates editor button -Raise Event- to raise any event from editor
/// </summary>

[CustomEditor(typeof(GameEvent_Osama))]
public class GameEventEditor_Osama : Editor
{
    public override void OnInspectorGUI()//  Draw Button inside unity editor
    {
        base.OnInspectorGUI();
        GUI.enabled = Application.isPlaying;
        GameEvent_Osama e = target as GameEvent_Osama;
        if (GUILayout.Button("Raise Event"))
        {
            e.Raise();
        }
    }
}