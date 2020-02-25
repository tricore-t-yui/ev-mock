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
        DUCT,       // ダクト
        DEATH,      // トドメ
    }

    [SerializeField]
    Rigidbody playerRigidbody = default;                        // リジットボディ
    [SerializeField]
    PlayerAnimationContoller animationContoller = default;      // アニメーション管理クラス
    [SerializeField]
    PlayerHealthController healthController = default;          // 体力管理クラス
    [SerializeField]
    PlayerHideController playerHideController = default;        // 隠れるアクションクラス

    [SerializeField]
    PlayerStatusData playerData = default;                      // プレイヤーのデータのスクリプタブルオブジェクト

    HideObjectController hideObjectController = null;           // 隠れているオブジェクト

    public DamageType Type { get; private set; } = default;     // ダメージタイプ
    public Transform EnemyPos { get; private set; } = default;  // 敵のTransform
    public bool IsInvincible { get; private set; } = false;     // 無敵時間かどうか
    public bool IsObjHit { get; private set; } = false;         // オブジェクトに当たったらどうか
    public bool IsFinishBlow { get; private set; } = false;     // トドメの開始フラグ

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // NOTE: k.oishi リセットが入るため今のところ死んだらはなし
        //IsDeath();
    }

    /// <summary>
    /// 敵座標の登録
    /// </summary>
    public void SetInfo(Transform enemyPos, float damage, DamageType type)
    {
        // ダメージを食らう
        healthController.Damage(damage);
        Type = type;
        EnemyPos = enemyPos;
        transform.LookAt(new Vector3(EnemyPos.position.x, transform.position.y, EnemyPos.position.z));

        // ダメージによって吹き飛ばしてアニメーション開始
        switch (Type)
        {
            case DamageType.NORMAL:
                animationContoller.AnimStart(AnimationType.DAMAGE); break;
            case DamageType.HIDEBED:
                animationContoller.AnimStart(AnimationType.HIDEDRAGOUT);
                hideObjectController = playerHideController.HideObj.GetComponent<HideObjectController>();
                hideObjectController.SetActiveCollider(false);
                playerRigidbody.AddForce((new Vector3(EnemyPos.position.x,transform.position.y,EnemyPos.position.z) - transform.position).normalized * 8, ForceMode.Impulse); break;
            case DamageType.HIDELOCKER:
                animationContoller.AnimStart(AnimationType.HIDEDRAGOUT); break;
            case DamageType.DUCT:
                animationContoller.AnimStart(AnimationType.DUCTDRAGOUT);
                playerRigidbody.AddForce((new Vector3(EnemyPos.position.x, transform.position.y, EnemyPos.position.z) - transform.position).normalized * 8, ForceMode.Impulse); break;

        }

        // 処理開始
        enabled = true;
    }

    /// <summary>
    /// 今のダメージで死んでしまうかどうか
    /// </summary>
    public void IsDeath()
    {
        // 死んでしまうならトドメアニメーション
        if (healthController.IsDeath)
        {
            // NOTE: k.oishi リセットが入るため今のところトドメのアニメーションは無し
            //Type = DamageType.DEATH;
            //animationContoller.AnimStart(AnimationType.DEATH);
        }
        // 生きているなら立ち上がる
        else
        {
            animationContoller.AnimStop(AnimationType.DEATH);
        }
    }

    /// <summary>
    /// トドメの開始
    /// </summary>
    public void StartFinishingBlow()
    {
        IsFinishBlow = true;
    }

    /// <summary>
    /// 各アクションの終了
    /// </summary>
    public void EndDamageAction()
    {
        // アニメーションが再生され終わったら終了処理
        // NOTE: k.oishi リセットが入るため今のところ死んだらはなし
        if (animationContoller.EndAnimation(PlayerAnimationContoller.EndAnimationType.DAMAGE) )//&& !healthController.IsDeath)
        {
            animationContoller.SetEndAnimationFlag(PlayerAnimationContoller.EndAnimationType.DAMAGE);
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

            // 無敵時間開始
            IsInvincible = true;
            StartCoroutine(InvincibleCount());

            if(Type == DamageType.HIDEBED)
            hideObjectController?.SetActiveCollider(true);

            // 処理終了
            enabled = false;
        }
    }

    /// <summary>
    /// 無敵時間のコルーチン
    /// </summary>
    IEnumerator InvincibleCount()
    {
        yield return new WaitForSeconds(playerData.InvincibleSecond);
        IsInvincible = false;
    }
}