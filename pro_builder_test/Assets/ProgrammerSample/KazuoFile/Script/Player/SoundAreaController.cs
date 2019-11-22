using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音領域のクラス
/// </summary>
public class SoundAreaController : MonoBehaviour
{
    [SerializeField]
    SphereCollider collider = default;      // コライダー
    [SerializeField]
    Transform player = default;             // プレイヤー
    [SerializeField]
    SoundAreaSpawner areaSpawner = default; // スポナー

    float areaDecrease = 0.1f;                 // 1フレームの領域の減少量

    /// <summary>
    /// 起動処理
    /// </summary>
    private void OnEnable()
    {
        transform.position = player.position;
        collider.radius = areaSpawner.GetColliderRadius();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        collider.radius -= areaDecrease;
        if(collider.radius < 0)
        {
            gameObject.SetActive(false);
        }
    }
}
