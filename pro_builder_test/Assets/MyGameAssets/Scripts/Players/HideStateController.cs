using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 隠れる状態管理クラス
/// </summary>
public class HideStateController : MonoBehaviour
{
    /// <summary>
    /// 心音
    /// </summary>
    public enum HeartSoundType
    {
        NORMAL,     // 通常
        MEDIUM,     // 中
        LARGE,      // 大
    }

    [SerializeField]
    CapsuleCollider playerCollider = default;               // プレイヤーのコライダー
    [SerializeField]
    PlayerBreathController breathController = default;      // 息管理クラス
    [SerializeField]
    HeartSe heartSe = default;

    bool isSafetyArea = false;                              // 安全地帯内にいないかどうか
    bool isLookEnemy = false;                               // 敵が見えているかどうか
    public HeartSoundType HeartSound { get; private set; } = HeartSoundType.NORMAL;      // 心音

    List<int> enemyLayerId = new List<int>();
    List<int> obstacleLayerId = new List<int>();

    //private void Awake()
    //{
    //    enemyLayerId.Add(LayerMask.NameToLayer("ShadowHuman"));
    //    enemyLayerId.Add(LayerMask.NameToLayer("Tsun"));
    //    enemyLayerId.Add(LayerMask.NameToLayer("Oni"));
    //    obstacleLayerId.Add(LayerMask.NameToLayer("Door"));
    //    obstacleLayerId.Add(LayerMask.NameToLayer("Bed"));
    //    obstacleLayerId.Add(LayerMask.NameToLayer("Locker"));
    //    obstacleLayerId.Add(LayerMask.NameToLayer("Stage"));
    //    obstacleLayerId.Add(LayerMask.NameToLayer("Duct"));
    //}

    ///// <summary>
    ///// 起動処理
    ///// </summary>
    //void OnEnable()
    //{
    //    isSafetyArea = false;
    //    isLookEnemy = false;
    //}

    //private void OnDisable()
    //{
    //    // 心音の変更に合わせた処理
    //    HeartSound = HeartSoundType.NORMAL;
    //    breathController.ChangeHideDecrement(HeartSound);
    //    heartSe.ChangeHeartSound(HeartSound);
    //}

    /// <summary>
    /// 更新処理
    /// </summary>
    //void Update()
    //{
    //    // スフィアキャストをして周囲に敵がいないか判定
    //    Vector3 playerSpine = new Vector3(transform.position.x, transform.position.y + (playerCollider.height / 2), transform.position.z);
    //    var ray = new Ray(playerSpine, transform.forward);
    //    var hits = Physics.SphereCastAll(ray, 10.0f, 0.001f); // 最大範囲10mでとりあえず
    //    isSafetyArea = true;
    //    foreach (var item in hits)
    //    {
    //        if (enemyLayerId.Contains(item.collider.gameObject.layer))
    //        {
    //            // 警戒状態の敵が近くにいたら安全でないと判断
    //            var hitEnemy = item.collider.GetComponent<EnemyBase>();
    //            if (hitEnemy && hitEnemy.currentState != EnemyParameter.StateType.Normal)
    //            {
    //                isSafetyArea = false;
    //                break;
    //            }
    //        }
    //    }

    //    // レイキャストで視界内にエネミーがいるかどうか判定（範囲は敵が持つ）
    //    // 最大範囲10mでとりあえず
    //    ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
    //    hits = Physics.RaycastAll(ray, 10.0f); // 最大範囲10mでとりあえず
    //    float nearestEnemy = float.MaxValue;
    //    float nearestStage = float.MaxValue;
    //    foreach (var item in hits)
    //    {
    //        // 障害物
    //        if (obstacleLayerId.Contains(item.collider.gameObject.layer))
    //        {
    //            if (item.distance < nearestStage)
    //            {
    //                nearestStage = item.distance;
    //            }
    //        }
    //        // 敵
    //        else if (enemyLayerId.Contains(item.collider.gameObject.layer))
    //        {
    //            if (item.distance < nearestEnemy)
    //            {
    //                nearestEnemy = item.distance;
    //            }
    //        }
    //    }
    //    isLookEnemy = false;
    //    if (nearestEnemy < nearestStage)
    //    {
    //        isLookEnemy = true;
    //    }

    //    // 心音の変更
    //    ChangeHeartSound();
    //}

    /// <summary>
    /// 敵が見えているかどうか
    /// </summary>
    public void VisibleEnemy(Transform enemy, float height)
    {
        //if (VisibleEnemyRay(enemy, height))
        //{
        //    visibleEnemy.Add(true);
        //}
        //else
        //{
        //    visibleEnemy.Add(false);
        //}
    }

    /// <summary>
    /// 心音の変更
    /// </summary>
    void ChangeHeartSound()
    {
        if (isSafetyArea)
        {
            // 安全地帯内
            HeartSound = HeartSoundType.NORMAL;

            // 安全地帯内敵が見えている状態
            if (isLookEnemy)
            {
                HeartSound = HeartSoundType.MEDIUM;
            }
        }
        else
        {
            // 安全地帯内に敵がおらず、敵が見えていない状態(消費小)
            HeartSound = HeartSoundType.MEDIUM;

            // 安全地帯内に敵がおらず、敵が見えている状態
            if (isLookEnemy)
            {
                // 安全地帯内に敵がいて姿を見ている状態(消費中)
                HeartSound = HeartSoundType.LARGE;
            }
        }

        // 心音の変更に合わせた処理
        //breathController.ChangeHideDecrement(HeartSound);
       // heartSe.ChangeHeartSound(HeartSound);
    }
}
