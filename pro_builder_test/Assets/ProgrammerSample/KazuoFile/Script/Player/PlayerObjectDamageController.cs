using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトのダメージ管理クラス
/// </summary>
public class PlayerObjectDamageController : MonoBehaviour
{
    /// <summary>
    /// ダメージのタイプ
    /// </summary>
    public enum DamageType
    {
        WALK,
        DASH,
        STEALTH,
    }
    [SerializeField]
    PlayerStateController stateController = default;    // ステート管理クラスwww

    [SerializeField]
    float walkDamage = 1;           // 歩き時のダメージ
    [SerializeField]
    float dashDamage = 2;           // ダッシュ時のダメージ
    [SerializeField]
    float stealthDamage = 0.5f;     // 忍び歩き時のダメージ
    [SerializeField]
    float recoveryCount = 5;        // 回復するまでの回数

    public bool IsObjectDamage { get; private set; } = false;    // ダメージオブジェクトにふれているかどうか
    public float NowObjectDamage { get; private set; } = 0;      // 今食らっているオブジェクトダメージ

    /// <summary>
    /// 障害物に当たっている間
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionStay(Collision collision)
    {
        // ダメージオブジェクト触れているかどうか
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Damage" && !stateController.IsShoes)
        {
            IsObjectDamage = true;
            NowObjectDamage = 100;
        }
    }

    // NOTE: k.oishi ダメージを追っている状態で動いた時に深刻度が増す用の関数です。(実装するかどうか)
    ///// <summary>
    ///// オブジェクトダメージの回復
    ///// </summary>
    //public void ObjectDamage(DamageType type)
    //{
    //    if (isObjectDamage)
    //    {
    //        switch (type)
    //        {
    //            case DamageType.WALK: NowObjectDamage += walkDamage; break;
    //            case DamageType.DASH: NowObjectDamage += dashDamage; break;
    //            case DamageType.STEALTH: NowObjectDamage += stealthDamage; break;
    //        }
    //    }
    //}

    /// <summary>
    /// 深呼吸回復
    /// </summary>
    public void RecoveryDeepBreath()
    {
        // 深呼吸回数をカウントし、回数が上限を超えたら回復
        NowObjectDamage--;
        if (NowObjectDamage <= 0)
        {
            NowObjectDamage = 0;
            IsObjectDamage = false;
        }
    }
}
