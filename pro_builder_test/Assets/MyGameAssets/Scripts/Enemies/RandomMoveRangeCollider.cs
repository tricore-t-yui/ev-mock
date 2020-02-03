using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

/// <summary>
/// 徘徊型のランダム移動のコライダー
/// </summary>
public class RandomMoveRangeCollider : MonoBehaviour
{
    // スフィアコライダー
    SphereCollider sphereCollider = null;

    // 範囲から抜けた瞬間のコールバック
    UnityAction<string> onRangeExit;

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        // スフィアコライダーのコンポーネントを取得
        sphereCollider = GetComponent<SphereCollider>() ?? sphereCollider;
    }

    /// <summary>
    /// コールバックをセット
    /// </summary>
    /// <param name="call"></param>
    public void SetCallback(UnityAction<string> call)
    {
        onRangeExit = call;
    }

    /// <summary>
    /// 半径を取得
    /// </summary>
    /// <returns></returns>
    public float GetRadius()
    {
        return sphereCollider.radius;
    }

    /// <summary>
    /// コライダーの範囲から抜けた瞬間
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Kage")
        {
            // コールバック関数を呼ぶ
            onRangeExit?.Invoke(other.transform.parent.parent.tag);
        }
    }
}
