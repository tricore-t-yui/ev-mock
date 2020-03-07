using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathSe : MonoBehaviour
{
    [SerializeField]
    AudioSource source = default;

    [SerializeField]
    PlayerBreathController.BrethState currentState = PlayerBreathController.BrethState.NOTCONFUSION;

    [SerializeField] AudioClip holdClip;
    [SerializeField] AudioClip overClip;
    [SerializeField] AudioClip endClip;

    [SerializeField]
    AudioClip[] stateLoopClip;
    [SerializeField]
    float[] playTime = { 0, 1.2f, 1.2f, 1.2f, 0.5f };
    [SerializeField]
    float[] pitch = { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
    [SerializeField]
    float[] volume = { 0, 0.1f, 0.1f, 0.1f, 0.3f };

    float prevPlayTime;
    bool isBreathHold;

#if DEBUG
    [SerializeField] PlayerBreathController.BrethState debugState = PlayerBreathController.BrethState.NOTCONFUSION;
    [SerializeField] bool debugBreathHold;
    bool debugPrevBreathHold;
#endif

    private void Start()
    {
        source.volume = 0;
        prevPlayTime = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        if (!isBreathHold && currentState != PlayerBreathController.BrethState.NOTCONFUSION)
        {
            int i = (int)currentState;
            if (Time.timeSinceLevelLoad - prevPlayTime > playTime[i])
            {
                PlayInternal();
            }
        }
    }

    public void ChangeBreathState(PlayerBreathController.BrethState state)
    {
        currentState = state;

    }

    public void OnStartBreathHold()
    {
        isBreathHold = true;
    }

    public void OnEndBreathHold()
    {
        prevPlayTime = Time.timeSinceLevelLoad;
        isBreathHold = false;
    }

    void PlayInternal()
    {
        int i = (int)currentState;
        source.pitch = pitch[i];
        source.volume = volume[i];
        source.Play();
        prevPlayTime = Time.timeSinceLevelLoad;
    }

#if UNITY_EDITOR && DEBUG
    void OnValidate()
    {
        if(currentState != debugState)
        {
            ChangeBreathState(debugState);
        }
        if(debugBreathHold && !debugBreathHold)
        {
            OnStartBreathHold();
        }
        if (debugBreathHold && !debugBreathHold)
        {
            OnEndBreathHold();
        }
    }
#endif
}
