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
        Walk,           // 歩き
        Dash,           // ダッシュ
        DamageObject,   // ダメージ床
    }

    [SerializeField]
    GameObject spawnBase = default;                     // 複製元
    [SerializeField]
    AudioClip[] sounds = default;                       // 再生される音達

    AudioClip stopSound = default;                      // 停止する音
    bool isLoop = false;                                // ループするかどうか
    List<Transform> spawnList = new List<Transform>();  // スポーンされたオブジェクトのリスト

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
        var playSound = sounds[(int)type];

        // スポーンしてリストに追加
        var spawn = PoolManager.Pools["Sound"].Spawn(spawnBase);
        var spawnC = spawn.GetComponent<SoundController>();
        spawnC.Spawned(type, playSound);
        spawnList.Add(spawn);
    }
}
