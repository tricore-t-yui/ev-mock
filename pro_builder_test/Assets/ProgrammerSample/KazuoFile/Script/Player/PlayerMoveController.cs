using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using KeyType = KeyController.KeyType;
using StickType = KeyController.StickType;

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
        NOTMOVE,
        WALK,
        DASH,
        SQUAT,
        BREATHHOLD,
        BAREFOOT,
        OBJECTDAMAGE,
        STAMINA,
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
    Rigidbody playerRigidbody = default;                          // リジットボディ
    [SerializeField]
    CapsuleCollider playerCollider = default;                      // コライダー
    [SerializeField]
    Animator playerAnim = default;                                // アニメーター
    [SerializeField]
    PlayerMoveData moveData = default;                            // プレイヤーデータのスクリプタブルオブジェクト

    [SerializeField]
    playerStaminaController staminaController = default;            // スタミナクラス
    [SerializeField]
    PlayerObjectDamageController objectDamageController = default;   // オブジェクトダメージクラス
    [SerializeField]
    PlayerStateController stateController = default;                // ステート管理クラス
    [SerializeField]
    KeyController keyController = default;                         // キー操作クラス
    [SerializeField]
    SoundAreaSpawner soundAreaSpawner = default;                    // 音領域生成クラス
    [SerializeField]
    SoundSpawner soundSpawner = default;                           // 音生成クラス

    float moveTypeSpeedLimit = 0;               // 移動タイプによる移動速度の限界
    float dirTypeSpeedLimit = 0;                // 移動方向による移動速度の限界
    float stickSpeedLimit = 0;                  // スティックの入力加減による移動速度の限界
    Vector3 moveSpeed = Vector3.zero;           // 移動速度
    bool isAnimPosition = false;                // アニメーションに座標移動をまかせるかどうか
    bool isAnimRotation = false;                // アニメーションに回転をまかせるかどうか
    Vector3 initPos = default;                  // 初期位置
    Quaternion initRota = default;              // 初期の向き

    public bool IsDuct { get; private set; } = false;   // ダクトないかどうか

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
    /// トリガーに触れたとき
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("SquatObject"))
        {
            IsDuct = true;
        }
    }

    /// <summary>
    /// トリガーから離れたとき
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SquatObject"))
        {
            IsDuct = false;
        }
    }

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        initPos = transform.position;
        initRota = transform.rotation;
    }

    /// <summary>
    /// 移動
    /// </summary>
    public void Move()
    {
        IsRootMotion(false, false);

        //　移動速度計算
        SpeedCalculation();
    }

    /// <summary>
    /// 移動速度の計算
    /// </summary>
    void SpeedCalculation()
    {
        // スティックの入力ベクトル取得
        Vector2 stick = new Vector2(keyController.GetStick(StickType.LEFTSTICK).x, keyController.GetStick(StickType.LEFTSTICK).y);

        // 入力ベクトルの長さを取得してノーマライズ
        float length = stick.magnitude;
        stick = stick.normalized;

        // 移動キーが押されていたら移動
        if (keyController.GetKey(KeyType.MOVE))
        {
            dirTypeSpeedLimit = ChangeDirTypeSpeedLimit(stick);
            stickSpeedLimit = ChangeStickSpeedLimit(length);
            moveSpeed = Vector3.Scale(transform.forward * stick.y + transform.right * stick.x, new Vector3(1, 0, 1)).normalized * stickSpeedLimit;
        }

        // 段差に当たったら上方向に力を加え登らせる
        if (DirectionRay(RayType.MOVEDIRECTION) && !DirectionRay(RayType.DIAGONALDIRECTION))
        {
            moveSpeed += Vector3.up * moveData.StepUpPower;
        }
        // 移動速度の限界を超えるまで力をくわえ続ける
        float walkSpeed = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z).magnitude;
        if (walkSpeed <= dirTypeSpeedLimit * moveTypeSpeedLimit * stickSpeedLimit)
        {
            playerRigidbody.AddForce(moveSpeed * moveData.SpeedMagnification);
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
        float distance = playerCollider.radius * 2f;

        // レイヤーマスク(プレイヤーからレイが伸びているので除外)
        int layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("SafetyArea");
        layerMask = ~layerMask;

        // レイのタイプによって向き変更
        switch (type)
        {
            case RayType.MOVEDIRECTION:
                // NOTE:k.oishi startの高さに0.01f足しているのは斜めのレイと始点を被らせないようにするため(始点がかぶると反応しなくなる)
                start = new Vector3(transform.position.x, transform.position.y - (playerCollider.height / (2 + 0.1f)), transform.position.z);
                dir = new Vector3(moveSpeed.x, 0, moveSpeed.z); break;
            case RayType.DIAGONALDIRECTION:
                start = new Vector3(transform.position.x, transform.position.y - (playerCollider.height / (2 + 0.2f)), transform.position.z);
                dir = new Vector3(moveSpeed.normalized.x, moveData.StepAngle / 100, moveSpeed.normalized.z); break;
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
    /// 移動タイプによって移動速度上限の変更
    /// </summary>
    /// <param name="type">移動タイプ</param>
    public void ChangeMoveTypeSpeedLimit(SpeedLimitType type)
    {
        switch (type)
        {
            case SpeedLimitType.NOTMOVE: moveTypeSpeedLimit = 0; break;
            case SpeedLimitType.WALK: moveTypeSpeedLimit = moveData.WalkSpeedLimit; break;
            case SpeedLimitType.DASH: moveTypeSpeedLimit = moveData.DashSpeedLimit; break;
            case SpeedLimitType.BREATHHOLD: moveTypeSpeedLimit = moveData.StealthSpeedLimit; break;
        }

        // 状態によって移動速度上限の変更
        ChangeStatusTypeSpeedLimit();
    }

    /// <summary>
    /// 状態によって移動速度上限の変更
    /// </summary>
    public void ChangeStatusTypeSpeedLimit()
    {
        if (stateController.IsSquat)
        {
            moveTypeSpeedLimit = moveTypeSpeedLimit * moveData.SquatSpeedLimit;
        }
        // 裸足
        if (!stateController.IsShoes)
        {
            moveTypeSpeedLimit = moveTypeSpeedLimit * moveData.BarefootSpeedReduction;
        }
        // ダメージ所持状態
        if (objectDamageController.IsDamage)
        {
            moveTypeSpeedLimit = moveTypeSpeedLimit * moveData.ObjectDamageSpeedReduction;
        }
        // スタミナ切れ時
        if (staminaController.IsDisappear)
        {
            moveTypeSpeedLimit = moveTypeSpeedLimit * moveData.StaminaSpeedReduction;
        }
    }

    /// <summary>
    /// 入力ベクトルの長さによってスピード上限変更
    /// </summary>
    /// <param name="length">ベクトルの長さ</param>
    float ChangeStickSpeedLimit(float length)
    {
        if (length >= 1)
        {
            soundAreaSpawner.AddSoundLevel(SoundAreaSpawner.ActionSoundType.WALK);
            return 1;
        }
        else if (length > 0.5f)
        {
            soundAreaSpawner.AddSoundLevel(SoundAreaSpawner.ActionSoundType.STEALTH);
            return 0.5f;
        }
        else if (length > 0.1f)
        {
            soundAreaSpawner.AddSoundLevel(SoundAreaSpawner.ActionSoundType.STEALTH);
            return 0.2f;
        }

        return 0;
    }

    /// <summary>
    /// 向きスピード上限変更
    /// </summary>
    /// <param name="vec">向きベクトル</param>
    float ChangeDirTypeSpeedLimit(Vector2 vec)
    {
        float speed = 0;

        if(vec.x != 0)
        {
            speed = moveData.SideSpeed;
        }

        if (vec.y > 0)
        {
            speed = moveData.ForwardSpeed;
        }
        else if (vec.y < 0)
        {
            speed = moveData.BackSpeed;
        }

        return speed;
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
            playerRigidbody.velocity = Vector3.zero;
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

    /// <summary>
    /// 位置、回転のリセット
    /// </summary>
    public void ResetPos()
    {
        transform.position = initPos;
        transform.rotation = initRota;
    }

    /// <summary>
    /// 歩いているときの足音
    /// </summary>
    /// NOTE:アニメーション用関数
    public void PlayWalkSound()
    {
        soundSpawner.Play(SoundSpawner.SoundType.Walk);
        if(objectDamageController.IsTouch)
        {
            soundSpawner.Play(SoundSpawner.SoundType.DamageObject);
        }
    }

    /// <summary>
    /// 走っているときの足音
    /// </summary>
    /// NOTE:アニメーション用関数
    public void PlayDashSound()
    {
        soundSpawner.Play(SoundSpawner.SoundType.Dash);
    }
}