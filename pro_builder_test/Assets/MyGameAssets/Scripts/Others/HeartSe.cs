using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSe : MonoBehaviour
{
    [SerializeField]
    AudioSource source = default;

    [SerializeField]
    HideStateController.HeartSoundType currentState = HideStateController.HeartSoundType.NORMAL;

    [SerializeField]
    float[] playTime = { 0, 1.2f, 0.5f };
    [SerializeField]
    float[] pitch = { 1.0f, 1.0f, 1.3f };
    [SerializeField]
    float[] volume = { 0, 0.1f, 0.3f };

    float prevPlayTime;

    private void Start()
    {
        source.volume = 0;
        prevPlayTime = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        if(currentState != HideStateController.HeartSoundType.NORMAL)
        {
            int i = (int)currentState;
            if(Time.timeSinceLevelLoad - prevPlayTime > playTime[i])
            {
                PlayInternal(i);
            }
        }
    }

    /// <summary>
    /// 心音の変更
    /// </summary>
    public void ChangeHeartSound(HideStateController.HeartSoundType state)
    {
        currentState = state;
        int i = (int)currentState;
        PlayInternal(i);
    }

    void PlayInternal(int i)
    {
        source.pitch = pitch[i];
        source.volume = volume[i];
        source.Play();
        prevPlayTime = Time.timeSinceLevelLoad;
    }
}
