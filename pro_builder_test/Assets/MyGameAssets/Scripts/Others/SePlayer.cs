/******************************************************************************/
/*!    \brief  指定SEを再生する.
*******************************************************************************/

using UnityEngine;
using PathologicalGames;
using System.Collections.Generic;
public class SePlayer : MonoBehaviour
{
    [SerializeField]
    SpawnPool sePool = default;

    List<AudioSource> endWatchSeList = new List<AudioSource>();

    public static SePlayer Inst { get; private set; }
    private void Awake()
    {
        Inst = this;
    }

    /// <summary>
    /// 再生.
    /// </summary>
    public void PlaySe(AudioSource copySource, float volumeRandomRange = 0.0f, float pitchRandomRange = 0.0f)
    {
        Transform spawnedSeTrans;
        spawnedSeTrans = sePool.Spawn(copySource.gameObject);
        spawnedSeTrans.position = copySource.transform.position;
        AudioSource spawnedSe = spawnedSeTrans.GetComponent<AudioSource>();
        spawnedSe.clip = copySource.clip;
        spawnedSe.outputAudioMixerGroup = copySource.outputAudioMixerGroup;
        spawnedSe.loop = copySource.loop;
        spawnedSe.ignoreListenerVolume = copySource.ignoreListenerVolume;
        spawnedSe.ignoreListenerPause = copySource.ignoreListenerPause;
        spawnedSe.velocityUpdateMode = copySource.velocityUpdateMode;
        spawnedSe.panStereo = copySource.panStereo;
        spawnedSe.spatialBlend = copySource.spatialBlend;
        spawnedSe.spatialize = copySource.spatialize;
        spawnedSe.spatializePostEffects = copySource.spatializePostEffects;
        spawnedSe.reverbZoneMix = copySource.reverbZoneMix;
        spawnedSe.bypassEffects = copySource.bypassEffects;
        spawnedSe.bypassListenerEffects = copySource.bypassListenerEffects;
        spawnedSe.bypassReverbZones = copySource.bypassReverbZones;
        spawnedSe.dopplerLevel = copySource.dopplerLevel;
        spawnedSe.spread = copySource.spread;
        spawnedSe.priority = copySource.priority;
        spawnedSe.mute = copySource.mute;
        spawnedSe.minDistance = copySource.minDistance;
        spawnedSe.maxDistance = copySource.maxDistance;
        spawnedSe.time = copySource.time;

        // 一つ目、二つ目のfloatパラメータをそれぞれボリュームとピッチに設定.
        spawnedSe.volume = copySource.volume + Random.Range(-volumeRandomRange, volumeRandomRange);
        spawnedSe.pitch = copySource.pitch + Random.Range(-pitchRandomRange, pitchRandomRange);
        spawnedSe.Play();

        endWatchSeList.Add(spawnedSe);
    }


    /// <summary>
    /// 再生終了したものをすべて削除する.
    /// </summary>
    void LateUpdate()
    {
        List<AudioSource> removeSeList = endWatchSeList.FindAll(se => !se.isPlaying);
        foreach (var item in removeSeList)
        {
            sePool.Despawn(item.transform);
            endWatchSeList.Remove(item);
        }
    }
    /// <summary>
    /// 全ての音を停止させる.
    /// </summary>
    public void StopSeAll()
    {
        List<AudioSource> removeSeList = endWatchSeList.FindAll(se => !se.isPlaying);
        foreach (var item in removeSeList)
        {
            item.Stop();
        }
    }
}