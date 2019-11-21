using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// プレイヤー座標移動クラス
/// </summary>
public class PlayerMoveController : MonoBehaviour
{
    /// <summary>
    /// 移動方向
    /// </summary>
    public enum MoveDirection
    {
        FORWARD,
        BACK,
        LEFT,
        RIGHT,
    }

    /// <summary>
    /// 移動速度上限のタイプ
    /// </summary>
    public enum SpeedLimitType
    {
        WALK,
        DASH,
        SQUAT,
        STEALTH,
        BREATHLESSNESS,
        BAREFOOT,
    }

    /// <summary>
    /// レイのタイプ
    /// </summary>
    enum RayType
    {
        MOVEDIRECTION,
        DIAGONALDIRECTION,
    }

    [SerializeField]
    Rigidbody rigid = default;                  // リジットボディ
    [SerializeField]
    CapsuleCollider collider = default;         // コライダー
    [SerializeField]
    Animator playerAnim = default;              // アニメーター

    [SerializeField]
    PlayerObjectDamageController objectDamageController = default;  // オブジェクトダメージクラス
    [SerializeField]
    PlayerStateController stateController = default;                // ステート管理クラス

    [SerializeField]
    float forwardSpeed = 2f;                    // 前移動時のスピード
    [SerializeField]
    float sideSpeed = 2f;                       // 横移動時のスピード
    [SerializeField]
    float backSpeed = 1.5f;                     // 後ろ移動時のスピード
    [SerializeField]
    float speedMagnification = 10;              // 移動速度の倍率

    [SerializeField, Range(0, 1)]
    float barefootSpeedReduction = 0.85f;       // 裸足時の移動速度の減少割合
    [SerializeField, Range(0, 1)]
    float objectDamageSpeedReduction = 0.85f;   // オブジェクトダメージ時の移動速度の減少割合

    [SerializeField]
    float walkSpeedLimit = 0.75f;               // 歩き時の移動速度の限界
    [SerializeField]
    float dashSpeedLimit = 1.5f;                // ダッシュ時の移動速度の限界
    [SerializeField]
    float squatSpeedLimit = 0.25f;              // しゃがみ時の移動速度の限界
    [SerializeField]
    float stealthSpeedLimit = 0.25f;            // 忍び歩き時の移動速度の限界
    [SerializeField]
    float breathlessnessSpeedLimit = 0.25f;     // 息切れ時の移動速度の限界

    [SerializeField, Range(0,180)]
    float stepAngle = 60;                       // 段差の許容角度
    [SerializeField]
    float stepUpPower = 1.45f;                  // 段差に当たった時に加える上方向の力

    float moveTypeSpeedLimit = 0;               // 移動タイプによる移動速度の限界
    float dirTypeSpeedLimit = 0;                // 移動方向による移動速度の限界
    Vector3 moveSpeed = Vector3.zero;           // 移動速度
    bool isAnimPosition = false;                // アニメーションに座標移動をまかせるかどうか
    bool isAnimRotation = false;                // アニメーションに回転をまかせるかどうか

    /// <summary>
    /// アニメーション中の移動方法
    /// </summary>
    public void OnAnimatorMove()
    {
        // 各フラグが立ったら
        if (isAnimPosition)
        {
            // 座標移動をanimatorに任せる
            transform.position = playerAnim.rootPosition;
        }
        if (isAnimRotation)
        {
            // 回転をanimatorに任せる
            transform.rotation = playerAnim.rootRotation;
        }
    }

    /// <summary>
    /// 移動
    /// </summary>
    public void Move()
    {
        IsRootMotion(false, false);

        //　移動速度計算
        SpeedCalculation();

        // 移動速度の限界を超えるまで力をくわえ続ける
        float walkSpeed = new Vector3(rigid.velocity.x, 0, rigid.velocity.z).magnitude;
        if (walkSpeed <= dirTypeSpeedLimit * moveTypeSpeedLimit)
        {
            rigid.AddForce(moveSpeed.normalized * speedMagnification);
        }
    }

    /// <summary>
    /// 移動速度の計算
    /// </summary>
    void SpeedCalculation()
    {
        // 計算前の初期化
        moveSpeed = Vector3.zero;

        // 前移動の移動量計算
        if (Input.GetKey(KeyCode.W))
        {
            moveSpeed += transform.forward;
            dirTypeSpeedLimit = forwardSpeed;
        }
        // 後ろ移動
        else if (Input.GetKey(KeyCode.S))
        {
            moveSpeed -= transform.forward;
            dirTypeSpeedLimit = backSpeed;
        }

        // 左移動
        if (Input.GetKey(KeyCode.A))
        {
            moveSpeed -= Vector3.Cross(Vector3.up, transform.forward);
            dirTypeSpeedLimit = sideSpeed;
        }
        // 右移動
        else if (Input.GetKey(KeyCode.D))
        {
            moveSpeed += Vector3.Cross(Vector3.up, transform.forward);
            dirTypeSpeedLimit = sideSpeed;
        }

        // 段差に当たったら上方向に力を加え登らせる
        if (!DirectionRay(RayType.DIAGONALDIRECTION) && DirectionRay(RayType.MOVEDIRECTION))
        {
            moveSpeed += Vector3.up * stepUpPower;
        }
    }

    /// <summary>
    /// 移動方向のRayTypeに対応した向きのRaycast
    /// </summary>
    /// <returns>オブジェクトに当たっているかどうか</returns>
    bool DirectionRay(RayType type)
    {
        // レイのスタート位置
        Vector3 start = Vector3.zero;

        // レイの向き
        Vector3 dir = Vector3.zero;

        // レイの距離
        float distance = collider.radius * 2;

        // レイヤーマスク(プレイヤーからレイが伸びているので除外)
        int layerMask = (1 << LayerMask.NameToLayer("Player"));
        layerMask = ~layerMask;

        // レイのタイプによって向き変更
        switch (type)
        {
            case RayType.MOVEDIRECTION:
                // NOTE:k.oishi startの高さに0.01f足しているのは斜めのレイと始点を被らせないようにするため(始点がかぶると反応しなくなる)
                start = new Vector3(transform.position.x, transform.position.y - (collider.height / (2 + 0.01f)), transform.position.z);
                dir = new Vector3(moveSpeed.x, 0, moveSpeed.z); break;
            case RayType.DIAGONALDIRECTION:
                start = new Vector3(transform.position.x, transform.position.y - (collider.height / 2), transform.position.z);
                dir = new Vector3(moveSpeed.normalized.x, stepAngle / 100, moveSpeed.normalized.z); break;
        }

        // レイ作成
        Ray ray = new Ray(start, dir);
        RaycastHit hit = default;

        // デバック用ライン
        Debug.DrawLine(start, start + (dir * distance), Color.red);

        // レイに当たったらtrue、外れていたらfalse
        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 移動速度上限の変更
    /// </summary>
    /// <param name="type">移動タイプ</param>
    public void ChangeSpeedLimit(SpeedLimitType type)
    {
        switch (type)
        {
            case SpeedLimitType.WALK: moveTypeSpeedLimit = walkSpeedLimit; break;
            case SpeedLimitType.DASH: moveTypeSpeedLimit = dashSpeedLimit; break;
            case SpeedLimitType.SQUAT: moveTypeSpeedLimit = squatSpeedLimit; break;
            case SpeedLimitType.STEALTH: moveTypeSpeedLimit = stealthSpeedLimit; break;
            case SpeedLimitType.BREATHLESSNESS: moveTypeSpeedLimit = breathlessnessSpeedLimit; break;
        }

        // 裸足
        if (!stateController.IsShoes)
        {
            moveTypeSpeedLimit = moveTypeSpeedLimit * barefootSpeedReduction;
        }
        // ダメージ所持状態
        if(objectDamageController.IsObjectDamage)
        {
            moveTypeSpeedLimit = moveTypeSpeedLimit * objectDamageSpeedReduction;
        }
    }

    /// <summary>
    /// RootMotionに移動、回転を任せるかどうか
    /// </summary>
    /// <param name="isPosition">移動を任せるかどうか</param>
    /// <param name="isRotation">回転を任せるかどうか</param>
    public void IsRootMotion(bool isPosition, bool isRotation)
    {
        if (isPosition)
        {
            rigid.velocity = Vector3.zero;
            isAnimPosition = true;
        }
        else
        {
            isAnimPosition = false;
        }

        if (isRotation)
        {
            isAnimRotation = true;
        }
        else
        {
            isAnimRotation = false;
        }
    }
}