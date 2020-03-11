using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.AnimatedValues;
using System.IO;
using System.Net.Mail;

public class GPUParticleSystemBugReporter : EditorWindow
{
	private string email = "E-mail";
	private string description = "Description";
	private string reproduction = "How to reproduce";
	private MailMessage message;
	
	public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(GPUParticleSystemBugReporter), true, "Report a bug");
        window.position = new Rect(Screen.width/2f,Screen.height/2f,600f,400f);
        window.maxSize = new Vector2(600f, 400f);
        window.minSize = new Vector2(600f, 400f);
    }

    void OnEnable()
    {
	}

    void OnGUI()
    {
		EditorGUILayout.BeginVertical();
		{
			EditorGUILayout.LabelField("Bug Reporter");
			EditorGUILayout.HelpBox("Before reporting a bug, please check if the bug has already been reported before. you can find all reported bugs in the roadmap.", MessageType.Info);

			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Roadmap"))
				{
					Application.OpenURL("https://docs.google.com/spreadsheets/d/16KmluoemHBrA_BGpw8YBms8Ib7YX7rQSAvvCeVgSmPc/edit?usp=sharing");
				}
				GUILayout.FlexibleSpace();
				/*
				if (GUILayout.Button("Help"))
				{
					GPUParticleSystemHelperWindow.ShowWindow();
				}
				GUILayout.FlexibleSpace();
				*/
			}
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			EditorGUILayout.LabelField("How can I contact you?");
			email = EditorGUILayout.TextArea(email);

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			EditorGUILayout.LabelField("Please give a detailed description of the bug.");
			description = EditorGUILayout.TextArea(description, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 5f));

			GUILayout.Space(EditorGUIUtility.singleLineHeight);

			EditorGUILayout.LabelField("How can I reproduce the bug? The more details the better!");
			reproduction = EditorGUILayout.TextArea(reproduction, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 5f));

			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Send Report"))
				{
					
				}
				GUILayout.FlexibleSpace();
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndVertical();
	}

}
