using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DrawCloudsExample))]
public class DrawCloudsExampleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawCloudsExample clouds = (DrawCloudsExample)target;

        if (DrawDefaultInspector())
        {
            if (clouds.autoUpdate)
            {
                clouds.UpdateClouds();
            }
        }
    }
}
