using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEditor.Animations;

[System.Serializable]
public struct MotionSoundParameter
{
    public AudioSource audioSource;
    public float loopIntervalCounterMax;

    [HideInInspector]
    public float loopIntervalCounter;
}

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    Animator animator = default;

    [SerializeField]
    List<MotionSoundParameter> soundParameters = default;

    void Update()
    {
        foreach(MotionSoundParameter parameter in soundParameters)
        {
           
        }
    }
}
