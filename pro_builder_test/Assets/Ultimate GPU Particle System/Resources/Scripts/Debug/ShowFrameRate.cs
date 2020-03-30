using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 [ExecuteInEditMode]
public class ShowFrameRate : MonoBehaviour
{
	public Text text;

	public void OnGUI()
	{
		if (text == null)
		{
			GUI.Label(new Rect(Screen.width / 2f, 0f, 150f, 50f), (1f / Time.smoothDeltaTime).ToString("f1"));
		}
		else {
			text.text = (1f / Time.smoothDeltaTime).ToString("f1");
		}
		
	}
}
