using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnDrawGizmos()
    {
        // スポーン位置に球体を表示
        Gizmos.DrawSphere(spawnPos, 0.5f);
    }
}
