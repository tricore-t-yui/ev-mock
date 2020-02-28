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
    PlayerStatusData playerData = default;                      // プレイヤーのデータのスクリプタブルオブジェクト

    [SerializeField]
    bool isDebug = false;

    public bool IsDeepBreath { get; private set; } = false;     // 深呼吸強制フラグ
    public bool IsTouch { get; private set; } = false;         // ダメージオブジェクトにふれているかどうか
    public bool IsDamage { get; private set; } = false;         // ダメージを負ってしまっているかどうか
    public float NowDamage { get; private set; } = 0;           // 今食らっているオブジェクトダメージ

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        IsTouch = false;
        IsDeepBreath = false;
        IsDamage = false;
        NowDamage = 0;
    }

    /// <summary>
    /// 障害物に当たっている瞬間
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        // ダメージオブジェクト触れているかどうか
        if (LayerMask.LayerToName(other.gameObject.layer) == "Damage")
        {
            if (!stateController.IsShoes)
            {
                IsDamage = true;
                NowDamage += 50;
            }

            IsTouch = true;
        }
    }

    /// <summary>
    /// 障害物に当たっている間
    /// </summary>
    void OnTriggerStay(Collider other)
    {
        // ダメージオブジェクト触れているかどうか
        if (LayerMask.LayerToName(other.gameObject.layer) == "Damage" && !soundArea.IsDamageObjectSound)
        {
            if (stateController.IsShoes)
            {
                soundArea.SetSoundLevel(SoundAreaSpawner.ActionSoundType.SHOESDAMAGEOBJECT);
            }
            else
            {
                soundArea.SetSoundLevel(SoundAreaSpawner.ActionSoundType.BAREFOOTDAMAGEOBJECT);
            }

            soundArea.SetIsDamageObjectSound(true); 
        }
    }

    /// <summary>
    /// トリガーから離れたとき
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Damage")
        {
            IsTouch = false;
        }
    }

    /// <summary>
    /// 各ステートに合わせた処理
    /// </summary>
    public void StateUpdate(MoveType type)
    {
        if (IsDamage && !isDebug)
        {
            // 各ステートに合わせた処理を実行
            switch (type)
            {
                case MoveType.WALK: NowDamage += playerData.ObjectDamageAmount; break;
                case MoveType.DASH: NowDamage += playerData.ObjectDamageAmount; break;
                case MoveType.BREATHHOLDMOVE: NowDamage += playerData.ObjectDamageAmount; break;
                default: break;
            }

            // 100を超えたら深呼吸を強制
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
        NowDamage -= playerData.ObjectDamagedeepBreathRecovery;
        if (NowDamage <= 0)
        {
            NowDamage = 0;
            IsDeepBreath = false;
            IsDamage = false;
        }
    }

    /// <summary>
    /// リセット
    /// </summary>
    public void ResetDamage()
    {
        IsDeepBreath = false;
        IsDamage = false;
        NowDamage = 0;
    }
}
