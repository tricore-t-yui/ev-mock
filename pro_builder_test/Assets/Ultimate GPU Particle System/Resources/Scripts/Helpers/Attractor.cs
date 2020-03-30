using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attractor
{
	public Transform attractor;
	public float strength = 1f;
	public float attenuation = 1f;

	public Attractor(Transform attractor, float strength, float attenuation)
	{
		this.attractor = attractor;
		this.strength = strength;
		this.attenuation = attenuation;
	}
}
