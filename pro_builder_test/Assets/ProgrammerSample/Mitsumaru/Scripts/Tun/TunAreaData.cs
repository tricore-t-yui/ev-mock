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

    // ハイドオブジェクト
    [SerializeField]
    List<GameObject> hideObject = default;


}
