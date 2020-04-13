using UnityEngine;
using UnityEditor;
using System;
using System.Text.RegularExpressions;

public class ToggleMeshRendering : Editor
{
    [MenuItem("GameObject/MedhRendererのOnOff切り替え", false, 0)]
    static void Toggle()
    {
        Undo.RecordObjects(Selection.objects, "toggle render");

        int count = Selection.objects.Length;

        bool decideFirstObjToggle = false;
        bool firstObjectToggle = true;

        for (int i = 0; i < count; i++)
        {
            GameObject obj = (GameObject)Selection.objects[i];
            foreach (var item in obj.GetComponents<Renderer>())
            {
                if(!decideFirstObjToggle)
                {
                    decideFirstObjToggle = true;
                    firstObjectToggle = !item.enabled;
                }
                item.enabled = firstObjectToggle;
            }
            foreach (var item in obj.GetComponentsInChildren<Renderer>())
            {
                if (!decideFirstObjToggle)
                {
                    decideFirstObjToggle = true;
                    firstObjectToggle = !item.enabled;
                }
                item.enabled = firstObjectToggle;
            }
        }
    }
}