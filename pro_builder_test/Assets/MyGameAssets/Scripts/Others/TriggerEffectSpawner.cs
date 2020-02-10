using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

/// <summary>
/// トリガーエフェクトのスポナー
/// </summary>
public class TriggerEffectSpawner : MonoBehaviour
{
    /// <summary>
    /// エフェクトのタイプ
    /// </summary>
    public enum EffectType
    {
        // NOTE:k.oishi 名前がわからないのでとりあえずサンプルとして用意しています。
        SAMPLE1,
        SAMPLE2,
        SAMPLE3,
        SAMPLE4,
        SAMPLE5,
    }

    [SerializeField]
    GameObject spawnBase = default;                               // 複製元
    [SerializeField]
    Sprite[] tirggerEffect = default;                             // 表示される画像達

    public Sprite displaySprite { get; private set; } = default;  // 表示する画像
    List<Transform> spawnList = new List<Transform>();            // スポーンされたオブジェクトのリスト

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
    public void Spawn(EffectType type)
    {
        // 再生される音をセット
        displaySprite = tirggerEffect[(int)type];

        // スポーンしてリストに追加
        var spawn = PoolManager.Pools["Sound"].Spawn(spawnBase);
        spawnList.Add(spawn);
    }
}
