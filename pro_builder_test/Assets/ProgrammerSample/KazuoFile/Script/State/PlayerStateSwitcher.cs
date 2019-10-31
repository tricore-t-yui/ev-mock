using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのステート変更条件クラス
/// </summary>
public class PlayerStateSwitcher : MonoBehaviour
{
    /// <summary>
    /// ステートタイプ
    /// </summary>
    public enum StateType
    {
        WALK,           // 歩く時
        DASH,           // ダッシュ時
        STEALTH,        // 忍び歩き時
        SQUAT,          // しゃがみ時
        DOOROPEN,       // ドア時
        HIDE,           // 隠れる時
        DEEPBREATH,     // 深呼吸時
        BREATHLESSNESS, //息切れ時
    }

    GameObject rayObject = default;                     // レイに当たったオブジェクト

    [SerializeField]
    Transform player = default;                         // プレイヤー
    [SerializeField]
    CapsuleCollider collider = default;                 // プレイヤーのコライダー
    [SerializeField]
    PlayerBrethController brethController = default;    // 息管理クラス
    [SerializeField]
    PlayerDoorController doorController = default;      // ドア管理クラス
    [SerializeField]
    PlayerHideController hideController = default;      // 隠れる管理クラス
    [SerializeField]
    PlayerEventCaller eventCaller = default;            // イベント呼び出しクラス
    [SerializeField]
    Animator stateAnimator = default;                   // ステートのアニメーター

    [SerializeField]
    KeyCode dashKey = KeyCode.LeftShift;                // ダッシュキー
    [SerializeField]
    KeyCode squatKey = KeyCode.LeftCommand;             // しゃがみキー
    [SerializeField]
    KeyCode stealthKey = KeyCode.LeftControl;           // 忍び足キー
    [SerializeField]
    KeyCode deepBreathKey = KeyCode.Space;              // 深呼吸キー

    /// <summary>
    /// 各ステート検知
    /// </summary>
    /// <param name="type">各ステートのタイプ</param>
    public void CheckState(StateType type)
    {
        switch(type)
        {
            case StateType.WALK: CheckWalkState(); break;
            case StateType.DASH: CheckDashState(); break;
            case StateType.STEALTH: CheckStealthState(); break;
            case StateType.SQUAT: CheckSquatState(); break;
            case StateType.DOOROPEN: CheckDoorOpenState(); break;
            case StateType.HIDE: CheckHideState(); break;
            case StateType.DEEPBREATH: CheckDeepBreathState(); break;
            case StateType.BREATHLESSNESS: CheckBrethlessnessState(); break;
        }
    }

    /// <summary>
    /// しゃがみ検知
    /// </summary>
    /// NOTE:k.oishi しゃがみはステートを持っていないので検知と同時に処理
    void CheckSquatState()
    {
        if(Input.GetKey(squatKey))
        {
            stateAnimator.SetBool("Squat", true);
            eventCaller.Invoke(PlayerEventCaller.EventType.SQUAT);
        }
        else
        {
            stateAnimator.SetBool("Squat", false);
            eventCaller.Invoke(PlayerEventCaller.EventType.SQUATEND);
        }
    }

    /// <summary>
    /// 歩き検知
    /// </summary>
    void CheckWalkState()
    {
        if(GetDirectionKey() && !Input.GetKey(dashKey) && !Input.GetKey(stealthKey) && !stateAnimator.GetBool("DeepBreath"))
        {
            stateAnimator.SetBool("Walk", true);
        }
        else
        {
            stateAnimator.SetBool("Walk", false);
        }
    }

    /// <summary>
    /// ダッシュ検知
    /// </summary>
    void CheckDashState()
    {
        // 方向キーが押されている時
        if (GetDirectionKey() && Input.GetKey(dashKey) && !stateAnimator.GetBool("DeepBreath"))
        {
            stateAnimator.SetBool("Squat", true);
            stateAnimator.SetBool("Dash", true);
        }
        else
        {
            stateAnimator.SetBool("Dash", false);
        }
    }

    /// <summary>
    /// 忍び歩き検知
    /// </summary>
    void CheckStealthState()
    {
        if (GetDirectionKey() && !Input.GetKey(dashKey) && Input.GetKey(stealthKey) && !stateAnimator.GetBool("DeepBreath"))
        {
            stateAnimator.SetBool("Stealth", true);
        }
        else
        {
            stateAnimator.SetBool("Stealth", false);
        }
    }

    /// <summary>
    /// 深呼吸検知
    /// </summary>
    void CheckDeepBreathState()
    {
        if (!GetDirectionKey() && Input.GetKey(deepBreathKey) && brethController.NowAmount < 100)
        {
            stateAnimator.SetBool("DeepBreath", true);
        }
        else
        {
            stateAnimator.SetBool("DeepBreath", false);
        }
    }

    /// <summary>
    /// 息切れ検知
    /// </summary>
    void CheckBrethlessnessState()
    {
        if(brethController.NowAmount <= 0)
        {
            stateAnimator.SetBool("Brethlessness", true);
        }
    }

    /// <summary>
    /// 隠れる検知
    /// </summary>
    void CheckHideState()
    {
        if (Input.GetMouseButton(0) && ObjectLayer() == LayerMask.NameToLayer("Hide"))
        {
            if (!stateAnimator.GetBool("Hide"))
            {
                hideController.Init(rayObject);
                stateAnimator.SetBool("Hide", true);
            }
        }
        else
        {
            stateAnimator.SetBool("Hide", false);
        }
    }

    /// <summary>
    /// ドア開閉検知
    /// </summary>
    void CheckDoorOpenState()
    {
        if (Input.GetMouseButton(0) && ObjectLayer() == LayerMask.NameToLayer("Door"))
        {
            if (!stateAnimator.GetBool("DoorOpen"))
            {
                doorController.SetInfo(rayObject, stateAnimator.GetBool("Dash"));
                stateAnimator.SetBool("DoorOpen", true);
            }
        }

        if(!doorController.enabled && stateAnimator.GetBool("DoorOpen"))
        {
            stateAnimator.SetBool("DoorOpen", false);
        }
    }


    /// <summary>
    /// 方向キー検知
    /// </summary>
    /// <returns>方向キーのどれか１つが押されたかどうか</returns>
    bool GetDirectionKey()
    {
        if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 移動方向のレイに当たったオブジェクトのレイヤーを返す
    /// </summary>
    /// <returns>当たったオブジェクトのレイヤー</returns>
    int ObjectLayer()
    {
        // レイのスタート位置
        Vector3 start = player.position;

        // レイの向き
        Vector3 dir = player.forward;

        // レイの距離
        float distance = collider.radius * 3.5f;

        // レイヤーマスク(プレイヤーからレイが伸びているので除外)
        int layerMask = (1 << LayerMask.NameToLayer("Player"));
        layerMask = ~layerMask;

        // レイ作成
        Ray ray = new Ray(start, dir);
        RaycastHit hit = default;

        // デバック用ライン
        Debug.DrawLine(start, start + (dir * distance), Color.red);

        // レイに当たったらオブジェクトのレイヤー、外れていたら-1を返す
        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            rayObject = hit.collider.gameObject;
            return hit.collider.gameObject.layer;
        }
        else
        {
            return -1;
        }
    }
}
