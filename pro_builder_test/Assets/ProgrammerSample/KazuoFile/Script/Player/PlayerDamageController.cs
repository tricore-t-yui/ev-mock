using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationType = PlayerAnimationContoller.AnimationType;

/// <summary>
/// ダメージリアクションクラス
/// </summary>
public class PlayerDamageController : MonoBehaviour
{
    /// <summary>
    /// ダメージのタイプ
    /// </summary>
    public enum DamageType
    {
        NORMAL,     // 通常のダメージ
        HIDEBED,    // ベッドから引き摺り出される時のダメージ
        HIDELOCKER, // ロッカーから引き摺り出される時のダメージ
    }

    [SerializeField]
    Rigidbody playerRigidbody = default;                    // リジットボディ
    [SerializeField]
    PlayerAnimationContoller animationContoller = default;  // アニメーション管理クラス
    [SerializeField]
    PlayerHealthController healthController = default;      // 体力管理クラス
    [SerializeField]
    InteractFunction interactController = default;          // インタラクト用関数クラス

    [SerializeField]
    float invincibleSecond = 2;                             // ダメージ処理後の無敵時間

    public DamageType Type { get; private set; } = default;
    public bool IsInvincible { get; private set; } = false; // 無敵時間かどうか
    public bool IsObjHit { get; private set; } = false;     // オブジェクトに当たったらどうか

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // 初期化
        interactController.CommonInit();
        IsDeath();
    }

    /// <summary>
    /// 敵座標の登録
    /// </summary>
    /// <param name="enemyPos"></param>
    public void SetInfo(Vector3 enemyPos, float damage, DamageType type)
    {
        // ダメージを食らう
        healthController.Damage(damage);
        Type = type;

        // ダメージによって吹き飛ばしてアニメーション開始
        switch (Type)
        {
            case DamageType.NORMAL:
                playerRigidbody.AddForce((enemyPos - transform.position).normalized, ForceMode.Impulse);
                animationContoller.AnimStart(AnimationType.DAMAGE);break;
            case DamageType.HIDEBED:
                playerRigidbody.AddForce((enemyPos - transform.position).normalized * -10, ForceMode.Impulse);
                animationContoller.AnimStart(AnimationType.DRAGOUT); break;
            case DamageType.HIDELOCKER:
                animationContoller.AnimStart(AnimationType.DRAGOUT); break;
        }

        if(Type != DamageType.HIDEBED)
        {
            // アニメーションに入る前にプレイヤーのrotationを初期化
            transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        }

        // 処理開始
        enabled = true;
    }

    /// <summary>
    /// 死んでいないかどうか
    /// </summary>
    public void IsDeath()
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
