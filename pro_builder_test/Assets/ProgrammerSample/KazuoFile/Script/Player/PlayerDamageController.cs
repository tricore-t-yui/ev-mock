using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationType = PlayerAnimationContoller.AnimationType;

/// <summary>
/// ダメージリアクションクラス
/// </summary>
public class PlayerDamageController : MonoBehaviour
{
    [SerializeField]
    PlayerAnimationContoller animationContoller = default;  // アニメーション管理クラス
    [SerializeField]
    Rigidbody rigidbody = default;                          // リジットボディ
    [SerializeField]
    PlayerHealthController healthController = default;      // 体力管理クラス
    [SerializeField]
    InteractFunction interactController = default;          // インタラクト用関数クラス

    [SerializeField]
    float objectDamage = 0.1f;                              // オブジェクトでのダメージ量
    [SerializeField]
    float invincibleSecond = 2;                             // ダメージ処理後の無敵時間

    bool isInvincible = false;                              // 無敵時間かどうか
    bool isDamageObjHit = false;                            // ダメージオブジェクトにふれているかどうか
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

        // ダメージオブジェクト触れているかどうか
        if (LayerMask.LayerToName(collision.gameObject.layer) == "DamageObj")
        {
            isDamageObjHit = true;
        }
        else
        {
            isDamageObjHit = false;
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
            animationContoller.AnimStart(AnimationType.DAMAGE);

            // 処理開始
            enabled = true;
        }
    }

    /// <summary>
    /// ダメージオブジェクトに当たった時のダメージ処理
    /// </summary>
    public void HitDamageObject()
    {
        // ダメージオブジェクトに触れていたら
        if(isDamageObjHit)
        {
            // ダメージを食らう
            healthController.Damage(objectDamage);
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
            animationContoller.AnimStart(AnimationType.DEATH);
        }
        // 生きているなら立ち上がる
        else
        {
            animationContoller.AnimStop(AnimationType.DEATH);
        }
    }

    /// <summary>
    /// 各アクションの終了
    /// </summary>
    public void EndDamageAction()
    {
        // アニメーションが再生され終わったら終了処理
        if (animationContoller.EndAnimation(PlayerAnimationContoller.EndAnimationType.DAMAGE) && !healthController.IsDeath)
        {
            animationContoller.SetEndAnimationFlag(PlayerAnimationContoller.EndAnimationType.DAMAGE);
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
