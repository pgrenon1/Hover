using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Map map = (Map)target;

        if (DrawDefaultInspector())
        {
            if (map.autoUpdate)
            {
                map.Generate();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            map.Generate();
        }
        if (GUILayout.Button("Destroy"))
        {
            map.Destroy();
        }
    }
}
