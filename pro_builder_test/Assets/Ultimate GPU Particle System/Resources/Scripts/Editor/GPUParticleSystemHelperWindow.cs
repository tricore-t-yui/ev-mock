using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.AnimatedValues;
using System.IO;
using System.Net.Mail;

public class GPUParticleSystemHelperWindow : EditorWindow
{
	private bool tab1 = false;
	private bool tab2 = false;
	private bool tab3 = false;
	private bool tab4 = false;
	private bool tab5 = false;
	private bool tab6 = false;

	public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(GPUParticleSystemHelperWindow), true, "Help & Trouble shooting");
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
			EditorGUILayout.LabelField("FAQ");

			tab1 = DrawTab("What is a GPU Particle System good at?", "A GPU Particle System is good at displaying many simple particles at the same time. Keep in mind, that you should preload the particle system because allocating particles, buffers and other data can take a long time. This will slow down your game." +
						"Instead, load the particle system with your level and use the same particle system multiple times (Reposition and emit)", tab1);

			tab2 = DrawTab("I cant see particles", "Make sure, that emission is enabled, the size is bigger than 0, you are using a texture that is not completely black (add) or transparent (alpha blended)." +
				"If you use stretched billboards or tail stretched billboards and the velocity is 0, the size will also be 0. Fix this by incresing the velocity. Somtimes, starting the game fixes this problem." +
			"Using a texture on a point particle can make particles invisible.", tab2);

			tab3 = DrawTab("The turbulence is not working. All particles fly into the same direction", "The Turbulence texture or .fga file have not been serialized properly. Try to drag and drop the texture or fga file agin into the corresponding slot or disable and reenable the particle system.", tab3);

			tab4 = DrawTab("Another question", "Another Answer", tab4);

			tab5 = DrawTab("Another question", "Another Answer", tab5);

			tab6 = DrawTab("Another question", "Another Answer", tab6);
		}
		EditorGUILayout.EndVertical();
	}

	private bool DrawTab(string question, string answer, bool show)
	{
		if (GUILayout.Button(question, EditorStyles.toolbarButton))
			show = !show;

		if (show)
		{
			EditorGUILayout.HelpBox(answer, MessageType.Info);
		}

		return show;
	}
}
