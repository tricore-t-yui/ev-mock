using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの動き管理クラス
/// </summary>
public class PlayerMoveController : MonoBehaviour
{
    /// <summary>
    /// ジャンプの状態
    /// </summary>
    enum JumpState
    {
        START,
        WAIT,
        JUMP,
        LENGHT,
    }

    [SerializeField]
    Rigidbody rigidbody = default;          // リジットボディ
    [SerializeField]
    CapsuleCollider collider = default;     // コライダー

    [SerializeField]
    KeyCode dashKey = default;              // ダッシュキー
    [SerializeField]
    KeyCode squatKey = default;             // しゃがみキー
    [SerializeField]
    KeyCode hidedKey = default;             // 隠れるキー
    [SerializeField]
    KeyCode jumpKey = default;              // ジャンプキー
    [SerializeField]
    KeyCode stealthKey = default;             // 隠れるキー

    public bool IsDash { get; private set; } = false;           // ダッシュフラグ
    public bool IsSquat { get; private set; } = false;          // しゃがみフラグ
    public bool IsStealth { get; private set; } = false;        // 忍び歩きフラグ
    public bool IsHide { get; private set; } = false;           // 隠れるフラグ
    public bool IsWarning { get; private set; } = false;        // 警戒フラグ

    bool[] isJump = new bool[(int)JumpState.LENGHT];            // ジャンプフラグ

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        // 各要素の初期化
        IsDash = false;
        IsSquat = false;
        IsHide = false;
        IsWarning = false;
        IsStealth = false;

        for (int i = 0; i < (int)JumpState.LENGHT; i++)
        {
            isJump[i] = false;
        }
    }

    /// <summary>
    /// トリガーに当たった時
    /// </summary>
    /// <param name="other">当たったコライダー</param>
    void OnTriggerEnter(Collider other)
    {
        if(LayerMask.LayerToName(other.gameObject.layer) == "WarningArea")
        {
            IsWarning = true;
        }
    }

    /// <summary>
    /// トリガーから離れた時
    /// </summary>
    /// <param name="other">離れたコライダー</param>
    void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "WarningArea")
        {
            IsWarning = false;
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 隠れる検知
        Hide();

        // 隠れていなかったら(動ける状態なら)
        if (!IsHide)
        {
            // 各移動処理の検知
            Stealth();
            Squat();
            Dash();
            Jump();
        }
    }

    /// <summary>
    /// 隠れる処理
    /// </summary>
    void Hide()
    {
        // 隠れるキーが押されたら
        if (Input.GetKeyDown(hidedKey))
        {
            // 隠れる開始
            if (!IsHide)
            {
                IsHide = true;

                // 動かないので移動系のフラグの初期化
                IsDash = false;
                IsSquat = false;
                for (int i = 0; i < (int)JumpState.LENGHT; i++)
                {
                    isJump[i] = false;
                }
            }
            else
            {
                // 隠れる終了
                IsHide = false;
            }          
        }
    }

    /// <summary>
    /// 忍び歩き検知処理
    /// </summary>
    void Stealth()
    {
        // 忍び歩きキーが押されたら
        if (Input.GetKey(stealthKey))
        {
            // 忍び歩き開始
            IsStealth = true;
        }
        else
        {
            // 忍び歩き終了
            IsStealth = false;
        }
    }

    /// <summary>
    /// ダッシュ検知処理
    /// </summary>
    void Dash()
    {
        // ダッシュキーが押されたら
        if (!IsStealth && !IsSquat && Input.GetKey(dashKey))
        {
            // ダッシュ開始
            IsDash = true;
        }
        else
        {
            // ダッシュ終了
            IsDash = false;
        }
    }

    /// <summary>
    /// しゃがみ検知処理
    /// </summary>
    void Squat()
    {
        // しゃがみキーが押されたら
        if (Input.GetKey(squatKey))
        {
            // コライダーをずらして、しゃがみ開始
            collider.height = 0.7f;
            IsSquat = true;
        }
        else
        {
            // コライダーを戻して、しゃがみ終了
            collider.height = 1.4f;
            IsSquat = false;
        }
    }

    /// <summary>
    /// ジャンプ検知処理
    /// </summary>
    void Jump()
    {
        // ジャンプキーが押されたら
        if (Input.GetKeyDown(jumpKey) && IsGround() && !isJump[(int)JumpState.START] && !isJump[(int)JumpState.WAIT] && !isJump[(int)JumpState.JUMP])
        {
            // ジャンプ処理開始
            isJump[(int)JumpState.START] = true;
        }

        // ジャンプし始めたらジャンプ開始フラグを立てる
        if (isJump[(int)JumpState.WAIT] && rigidbody.velocity.y > 0)
        {
            isJump[(int)JumpState.JUMP] = true;
        }

        // ジャンプしている状態で地面に着いたらジャンプ終了
        if (IsGround() && isJump[(int)JumpState.JUMP])
        {
            isJump[(int)JumpState.START] = false;
            isJump[(int)JumpState.WAIT] = false;
            isJump[(int)JumpState.JUMP] = false;
        }
    }

    /// <summary>
    /// ジャンプ処理の開始フラグの受け渡し処理
    /// </summary>
    public bool IsJumpStart()
    {
        if(isJump[(int)JumpState.START] && !isJump[(int)JumpState.WAIT] && !isJump[(int)JumpState.JUMP])
        {
            isJump[(int)JumpState.START] = false;
            isJump[(int)JumpState.WAIT] = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 地面に接ししているか
    /// </summary>
    bool IsGround()
    {
        // rayの距離
        // NOTE:0.1fは、コライダーのなかにrayが入り込んでしまうのを防ぐための誤差
        float maxDistance = collider.height / 2 + 0.1f;
        if (Physics.Raycast(transform.position, Vector3.down, maxDistance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
