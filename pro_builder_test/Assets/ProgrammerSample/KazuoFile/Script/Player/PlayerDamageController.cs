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
    float invincibleSecond = 2;                             // ダメージ処理後の無敵時間

    public bool IsInvincible { get; private set; } = false; // 無敵時間かどうか
    public bool IsObjHit { get; private set; } = false;     // オブジェクトに当たったらどうか

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // 初期化
        interactController.CommonInit();
        EndBlowAway();
    }

    /// <summary>
    /// 敵座標の登録
    /// </summary>
    /// <param name="enemyPos"></param>
    public void SetInfo(Vector3 enemyPos, float damage)
    {
        // ダメージを食らう
        healthController.Damage(damage);

        // 吹き飛ばしてアニメーション開始
        rigidbody.AddForce((enemyPos - transform.position).normalized * 5, ForceMode.Impulse);
        animationContoller.AnimStart(AnimationType.DAMAGE);

        // 処理開始
        enabled = true;
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
            IsInvincible = true;
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
        IsInvincible = false;
    }
}
