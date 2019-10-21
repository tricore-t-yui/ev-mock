using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// プレイヤー動きクラス
/// </summary>
public class PlayerMover : MonoBehaviour
{
    /// <summary>
    /// 移動方向
    /// </summary>
    enum MoveDirection
    {
        FORWARD,
        BACK,
        LEFT,
        RIGHT,
        NOINPUT,
        LENGHT,
    }
    [SerializeField]
    Rigidbody rigid = default;                       // リジットボディ
    [SerializeField]
    CapsuleCollider collider = default;              // コライダー
    [SerializeField]
    PlayerMoveController playerController = default; // プレイヤーの状態クラス
    [SerializeField]
    PlayerBrethController brethController = default; // プレイヤーの息管理クラス

    [SerializeField]
    float forwardSpeed = 2f;                // 前移動時のスピード
    [SerializeField]
    float sideSpeed = 2f;                   // 横移動時のスピード
    [SerializeField]
    float backSpeed = 1.5f;                 // 後ろ移動時のスピード
    [SerializeField]
    float speedMagnification = 10;          // 移動速度の倍率
    [SerializeField]
    float jumpPower = 3f;                   // ジャンプの高さ
    [SerializeField]
    float dashSpeedLimit = 2f;              // ダッシュ時の移動速度の限界
    [SerializeField]
    float squatSpeedLimit = 0.5f;           // しゃがみ時の移動速度の限界
    [SerializeField]
    float stealthSpeedLimit = 0.5f;       // 忍び歩き時の移動速度の限界
    [SerializeField]
    float breathlessnessSpeedLimit = 0.5f;  // 息切れ時の移動速度の限界

    [SerializeField, Range(0,180)]
    float stepAngle = 60;                   // 横移動時のスピード
    [SerializeField]
    float stepUpPower = 1.45f;              // 段差に当たった時に加える上方向の力

    float speedLimit = 0;                   // 移動速度の限界
    Vector3 moveSpeed = Vector3.zero;       // 移動速度

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 隠れていたら動けない
        if (!playerController.IsHide)
        {
            // 移動キーが１つでも押されていたら
            if (IsDirectionKey())
            {
                // 座標移動
                Move();
            }

            // ジャンプが開始されたなら
            if (playerController.IsJumpStart())
            {
                // ジャンプ
                Jump();
            }
        }
    }

    /// <summary>
    /// 座標移動
    /// </summary>
    void Move()
    {
        //　移動速度計算
        SpeedCalculation();

        // 移動速度の上限の計算
        SpeedLimitCalculation();

        // 移動速度(歩く、ダッシュのみ)の限界を超えるまで力をくわえ続ける
        float walkSpeed = new Vector3(rigid.velocity.x, 0, rigid.velocity.z).magnitude;
        if (walkSpeed <= speedLimit)
        {
            rigid.AddForce(moveSpeed * speedMagnification);
        }
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    void Jump()
    {
        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    /// <summary>
    /// 移動速度の計算
    /// </summary>
    void SpeedCalculation()
    {
        // 計算前の初期化
        moveSpeed = Vector3.zero;

        // 前移動の移動量計算
        if (GetDirectionKey(MoveDirection.FORWARD))
        {
            moveSpeed += transform.forward;
            speedLimit = forwardSpeed;
        }
        // 後ろ移動
        else if (GetDirectionKey(MoveDirection.BACK))
        {
            moveSpeed -= transform.forward;
            speedLimit = backSpeed;
        }
        // 左移動
        if (GetDirectionKey(MoveDirection.LEFT))
        {
            moveSpeed -= Vector3.Cross(Vector3.up, transform.forward);
            speedLimit = sideSpeed;
        }
        // 右移動
        else if (GetDirectionKey(MoveDirection.RIGHT))
        {
            moveSpeed += Vector3.Cross(Vector3.up, transform.forward);
            speedLimit = sideSpeed;
        }

        // 段差に当たったら上方向に力を加え登らせる
        if(!DiagonalDirectionRay() && MoveDirectionRay())
        {
            moveSpeed += Vector3.up * stepUpPower;
        }
    }

    /// <summary>
    /// 移動速度の上限の計算
    /// </summary>
    void SpeedLimitCalculation()
    {
        // しゃがんでいるときは移動速度の上限を下げる
        if (playerController.IsSquat)
        {
            speedLimit = speedLimit * squatSpeedLimit;
        }
        // 息止めている時は移動速度の上限を下げる
        if (playerController.IsStealth)
        {
            speedLimit = speedLimit * stealthSpeedLimit;
        }
        // 息切れている時は移動速度の上限を下げる
        if (brethController.IsBreathlessness)
        {
            speedLimit = speedLimit * breathlessnessSpeedLimit;
        }
        // ダッシュしているときは移動速度の上限をあげる
        if (playerController.IsDash)
        {
            speedLimit = speedLimit * dashSpeedLimit;
        }
    }

    /// <summary>
    /// 方向キーを押しているか、押していないか
    /// </summary>
    public bool IsDirectionKey()
    {
        if (GetDirectionKey(MoveDirection.FORWARD) || GetDirectionKey(MoveDirection.BACK) || GetDirectionKey(MoveDirection.LEFT) || GetDirectionKey(MoveDirection.RIGHT))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 方向キー入力の取得
    /// </summary>
    /// <param name="dir">移動方向</param>
    /// <returns>どの向きに動いているか</returns>
    bool GetDirectionKey(MoveDirection dir)
    {
        switch(dir)
        {
            case MoveDirection.FORWARD: if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { return true; } break;
            case MoveDirection.BACK: if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { return true; } break;
            case MoveDirection.LEFT : if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { return true; } break;
            case MoveDirection.RIGHT: if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { return true; } break;
        }

        return false;
    }

    /// <summary>
    /// 移動方向のRaycast
    /// </summary>
    /// <returns>移動方向でオブジェクトに衝突したかどうか</returns>
    bool MoveDirectionRay()
    {
        Vector3 start = new Vector3(transform.position.x, transform.position.y - (collider.height / 2), transform.position.z);
        Vector3 dir = new Vector3(moveSpeed.x, 0, moveSpeed.z);
        float distance = collider.radius + 0.1f;

        Ray ray = new Ray(start, dir);
        RaycastHit hit;

        Debug.DrawLine(start, start + (dir * distance), Color.red);

        if (Physics.Raycast(ray, out hit, distance,1 << 9))
        {
            return true; 
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 移動方向の斜め上のRaycast
    /// </summary>
    /// <returns>移動方向でオブジェクトに衝突したかどうか</returns>
    bool DiagonalDirectionRay()
    {
        Vector3 start = new Vector3(transform.position.x, transform.position.y - (collider.height / 2), transform.position.z);
        Vector3 dir = new Vector3(moveSpeed.normalized.x, stepAngle / 100, moveSpeed.normalized.z);
        float distance = collider.radius + 0.1f;

        Ray ray = new Ray(start, dir);
        RaycastHit hit;

        Debug.DrawLine(start, start + (dir * distance), Color.red);

        if (Physics.Raycast(ray, out hit, distance, 1 << 9))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
