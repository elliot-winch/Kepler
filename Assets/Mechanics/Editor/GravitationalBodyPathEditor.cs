using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GravitationBodyPath))]
public class GravitationalBodyPathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GravitationBodyPath path = (GravitationBodyPath)target;
        if (GUILayout.Button("Refresh"))
        {
            path.Refresh();
        }
    }
}
