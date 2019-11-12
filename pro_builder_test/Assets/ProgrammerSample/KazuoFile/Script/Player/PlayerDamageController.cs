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
    PlayerHealthController healthController = default;      // 体力管理クラス
    [SerializeField]
    InteractFunction interactController = default;          // インタラクト用関数クラス

    [SerializeField]
    float invincibleSecond = 2;                             // ダメージ処理後の無敵時間

    bool isInvincible = false;                              // 無敵時間かどうか 
    public bool IsObjHit { get; private set; } = false;     // オブジェクトに当たったらどうか

    /// <summary>
    /// 障害物に当たったら
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        // ダメージ処理に入っているなら
        if (enabled)
        {
            // オブジェクトに当たったら倒れる
            EndBlowAway();
            IsObjHit = true;
        }
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
    public void SetInfo(Vector3 enemyPos, float damage)
    {
        // 無敵時間じゃなかったら処理開始
        if (!isInvincible)
        {
            // ダメージを食らう
            healthController.Damage(damage);

            // 吹き飛ばしてアニメーション開始
            rigidbody.AddForce((enemyPos - transform.position).normalized * 5, ForceMode.Impulse);
            playerAnim.SetTrigger("Damage");

            // 処理開始
            enabled = true;
        }
    }

    /// <summary>
    /// 吹き飛び終了
    /// </summary>
    public void EndBlowAway()
    {
        // 死んでしまったなら死亡アニメーション
        if (healthController.IsDeath)
        {
            playerAnim.SetBool("Death", true);
        }
        // 生きているなら立ち上がる
        else
        {
            playerAnim.SetBool("Death", false);
        }
    }

    /// <summary>
    /// 各アクションの終了
    /// </summary>
    public void EndDamageAction()
    {
        // 閉じられていたら終了処理
        if (playerAnim.GetBool("DamageEnd"))
        {
            playerAnim.SetBool("DamageEnd", false);
            interactController.CommonEndAction();

            // 無敵時間開始
            isInvincible = true;
            StartCoroutine(InvincibleCount());

            // 処理終了
            enabled = false;
        }
    }

    /// <summary>
    /// 無敵時間のコルーチン
    /// </summary>
    IEnumerator InvincibleCount()
    {
        yield return new WaitForSeconds(invincibleSecond);
        isInvincible = false;
    }
}
