using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ツンのエリアのデータ
/// </summary>
public class TunAreaData : MonoBehaviour
{
    // スポーン位置
    [SerializeField]
    Vector3 spawnPos = Vector3.zero;
    public Vector3 SpawnPos => spawnPos;

    // ハイドオブジェクト
    [SerializeField]
    List<GameObject> hideObject = default;
    public IReadOnlyList<GameObject> HideObject => hideObject;

    // ナビメッシュサーフェス
    NavMeshSurface surface = null;

    // エリアのBouds
    Bounds areaBounds;

    private void Start()
    {
        // ナビメッシュサーフェスを取得
        surface = GetComponent<NavMeshSurface>();
        // Boundsを生成
        areaBounds = new Bounds(transform.position + surface.center, surface.size);
    }

    /// <summary>
    /// 指定位置がBoudsの範囲内か
    /// </summary>
    public bool IsBoudsContains(Vector3 pos)
    {
        return areaBounds.Contains(pos);
    }

    private void OnDrawGizmos()
    {
        // スポーン位置に球体を表示
        Gizmos.DrawSphere(spawnPos, 0.5f);
        Gizmos.DrawWireCube(areaBounds.center, areaBounds.size);
    }
}
