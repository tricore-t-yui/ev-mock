using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音領域のクラス
/// </summary>
public class SoundAreaController : MonoBehaviour
{
    [SerializeField]
    SphereCollider soundCollider = default; // コライダー
    [SerializeField]
    Transform player = default;             // プレイヤー
    [SerializeField]
    SoundAreaSpawner spawner = default;     // スポナー

    [SerializeField]
    float largeBorder = 3;                  // 大きな領域のボーダーライン
    [SerializeField]
    float smallAreaDecrease = 0.5f;         // 小さな領域の1フレームの領域の減少量
    [SerializeField]
    float largeAreaDecrease = 0.1f;         // 大きな領域の1フレームの領域の減少量

    float areaDecrease = 0;                 // 1フレームの領域の減少量

    /// <summary>
    /// 起動処理
    /// </summary>
    private void OnEnable()
    {
        transform.position = player.position;
        soundCollider.radius = spawner.GetColliderRadius();

        // 領域の大きさに応じて減少量を変更
        if(soundCollider.radius > largeBorder)
        {
            areaDecrease = largeAreaDecrease;
        }
        else
        {
            areaDecrease = smallAreaDecrease;
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        soundCollider.radius -= areaDecrease;
        if(soundCollider.radius < 0)
        {
            gameObject.SetActive(false);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        UnityEditor.Handles.color = new Color(1,0,1);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, soundCollider.radius);
    }
#endif
}
