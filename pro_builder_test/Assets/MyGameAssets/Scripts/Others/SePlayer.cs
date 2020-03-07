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

    [SerializeField]
    AudioSource seCopySource2D = default;

    [SerializeField]
    AudioSource seCopySource3D = default;

    List<AudioSource> endWatchSeList = new List<AudioSource>();

    public static SePlayer Inst { get; private set; }
    private void Awake()
    {
        Inst = this;
    }

    /// <summary>
    /// 再生.
    /// </summary>
    public void PlaySe(AudioClip clip, float volume, float pitch, float volumeRandomRange = 0.0f, float pitchRandomRange = 0.0f)
    {
        PlaySeInternal(transform.position, seCopySource2D, clip, volume, pitch, volumeRandomRange, pitchRandomRange);
    }
    public void PlaySe3D(Vector3 pos, AudioClip clip, float volume, float pitch, float volumeRandomRange = 0.0f, float pitchRandomRange = 0.0f)
    {
        PlaySeInternal(pos, seCopySource3D, clip, volume, pitch, volumeRandomRange, pitchRandomRange);
    }
    void PlaySeInternal(Vector3 pos, AudioSource copySource, AudioClip clip, float volume, float pitch, float volumeRandomRange, float pitchRandomRange)
    {
        Transform spawnedSeTrans;
        spawnedSeTrans = sePool.Spawn(copySource.gameObject);
        spawnedSeTrans.position = pos;
        AudioSource spawnedSe = spawnedSeTrans.GetComponent<AudioSource>();

        // 一つ目、二つ目のfloatパラメータをそれぞれボリュームとピッチに設定.
        spawnedSe.volume = volume + Random.Range(-volumeRandomRange, volumeRandomRange);
        spawnedSe.pitch = pitch + Random.Range(-pitchRandomRange, pitchRandomRange);
        spawnedSe.clip = clip;
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