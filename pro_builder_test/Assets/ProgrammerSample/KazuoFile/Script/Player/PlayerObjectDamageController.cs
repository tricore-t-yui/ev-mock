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
    PlayerStateController stateController = default;            // ステート管理クラス
    [SerializeField]
    SoundAreaSpawner soundArea = default;                       // 音管理クラス

    public bool IsObjectDamage { get; private set; } = false;   // ダメージオブジェクトにふれているかどうか
    public float NowObjectDamage { get; private set; } = 0;     // 今食らっているオブジェクトダメージ

    /// <summary>
    /// 障害物に当たっている間
    /// </summary>
    void OnCollisionStay(Collision collision)
    {
        // ダメージオブジェクト触れているかどうか
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Damage")
        {
            if (!stateController.IsShoes)
            {
                IsObjectDamage = true;
                NowObjectDamage = 100;
            }

            soundArea.AddSoundLevel(SoundAreaSpawner.ActionSoundType.DAMAGEOBJECT);
        }
    }

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
