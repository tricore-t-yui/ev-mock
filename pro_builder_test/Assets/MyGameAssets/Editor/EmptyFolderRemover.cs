using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EmptyFolderRemover : Editor
{
    [MenuItem("Tools/空のフォルダを削除")]
    public static void Run()
    {
        var emptyDirectories = new List<string>();
        while (true)
        {
            emptyDirectories.Clear();
            ListUpEmptyDirectories("Assets", emptyDirectories);
            if (emptyDirectories.Count == 0) break;

            foreach (var path in emptyDirectories)
            {
                Directory.Delete(path);
                Debug.Log("Deleted: " + path);
            }
            AssetDatabase.Refresh();
        }
    }

    public static void ListUpEmptyDirectories(string path, List<string> emptyDirectories)
    {
        var directories = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path);
        if (directories.Length + files.Length == 0)
        {
            emptyDirectories.Add(path);
            return;
        }
        foreach (var directory in directories)
        {
            ListUpEmptyDirectories(directory, emptyDirectories);
        }
    }
}