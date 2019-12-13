using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 罠ツンクラス
/// </summary>
public class TrapTunController : MonoBehaviour
{
    [SerializeField]
    Animator tunAnim = default;                         // アニメーター

    public bool IsEnd { get; private set; } = false;    // 終了フラグ
    public bool IsHit { get; private set; } = false;    // ヒットフラグ

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        IsHit = false;
    }

    /// <summary>
    /// トリガーが当たったら
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            IsHit = true;
        }
    }

    /// <summary>
    /// トリガーから離れたら
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            IsHit = false;
        }
    }

    /// <summary>
    /// 罠作動
    /// </summary>
    public void TrapOperate()
    {
        tunAnim.SetTrigger("TrapOperate");
    }
}
