using UnityEngine;
using UnityEditor;
using System;
using System.Text.RegularExpressions;

public class OrderingName : Editor
{
    [MenuItem("GameObject/オブジェクト名を連番にする", false, 0)]
    static void Ordering()
    {
        Undo.RecordObjects(Selection.objects, "modify name");

        int count = Selection.objects.Length;
        int d = count.ToString().Length;

        string name = Selection.objects[0].name;

        for (int i = 0; i < count; i++)
        {
            name = Regex.Replace(name, @"(\d)+$", "");
            Selection.objects[i].name = name + string.Format("{0:00}", i);
        }
    }
}