using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEditor.Animations;

[System.Serializable]
public struct MotionSoundParameter
{
    public string soundName;
    public AudioSource audioSource;
    public bool isLoop;
    public string loopIntervalOfParameterName;
    public float loopIntervalCounterMax;

    [HideInInspector]
    public float loopInterval;
    [HideInInspector]
    public float loopIntervalCounter;
}

public class MotionSoundPlayer : MonoBehaviour
{
    [SerializeField]
    Animator animator = default;

    [SerializeField]
    List<MotionSoundParameter> soundParameters = default;

    void Update()
    {
        foreach(MotionSoundParameter parameter in soundParameters)
        {
            if (parameter.isLoop)
            {
            }
        }
    }
}
