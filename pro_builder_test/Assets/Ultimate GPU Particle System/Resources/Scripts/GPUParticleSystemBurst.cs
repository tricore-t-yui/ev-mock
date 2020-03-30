using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GPUParticleSystemBurst
{
	public int minBurst = 150;
	public int maxBurst = 150;
	public float burstTime = 0.0f;
	public float burstProbability = 1.0f;
	public bool playOnce = false;

	public int GetBurst()
	{
		if (Random.Range(0.0f, 1.0f) <= burstProbability)
		{
			return Random.Range(minBurst, maxBurst);
		} else {
			return 0;
		}
		
	}
}
