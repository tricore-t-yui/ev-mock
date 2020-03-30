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
    [SerializeField]
    ParticleSystem effect = default;                        // エフェクト

    public bool IsEnd { get; private set; } = false;    // 終了フラグ
    public bool IsHit { get; private set; } = false;    // ヒットフラグ

    public bool IsStop { get; private set; }

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
        if (other.gameObject.tag == "Player")
        {
            IsHit = true;
        }
        if (other.gameObject.tag == "Oni")
        {
            effect.Stop();
            IsStop = true;
        }
    }

    /// <summary>
    /// トリガーから離れたら
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            IsHit = false;
        }
        if (other.gameObject.tag == "Oni")
        {
            effect.Play();
            IsStop = false;
        }
    }

    /// <summary>
    /// 罠作動
    /// </summary>
    public void TrapOperate()
    {
        effect.Stop();
        tunAnim.SetTrigger("TrapOperate");
    }

    public void Stop()
    {
        effect.Stop();
    }

    /// <summary>
    /// リセット
    /// </summary>
    public void ResetTrapTun()
    {
        effect.Play();
        IsHit = false;
    }
}
