using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージリアクションクラス
/// </summary>
public class PlayerDamageController : MonoBehaviour
{
    [SerializeField]
    Animator playerAnim = default;                          // アニメーター
    [SerializeField]
    Rigidbody rigidbody = default;                          // リジットボディ
    [SerializeField]
    PlayerHealthController healthController = default;      // HP管理クラス
    [SerializeField]
    InteractFunction interactController = default;          // インタラクト用関数クラス

    /// <summary>
    /// 障害物に当たったら
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        EndBlowAway();
    }

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // 初期化
        interactController.CommonInit();
    }

    /// <summary>
    /// 敵座標の登録
    /// </summary>
    /// <param name="enemyPos"></param>
    public void SetInfo(Vector3 enemyPos)
    {
        // 吹き飛ばしてアニメーション開始
        rigidbody.AddForce((enemyPos - transform.position),ForceMode.Impulse);
        playerAnim.SetTrigger("Damage");

        // 処理開始
        enabled = true;
    }

    /// <summary>
    /// 各アクションの終了
    /// </summary>
    /// NOTE:k.oishi アニメーションイベント用関数
    public void EndDoorAction()
    {
        // 閉じられていたら終了処理
        if (playerAnim.GetBool("DamageEnd"))
        {
            playerAnim.SetBool("DamageEnd", false);
            interactController.CommonEndAction();
            enabled = false;
        }
    }

    /// <summary>
    /// 吹き飛び終了
    /// </summary>
    public void EndBlowAway()
    {
        // 死んでしまったなら死亡アニメーション
        if(healthController.IsDeath)
        {
            playerAnim.SetBool("Death",true);
        }
        // 生きているなら立ち上がる
        else
        {
            playerAnim.SetTrigger("StandUp");
        }
    }
}
