using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class WelcomeWindow : EditorWindow
{
	private static bool dontShowAgain = false;

	private static bool tab0 = false;
	private static bool tab1 = false;
	private static bool tab2 = false;
	private static bool tab3 = false;
	private static bool tab4 = false;
	private static bool tab5 = false;
	private static bool tab6 = false;
	private static bool tab7 = false;

	private static Texture2D image;

	private Vector2 scroll = new Vector2();

	static WelcomeWindow()
	{
		if (!System.IO.File.Exists(Application.persistentDataPath + "/welcomeMessage.txt"))
		{
			image = new Texture2D(1, 1);
			image.LoadImage(System.IO.File.ReadAllBytes(Application.dataPath + "/Ultimate GPU Particle System/Resources/Logo.png"));
			image.Apply();

			WelcomeWindow window = (WelcomeWindow)EditorWindow.GetWindow(typeof(WelcomeWindow), true, "Ultimate GPU Particle System");
			window.maxSize = new Vector2(600f, 800f);
			window.minSize = new Vector2(600f, 400f);
		}
	}
	
	[MenuItem("Window/GPUP Welcome screen")]
	static void Init()
	{
		if (System.IO.File.Exists(Application.persistentDataPath + "/welcomeMessage.txt"))
		{
			System.IO.File.Delete(Application.persistentDataPath + "/welcomeMessage.txt");
		}

		image = new Texture2D(1, 1);
		image.LoadImage(System.IO.File.ReadAllBytes(Application.dataPath + "/Ultimate GPU Particle System/Resources/Logo.png"));
		image.Apply();

		WelcomeWindow window = (WelcomeWindow)EditorWindow.GetWindow(typeof(WelcomeWindow), true, "Ultimate GPU Particle System");
		window.maxSize = new Vector2(600f, 800f);
		window.minSize = new Vector2(600f, 400f);
	}

	void OnGUI()
	{
		Rect r = GUILayoutUtility.GetRect(600f, 110f);
		GUI.DrawTexture(r, image);

		GUIStyle gs = new GUIStyle(GUI.skin.label);
		gs.alignment = TextAnchor.MiddleLeft;
		gs.fontStyle = FontStyle.Bold;

		EditorGUILayout.BeginVertical("Box");
		{
			EditorStyles.label.wordWrap = true;
			EditorGUILayout.LabelField("Ultimate GPU Particle System v1.1", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("Thank you very much for purchasing Ultimate GPU Particle System. If you experience any bugs or have questions regarding the particle system, please contact me via m4xproud@gmail.com. Below you can find some frequently asked questions, tips and tricks and notifications.");
			//GUILayout.FlexibleSpace();

			scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Width(580f), GUILayout.Height(Screen.height-230f));
			{
				EditorGUILayout.BeginVertical("Box");
				{
					if (GUILayout.Button("What's new", gs))
					{
						tab0 = !tab0;
					}

					if (tab0)
					{
						EditorGUILayout.LabelField("Version 1.1 comes with a variety of new features, fixes and improvements. The biggest new feature is the possibility to use GPU trails now. See the documentation for furterh info.");

					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginVertical("Box");
				{
					if (GUILayout.Button("Getting started", gs))
					{
						tab1 = !tab1;
					}

					if (tab1)
					{
						EditorGUILayout.LabelField("To create a new GPU particle system, go to the toolbar and click GameObject/Effects/GPU Particle System. This will add a new GPU Particle System to the scene and can be edited using the inspector very much like Shuriken.");

					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginVertical("Box");
				{
					if (GUILayout.Button("Trails", gs))
					{
						tab7 = !tab7;
					}

					if (tab7)
					{
						EditorGUILayout.LabelField("Trails are stored in the Particle Buffer. That means, that the Particle width and heigth information changes to Num trails and Num segments. Each segment follows its preceding segment in the Position Buffer. By changing segment count or the follow speed setting in the rendering tab, you can make trails shorter or longer. The first element of a trail behaves just like a particle and is affected by forces and other manipulators. Please see the documentation for further details.");

					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginVertical("Box");
				{
					if (GUILayout.Button("Missing feature?!", gs))
					{
						tab2 = !tab2;
					}

					if (tab2)
					{
						EditorGUILayout.LabelField("If an advertised feature is missing, it's most likely caused by your Unity install version. Newer versions of Ultimate GPU Particle System will always be uploaded by the latest final Unity release. In order to get those features from the asset store, you have to upgrade you Unity version.");

					}
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical("Box");
				{
					if (GUILayout.Button("First time import", gs))
					{
						tab3 = !tab3;
					}

					if (tab3)
					{
						EditorGUILayout.LabelField("If you import Ultimate GPU Particle system for the first time, errors can occur. Usually you can fix them by right-clicking on the Ultimate GPU Particle System folder and clicking reimport. This usually resolves many errors.");

					}
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical("Box");
				{
					if (GUILayout.Button("Best practices", gs))
					{
						tab4 = !tab4;
					}

					if (tab4)
					{
						EditorGUILayout.LabelField("Ultimate GPU Particle System is a pseudo-instanced gpu particle system. That means, that all particles are always generated and ready in memory for usage. That makes it very fast and efficient but it has a few disadvantages: \n\n 1. Use as few emitters as possible. If you have the same effect multiple times, use the same particle system again using Emit(). \n\n 2. Make sure that you don't use more particles than necessary. The buffer width and height define the size in RAM. \n\n 3. Avoid transparent overdraw. Even though the meta data of particles can be efficiently calculated using shaders, filling the screen with millions of screen size particles is simply not possible with modern hardware.");
					}
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical("Box");
				{
					if (GUILayout.Button("Shader keywords update", gs))
					{
						tab5 = !tab5;
					}

					if (tab5)
					{
						EditorGUILayout.LabelField("In the latest version, the shader variants are being compiled using shader_feature instead of multi_compile. This has the advantage of cutting the time it takes to compile all shader variants, because only shader variants that are being used are being compiled. This improves also the time to build. The disadvantage is that no new Particle Systems can be created during runtime.");
					}
				}
				EditorGUILayout.EndVertical();

				EditorGUILayout.BeginVertical("Box");
				{
					if (GUILayout.Button("Post effects", gs))
					{
						tab6 = !tab6;
					}

					if (tab6)
					{
						EditorGUILayout.LabelField("To make post effects work, you have to import the post processing package from the package manager. Go to Toolbar>Window>Package Manager. In the top left, enable all packages. After the preview packages are refreshed, click on Post Processing and choose version 2.1.2. Then click download.");
					}
				}
				EditorGUILayout.EndVertical();

			}
			EditorGUILayout.EndScrollView();
		}
		EditorGUILayout.EndVertical();

		GUILayout.FlexibleSpace();

		EditorGUILayout.BeginVertical();
		{
			dontShowAgain = EditorGUILayout.Toggle("Don't show again", dontShowAgain);

			if (GUILayout.Button("Close Window"))
			{
				if (dontShowAgain == true)
				{
					System.IO.File.Create(Application.persistentDataPath + "/welcomeMessage.txt");
				}
				else
				{
					if (System.IO.File.Exists(Application.persistentDataPath + "/welcomeMessage.txt"))
					{
						System.IO.File.Delete(Application.persistentDataPath + "/welcomeMessage.txt");
					}
				}

				WelcomeWindow window = (WelcomeWindow)EditorWindow.GetWindow(typeof(WelcomeWindow));
				window.Close();
			}
		}
		EditorGUILayout.EndVertical();
	}
}