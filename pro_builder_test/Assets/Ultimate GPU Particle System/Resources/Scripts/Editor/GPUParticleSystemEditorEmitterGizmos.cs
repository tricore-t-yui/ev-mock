using UnityEngine;
using UnityEditor;
using System.Collections;

public partial class GPUParticleSystemEditor
{
#pragma warning disable 618
    private void DrawCross()
    {
        Vector3 p1 = particleSystem.transform.position + (particleSystem.transform.rotation * Vector3.back) * 0.5f;
        Vector3 p2 = particleSystem.transform.position + (particleSystem.transform.rotation * Vector3.left) * 0.5f;
        Vector3 p3 = particleSystem.transform.position + (particleSystem.transform.rotation * Vector3.down) * 0.5f;

        Handles.color = new Color(0.717f, 0.929f, 1f, 1f);

        Handles.color = Color.blue;
        Handles.DrawLine(p1, p1 + particleSystem.transform.rotation * Vector3.forward);
        Handles.color = Color.red;
        Handles.DrawLine(p2, p2 + particleSystem.transform.rotation * Vector3.right);
        Handles.color = Color.green;
        Handles.DrawLine(p3, p3 + particleSystem.transform.rotation * Vector3.up);
        Handles.color = Color.white;
    }

    private void DrawBox()
    {
        if (particleSystem == null)
            return;

        Handles.color = new Color(0.717f, 0.929f, 1f, 1f);

        Vector3 BottomLeftBack = particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(-param1.floatValue, -param2.floatValue, -param3.floatValue);
        Vector3 BottomRightBack = particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(param1.floatValue, -param2.floatValue, -param3.floatValue);
        Vector3 BottomLeftFront = particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(-param1.floatValue, -param2.floatValue, param3.floatValue);
        Vector3 BottomRightFront = particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(param1.floatValue, -param2.floatValue, param3.floatValue);

        Vector3 TopLeftBack = particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(-param1.floatValue, param2.floatValue, -param3.floatValue);
        Vector3 TopRightBack = particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(param1.floatValue, param2.floatValue, -param3.floatValue);
        Vector3 TopLeftFront = particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(-param1.floatValue, param2.floatValue, param3.floatValue);
        Vector3 TopRightFront = particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(param1.floatValue, param2.floatValue, param3.floatValue);

        Handles.DrawLine(BottomLeftBack, BottomRightBack);
        Handles.DrawLine(BottomLeftBack, BottomLeftFront);
        Handles.DrawLine(BottomRightBack, BottomRightFront);
        Handles.DrawLine(BottomLeftFront, BottomRightFront);

        Handles.DrawLine(BottomLeftBack, TopLeftBack);
        Handles.DrawLine(BottomRightBack, TopRightBack);
        Handles.DrawLine(BottomLeftFront, TopLeftFront);
        Handles.DrawLine(BottomRightFront, TopRightFront);

        Handles.DrawLine(TopLeftBack, TopRightBack);
        Handles.DrawLine(TopLeftBack, TopLeftFront);
        Handles.DrawLine(TopRightBack, TopRightFront);
        Handles.DrawLine(TopLeftFront, TopRightFront);

        float SX = param1.floatValue;
        float SY = param2.floatValue;
        float SZ = param3.floatValue;

        float sliderSize = Vector3.Distance(SceneView.lastActiveSceneView.camera.transform.position, particleSystem.transform.position) / 150f;

        Vector3 InitialPos1 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.left * SX);
        Vector3 ScaledPos1 = Handles.Slider(InitialPos1, particleSystem.transform.right, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos2 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.right * SX);
        Vector3 ScaledPos2 = Handles.Slider(InitialPos2, particleSystem.transform.right, sliderSize, Handles.DotCap, 0f);

        Vector3 InitialPos3 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.up * SY);
        Vector3 ScaledPos3 = Handles.Slider(InitialPos3, particleSystem.transform.up, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos4 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.down * SY);
        Vector3 ScaledPos4 = Handles.Slider(InitialPos4, particleSystem.transform.up, sliderSize, Handles.DotCap, 0f);

        Vector3 InitialPos5 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.forward * SZ);
        Vector3 ScaledPos5 = Handles.Slider(InitialPos5, particleSystem.transform.forward, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos6 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.back * SZ);
        Vector3 ScaledPos6 = Handles.Slider(InitialPos6, particleSystem.transform.forward, sliderSize, Handles.DotCap, 0f);

		if(Mathf.Abs(param1.floatValue - (Vector3.Distance(ScaledPos1, ScaledPos2) / 2f)) > 0.01f)
			param1.floatValue = Vector3.Distance(ScaledPos1, ScaledPos2) / 2f;

		if (Mathf.Abs(param2.floatValue - (Vector3.Distance(ScaledPos3, ScaledPos4) / 2f)) > 0.01f)
			param2.floatValue = Vector3.Distance(ScaledPos3, ScaledPos4) / 2f;

		if (Mathf.Abs(param3.floatValue - (Vector3.Distance(ScaledPos5, ScaledPos6) / 2f)) > 0.01f)
			param3.floatValue = Vector3.Distance(ScaledPos5, ScaledPos6) / 2f;

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawHemiSphere()
    {
        if (particleSystem == null)
            return;

        float length = param1.floatValue;
        float sliderSize = Vector3.Distance(SceneView.lastActiveSceneView.camera.transform.position, particleSystem.transform.position) / 150f;

        Handles.color = new Color(0.717f, 0.929f, 1f, 1f);

        Vector3 InitialPos1 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.left * length);
        Vector3 ScaledPos1 = Handles.Slider(InitialPos1, particleSystem.transform.right, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos2 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.right * length);
        Vector3 ScaledPos2 = Handles.Slider(InitialPos2, particleSystem.transform.right, sliderSize, Handles.DotCap, 0f);

        Vector3 InitialPos3 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.up * length);
        Vector3 ScaledPos3 = Handles.Slider(InitialPos3, particleSystem.transform.up, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos4 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.down * length);
        Vector3 ScaledPos4 = InitialPos4;

        Vector3 InitialPos5 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.forward * length);
        Vector3 ScaledPos5 = Handles.Slider(InitialPos5, particleSystem.transform.forward, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos6 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.back * length);
        Vector3 ScaledPos6 = Handles.Slider(InitialPos6, particleSystem.transform.forward, sliderSize, Handles.DotCap, 0f);

        float value1 = Vector3.Distance(ScaledPos1, ScaledPos2) / 2f;
        float value2 = Vector3.Distance(ScaledPos3, ScaledPos4) / 2f;
        float value3 = Vector3.Distance(ScaledPos5, ScaledPos6) / 2f;

		if (Mathf.Abs(param1.floatValue - ((value1 + value2 + value3) / 3f)) > 0.01f)
			param1.floatValue = (value1 + value2 + value3) / 3f;

        serializedObject.ApplyModifiedProperties();

        Handles.DrawWireArc(particleSystem.transform.position, particleSystem.transform.forward, particleSystem.transform.right, 180f, param1.floatValue);
        Handles.DrawWireArc(particleSystem.transform.position, -particleSystem.transform.right, particleSystem.transform.forward, 180f, param1.floatValue);
        Handles.CircleCap(0, particleSystem.transform.position, particleSystem.transform.rotation * Quaternion.Euler(90f, 0f, 0f), param1.floatValue);
    }

    private void DrawCircle()
    {
        if (particleSystem == null)
            return;

        float length = param1.floatValue;
        float sliderSize = Vector3.Distance(SceneView.lastActiveSceneView.camera.transform.position, particleSystem.transform.position) / 150f;

        Handles.color = new Color(0.717f, 0.929f, 1f, 1f);

        Vector3 InitialPos1 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.left * length);
        Vector3 ScaledPos1 = Handles.Slider(InitialPos1, particleSystem.transform.right, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos2 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.right * length);
        Vector3 ScaledPos2 = Handles.Slider(InitialPos2, particleSystem.transform.right, sliderSize, Handles.DotCap, 0f);
        
        Vector3 InitialPos5 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.forward * length);
        Vector3 ScaledPos5 = Handles.Slider(InitialPos5, particleSystem.transform.forward, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos6 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.back * length);
        Vector3 ScaledPos6 = Handles.Slider(InitialPos6, particleSystem.transform.forward, sliderSize, Handles.DotCap, 0f);
        
        float value1 = Vector3.Distance(ScaledPos1, ScaledPos2) / 2f;
        float value3 = Vector3.Distance(ScaledPos5, ScaledPos6) / 2f;

		if (Mathf.Abs(param1.floatValue - ((value1 + value3) / 2f)) > 0.01f)
		{
			param1.floatValue = (value1 + value3) / 2f;
		}
        
        serializedObject.ApplyModifiedProperties();

        Handles.color = new Color(0.717f, 0.929f, 1f, 1f);
        Handles.DrawWireArc(particleSystem.transform.position, particleSystem.transform.up, particleSystem.transform.right, 360f, param1.floatValue);
    }

    private void DrawEdge()
    {
        if (particleSystem == null)
            return;

        float Length = param1.floatValue / 2f;
        Handles.color = new Color(0.717f, 0.929f, 1f, 1f);

        float sliderSize = Vector3.Distance(SceneView.lastActiveSceneView.camera.transform.position, particleSystem.transform.position) / 150f;

        Vector3 InitialPos1 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.left * Length);
        Vector3 ScaledPos1 = Handles.Slider(InitialPos1, particleSystem.transform.right, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos2 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.right * Length);
        Vector3 ScaledPos2 = Handles.Slider(InitialPos2, particleSystem.transform.right, sliderSize, Handles.DotCap, 0f);

        Handles.DrawLine(ScaledPos1, ScaledPos2);

		if (Mathf.Abs(param1.floatValue - Vector3.Distance(ScaledPos1, ScaledPos2)) > 0.01f)
		{
			param1.floatValue = Vector3.Distance(ScaledPos1, ScaledPos2);
		}
		
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSphere()
    {
        if (particleSystem == null)
            return;

        float Length = param1.floatValue;
        Handles.color = new Color(0.717f, 0.929f, 1f, 1f);

        float sliderSize = Vector3.Distance(SceneView.lastActiveSceneView.camera.transform.position, particleSystem.transform.position) / 150f;

        Vector3 InitialPos1 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.left * Length);
        Vector3 ScaledPos1 = Handles.Slider(InitialPos1, particleSystem.transform.right, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos2 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.right * Length);
        Vector3 ScaledPos2 = Handles.Slider(InitialPos2, particleSystem.transform.right, sliderSize, Handles.DotCap, 0f);

        Vector3 InitialPos3 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.up * Length);
        Vector3 ScaledPos3 = Handles.Slider(InitialPos3, particleSystem.transform.up, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos4 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.down * Length);
        Vector3 ScaledPos4 = Handles.Slider(InitialPos4, particleSystem.transform.up, sliderSize, Handles.DotCap, 0f);

        Vector3 InitialPos5 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.forward * Length);
        Vector3 ScaledPos5 = Handles.Slider(InitialPos5, particleSystem.transform.forward, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos6 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.back * Length);
        Vector3 ScaledPos6 = Handles.Slider(InitialPos6, particleSystem.transform.forward, sliderSize, Handles.DotCap, 0f);

        float value1 = Vector3.Distance(ScaledPos1, ScaledPos2) / 2f;
        float value2 = Vector3.Distance(ScaledPos3, ScaledPos4) / 2f;
        float value3 = Vector3.Distance(ScaledPos5, ScaledPos6) / 2f;

		
		if (Mathf.Abs(param1.floatValue - ((value1 + value2 + value3) / 3f)) > 0.01f)
		{
			param1.floatValue = (value1 + value2 + value3) / 3f;
		}
		
        serializedObject.ApplyModifiedProperties();

        Handles.CircleCap(0, particleSystem.transform.position, particleSystem.transform.rotation, param1.floatValue);
        Handles.CircleCap(0, particleSystem.transform.position, particleSystem.transform.rotation * Quaternion.Euler(0f, 90f, 0f), param1.floatValue);
        Handles.CircleCap(0, particleSystem.transform.position, particleSystem.transform.rotation * Quaternion.Euler(90f, 0f, 0f), param1.floatValue);
    }

	private void DrawTextureEmitter()
	{
		Vector3 p1 = particleSystem.transform.rotation * new Vector3(param1.floatValue, param2.floatValue, 0f)+particleSystem.transform.position;
		Vector3 p2 = particleSystem.transform.rotation * new Vector3(-param1.floatValue, param2.floatValue, 0f) + particleSystem.transform.position;
		Vector3 p3 = particleSystem.transform.rotation * new Vector3(param1.floatValue, -param2.floatValue, 0f) + particleSystem.transform.position;
		Vector3 p4 = particleSystem.transform.rotation * new Vector3(-param1.floatValue, -param2.floatValue, 0f) + particleSystem.transform.position;

		Handles.DrawLine(p1, p2);
		Handles.DrawLine(p2, p4);
		Handles.DrawLine(p3, p4);
		Handles.DrawLine(p3, p1);
	}

    private void DrawConeEmitter()
    {
        if (particleSystem == null)
            return;

        float Radius = param1.floatValue;
        float Angle = param2.floatValue;
        float Length = param3.floatValue;
        Handles.color = new Color(0.717f, 0.929f, 1f, 1f);

        float sliderSize = Vector3.Distance(SceneView.lastActiveSceneView.camera.transform.position, particleSystem.transform.position) / 150f;

        Vector3 InitialPos1 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.left * Radius);
        Vector3 ScaledPos1 = Handles.Slider(InitialPos1, particleSystem.transform.right, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos2 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.right * Radius);
        Vector3 ScaledPos2 = Handles.Slider(InitialPos2, particleSystem.transform.right, sliderSize, Handles.DotCap, 0f);

        Vector3 InitialPos3 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.up * Radius);
        Vector3 ScaledPos3 = Handles.Slider(InitialPos3, particleSystem.transform.up, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos4 = particleSystem.transform.position + particleSystem.transform.rotation * (Vector3.down * Radius);
        Vector3 ScaledPos4 = Handles.Slider(InitialPos4, particleSystem.transform.up, sliderSize, Handles.DotCap, 0f);

        float AngleOffset = Mathf.Tan(Angle * Mathf.Deg2Rad) * Length;
        /*
        Vector3 InitialPos5 = particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(-Radius - AngleOffset, 0f, Length);
        Vector3 ScaledPos5 = Handles.Slider(InitialPos5, particleSystem.transform.forward, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos6 = particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(Radius + AngleOffset, 0f, Length);
        Vector3 ScaledPos6 = Handles.Slider(InitialPos6, particleSystem.transform.forward, sliderSize, Handles.DotCap, 0f);

        Vector3 InitialPos7 = particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, -Radius - AngleOffset, Length);
        Vector3 ScaledPos7 = Handles.Slider(InitialPos7, particleSystem.transform.forward, sliderSize, Handles.DotCap, 0f);
        Vector3 InitialPos8 = particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, Radius + AngleOffset, Length);
        Vector3 ScaledPos8 = Handles.Slider(InitialPos8, particleSystem.transform.forward, sliderSize, Handles.DotCap, 0f);
        */
        Vector3 InitialPos9 = particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, 0f, Length);
        Vector3 ScaledPos9 = Handles.Slider(InitialPos9, particleSystem.transform.forward, sliderSize, Handles.DotCap, 0f);

        float value1 = Vector3.Distance(ScaledPos1, ScaledPos2) / 2f;
        float value2 = Vector3.Distance(ScaledPos3, ScaledPos4) / 2f;

		//float d = AngleOffset + Vector3.Distance(ScaledPos5,InitialPos5);
		//float h = Mathf.Sqrt(Mathf.Pow(d, 2f) + Mathf.Pow(Length, 2f));

		//Debug.Log(d + " " + AngleOffset + " " + h);

        param1.floatValue = (value1 + value2) / 2f;
		//param2.floatValue = Mathf.Asin(d / h) * Mathf.Rad2Deg;

		float dist = Vector3.Distance(ScaledPos9, particleSystem.transform.position);

		if (Mathf.Abs(param3.floatValue - dist) > 0.01f)
		{
			param3.floatValue = dist;
		}

        serializedObject.ApplyModifiedProperties();



        Handles.CircleCap(0, particleSystem.transform.position, particleSystem.transform.rotation, param1.floatValue);

        Handles.CircleCap(0,
            particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, 0f, param3.floatValue),
            particleSystem.transform.rotation, param1.floatValue + AngleOffset);

        Handles.DrawLine(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(param1.floatValue, 0f, 0f),
            particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(param1.floatValue + AngleOffset, 0f, param3.floatValue));
        Handles.DrawLine(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, param1.floatValue, 0f),
            particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, param1.floatValue + AngleOffset, param3.floatValue));
        Handles.DrawLine(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(-param1.floatValue, 0f, 0f),
            particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(-param1.floatValue - AngleOffset, 0f, param3.floatValue));
        Handles.DrawLine(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, -param1.floatValue, 0f),
            particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, -param1.floatValue - AngleOffset, param3.floatValue));

        //Handles.DrawLine();
        /*
        Vector3 v1 = Handles.Slider(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(param1.floatValue, 0f, 0f), Vector3.right, sliderSize,
            Handles.DotCap, 0f);

        Vector3 v2 = Handles.Slider(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, param1.floatValue, 0f), Vector3.right, sliderSize,
            Handles.DotCap, 0f);

        Vector3 v3 = Handles.Slider(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(-param1.floatValue, 0f, 0f), Vector3.right, sliderSize,
            Handles.DotCap, 0f);

        Vector3 v4 = Handles.Slider(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, -param1.floatValue,  0f), Vector3.right, sliderSize,
            Handles.DotCap, 0f);
        */

        Handles.CircleCap(0,
            particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, 0f, param3.floatValue),
            particleSystem.transform.rotation, param1.floatValue + AngleOffset);

        Handles.DrawLine(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(param1.floatValue, 0f, 0f),
            particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(param1.floatValue + AngleOffset, 0f, param3.floatValue));
        Handles.DrawLine(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, param1.floatValue, 0f),
            particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, param1.floatValue + AngleOffset, param3.floatValue));
        Handles.DrawLine(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(-param1.floatValue, 0f, 0f),
            particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(-param1.floatValue - AngleOffset, 0f, param3.floatValue));
        Handles.DrawLine(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, -param1.floatValue, 0f),
            particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, -param1.floatValue - AngleOffset, param3.floatValue));
        /*
        Vector3 v5 = Handles.Slider(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(param1.floatValue + AngleOffset, 0f, param3.floatValue), Vector3.right, sliderSize,
            Handles.DotCap, 0f);

        Vector3 v6 = Handles.Slider(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, param1.floatValue + AngleOffset, param3.floatValue), Vector3.right, sliderSize,
            Handles.DotCap, 0f);

        Vector3 v7 = Handles.Slider(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(-param1.floatValue - AngleOffset, 0f, param3.floatValue), Vector3.right, sliderSize,
            Handles.DotCap, 0f);

        Vector3 v8 = Handles.Slider(particleSystem.transform.position + particleSystem.transform.rotation * new Vector3(0f, -param1.floatValue - AngleOffset, param3.floatValue), Vector3.right, sliderSize,
            Handles.DotCap, 0f);
            */
        //Radius.floatValue = v1.x;
        //serializedObject.ApplyModifiedProperties();

    }
#pragma warning restore 618
}
