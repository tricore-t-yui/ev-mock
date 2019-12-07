using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveType = PlayerStateController.ActionStateType;

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
    PlayerStateController stateController = default;            // ステート管理クラス
    [SerializeField]
    SoundAreaSpawner soundArea = default;                       // 音管理クラス

    [SerializeField]
    float damageAmount = 0.05f;                                 // ダメージ量

    [SerializeField]
    float deepBreathRecovery = 0.3f;                            // 深呼吸時の回復量

    public bool IsDeepBreath { get; private set; } = false;     // 深呼吸強制フラグ
    public bool IsDamage { get; private set; } = false;         // ダメージオブジェクトにふれているかどうか
    public float NowDamage { get; private set; } = 0;           // 今食らっているオブジェクトダメージ

    /// <summary>
    /// 障害物に当たっている間
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        // ダメージオブジェクト触れているかどうか
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Damage")
        {
            if (!stateController.IsShoes)
            {
                IsDamage = true;
                NowDamage += 50;
            }

            soundArea.AddSoundLevel(SoundAreaSpawner.ActionSoundType.DAMAGEOBJECT);
        }
    }

    /// <summary>
    /// 各ステートに合わせた処理
    /// </summary>
    public void StateUpdate(MoveType type)
    {
        if (IsDamage)
        {
            // 各ステートに合わせた処理を実行
            switch (type)
            {
                case MoveType.WALK: NowDamage += damageAmount; break;
                case MoveType.DASH: NowDamage += damageAmount; break;
                case MoveType.STEALTHMOVE: NowDamage += damageAmount; break;
                default: break;
            }

            if (NowDamage >= 100)
            {
                IsDeepBreath = true;
            }
        }

        // 値補正
        NowDamage = Mathf.Clamp(NowDamage, 0, 100);
    }

    /// <summary>
    /// 深呼吸回復
    /// </summary>
    public void DeepBreathRecovery()
    {
        // 深呼吸回数をカウントし、回数が上限を超えたら回復
        NowDamage -= deepBreathRecovery;
        if (NowDamage <= 0)
        {
            NowDamage = 0;
            IsDeepBreath = false;
            IsDamage = false;
        }
    }
}
