﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// トリガーに触れたとき音を出す
/// </summary>
public class SoundTrigger : MonoBehaviour
{
    [SerializeField, Tooltip("フラグがオンなら発動チャンスが一度きり")]
    bool isOneChance = false;

    [SerializeField, Range(1, 100), Tooltip("イベントの出現確率")]
    int appearRate = 100;

    [SerializeField, Tooltip("鳴らすオーディオのリスト。複数あった場合はランダム再生")]
    List<AudioClip> audioList;

    [SerializeField, Tooltip("トリガーグループ。0以外で数字が同じやつは、発動したら消える(画像エフェクトと共通)")]
    int triggerGrup = 0;

    AudioSource source;

    public int TriggerGroup{ get{ return triggerGrup; } }

    private void Awake()
    {
        source = GetComponentInChildren<AudioSource>();
        EffectTriggerGroup.Add(this);
    }

    private void OnDestroy()
    {
        EffectTriggerGroup.Remove(this);
    }

    /// <summary>
    /// トリガーに触れたとき
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var rate = Random.Range(0.0f, 1.0f);
            Debug.Log("Enter rate:" + rate);
            if (rate > 1.0f - (appearRate / 100.0f))
            {
                EffectTriggerGroup.Remove(this);
                EffectTriggerGroup.DestroyGroup(triggerGrup);
                StartCoroutine(PlayInternal());
            }
            else if (isOneChance)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator PlayInternal()
    {
        if(audioList.Count == 1)
        {
            source.clip = audioList[0];
        }
        else
        {
            var idx = Random.Range(0, audioList.Count * 100 - 1) / 100;
            //Debug.Log("idx:"+idx);
            source.clip = audioList[idx];
        }
        source.Play();
        while(source.isPlaying)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}