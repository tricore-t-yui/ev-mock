using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

/// <summary>
/// 音のスポナー
/// </summary>
public class SoundSpawner : MonoBehaviour
{
    /// <summary>
    ///  再生する音のタイプ
    /// </summary>
    public enum SoundType
    {
        HeartSound,
        Breth,
        Walk,
        Dash,
    }

    [SerializeField]
    GameObject spawnBase = default;                     // 複製元
    [SerializeField]
    AudioClip[] sounds = default;                       // 再生される音達

    AudioClip playSound = default;                      // 再生する音
    AudioClip stopSound = default;                      // 停止する音
    SoundType soundtype = default;                      // 再生する音の種類
    bool isLoop = false;                                // ループするかどうか
    List<Transform> spawnList = new List<Transform>();  // スポーンされたオブジェクトのリスト

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        Play(SoundType.Breth);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        Despawn();
    }

    /// <summary>
    /// デスポーン
    /// </summary>
    void Despawn()
    {
        List<Transform> DespawnList = new List<Transform>();      // スポーンされたオブジェクトのリスト

        foreach (var item in spawnList)
        {
            // デスポーンして、リストから削除
            if (!item.gameObject.activeInHierarchy)
            {
                PoolManager.Pools["Sound"].Despawn(item);
                DespawnList.Add(item);
            }
        }

        foreach (var item in DespawnList)
        {
            spawnList.Remove(item);
        }
    }

    /// <summary>
    /// 再生
    /// </summary>
    /// <param name="type">音のタイプ</param>
    public void Play(SoundType type)
    {
        // 再生される音をセット
        playSound = sounds[(int)type];
        soundtype = type;
        IsLoop(type);

        // スポーンしてリストに追加
        var spawn = PoolManager.Pools["Sound"].Spawn(spawnBase);
        spawnList.Add(spawn);
    }

    /// <summary>
    /// 停止
    /// </summary>
    /// <param name="type">音のタイプ</param>
    public void Stop(SoundType type)
    {
        // 停止される音をセット
        stopSound = sounds[(int)type];
    }

    /// <summary>
    /// 再生される音のゲット関数
    /// </summary>
    public AudioClip GetPlaySound()
    {
        return playSound;
    }

    /// <summary>
    /// 再生される音タイプのゲット関数
    /// </summary>
    public SoundType GetSoundType()
    {
        return soundtype;
    }

    /// <summary>
    /// ループするかどうかのゲット関数
    /// </summary>
    public bool GetIsLoop()
    {
        return isLoop;
    }

    /// <summary>
    /// ループするかどうか
    /// </summary>
    void IsLoop(SoundType type)
    {
        switch (type)
        {
            case SoundType.Breth: isLoop = true; break;
            case SoundType.HeartSound: isLoop = true; break;
            case SoundType.Walk: isLoop = true; break;
            case SoundType.Dash: isLoop = true; break;
            default: isLoop = false; break;
        }
    }

    /// <summary>
    /// 停止される音と一致しているかどうか
    /// </summary>
    public bool IsStopSound(AudioClip audioClip)
    {
        if(stopSound != null && audioClip == stopSound)
        {
            stopSound = null;
            return true;
        }
        else
        {
            return false;
        }
    }
}
