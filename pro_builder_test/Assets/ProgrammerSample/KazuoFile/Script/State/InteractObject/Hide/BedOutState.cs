using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirType = InteractFunction.DirType;

/// <summary>
/// ベッドに隠れるのをやめる時のステート
/// </summary>
public class BedOutState : StateMachineBehaviour
{
    PlayerHideController hideController = default;      // 隠れるアクションクラス

    [SerializeField]
    float exitRotationSpeed = 2;                        // 脱出方向へ向くスピード

    Vector3 exitPos = default;                          // 脱出座標
    Quaternion exitRotation = default;                  // 脱出回転方向
    DirType exitDir = default;                          // 脱出方向のタイプ

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 隠れるアクションクラス取得
        hideController = animator.gameObject.GetComponent<PlayerHideController>();

        // アニメーション回転フラグを切る
        hideController.SetIsAnimRotation(false);

        // 脱出方向、座標を求める
        ExitTransform(animator);
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 求めた脱出座標からベクトルを求め、ベクトル移動
        Vector3 exitVec = (exitPos - animator.gameObject.transform.position).normalized;
        animator.gameObject.transform.position += new Vector3(exitVec.x, 0, exitVec.z) * 0.02f;

        // 求めた脱出方向に向かって回転
        if (animator.gameObject.transform.rotation.eulerAngles != exitRotation.eulerAngles && !hideController.IsAnimRotation)
        {
            Quaternion rotation = Quaternion.RotateTowards(animator.gameObject.transform.rotation, exitRotation, exitRotationSpeed);
            animator.gameObject.transform.rotation = rotation;
        }
        // 回転し終わったらアニメーションに回転を任せる
        else
        {
            hideController.SetIsAnimRotation(true);
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 隠れる終了
        animator.ResetTrigger("BedIn");
        animator.ResetTrigger("BedOut");
        animator.SetBool("Stealth", false);
        animator.SetBool("HideEnd", true);
    }

    /// <summary>
    /// 脱出方向、座標を求める
    /// </summary>
    void ExitTransform(Animator animator)
    {
        // 隠れているオブジェクト
        Transform hideObj = hideController.HideObj.transform;

        // 今プレイヤーが向いている向き
        Vector3 angle = animator.gameObject.transform.eulerAngles;

        // 今プレイヤーが向いている方向から脱出方向をとり、その方向が壁とかぶっていないかを見る
        if ((angle.y >= 315 && angle.y <= 360) || (angle.y >= 0 && angle.y <= 45))
        {
            exitDir = ChangeAvoidWallDir(angle, DirType.BACK);
        }
        else if (angle.y >= 135 && angle.y <= 225)
        {
            exitDir = ChangeAvoidWallDir(angle, DirType.FORWARD);
        }
        else if(angle.y >= 180)
        {
            exitDir = ChangeAvoidWallDir(angle, DirType.RIGHT);
        }
        else
        {
            exitDir = ChangeAvoidWallDir(angle, DirType.LEFT);
        }

        // 求めた脱出方向から、座標、回転方向を求める
        switch (exitDir)
        {
            case DirType.FORWARD:
                exitPos = new Vector3(hideObj.position.x, hideObj.position.y, hideObj.position.z - (hideObj.localScale.z / 2 + 1));
                exitRotation = Quaternion.Euler(90, 0, -180); break;
            case DirType.BACK:
                exitPos = new Vector3(hideObj.position.x, hideObj.position.y, hideObj.position.z + (hideObj.localScale.z / 2 + 1));
                exitRotation = Quaternion.Euler(90, 0, 0); break;
            case DirType.RIGHT:
                exitPos = new Vector3(hideObj.position.x - (hideObj.localScale.x / 2 + 1), hideObj.position.y, hideObj.position.z);
                exitRotation = Quaternion.Euler(90, 0, -270); break;
            case DirType.LEFT:
                exitPos = new Vector3(hideObj.position.x + (hideObj.localScale.x / 2 + 1), hideObj.position.y, hideObj.position.z);
                exitRotation = Quaternion.Euler(90, 0, -90); break;
        }
    }

    /// <summary>
    /// 壁を避けた方向に行くように変更
    /// </summary>
    /// <param name="angle">プレイヤーの向き</param>
    /// <param name="dir">変更前の脱出方向</param>
    DirType ChangeAvoidWallDir(Vector3 angle, DirType dir)
    {
        switch(dir)
        {
            case DirType.FORWARD:
               return CheckAvoidWallForward(angle, dir);
            case DirType.BACK:
                return CheckAvoidWallBack(angle, dir);
            case DirType.RIGHT:
                return CheckAvoidWallRight(angle, dir);
            case DirType.LEFT:
                return CheckAvoidWallLeft(angle, dir);

        }
        return dir;
    }

    /// <summary>
    /// 壁を避けた方向に行くように変更(前)
    /// </summary>
    /// <param name="angle">プレイヤーの向き</param>
    /// <param name="dir">変更前の脱出方向</param>
    DirType CheckAvoidWallForward(Vector3 angle, DirType dir)
    {
        if (IsWallDir(dir))
        {
            if (angle.y >= 180)
            {
                if (IsWallDir(DirType.RIGHT))
                {
                    if (IsWallDir(DirType.LEFT))
                    {
                        return DirType.BACK;
                    }
                    else
                    {
                        return DirType.LEFT;
                    }
                }
                else
                {
                    return DirType.RIGHT;
                }
            }
            else
            {
                if (IsWallDir(DirType.LEFT))
                {
                    if (IsWallDir(DirType.RIGHT))
                    {
                        return DirType.BACK;
                    }
                    else
                    {
                        return DirType.RIGHT;
                    }
                }
                else
                {
                    return DirType.LEFT;
                }
            }
        }
        else
        {
            return DirType.FORWARD;
        }
    }
    /// <summary>
    /// 壁を避けた方向に行くように変更(後ろ)
    /// </summary>
    /// <param name="angle">プレイヤーの向き</param>
    /// <param name="dir">変更前の脱出方向</param>
    DirType CheckAvoidWallBack(Vector3 angle, DirType dir)
    {
        if (IsWallDir(dir))
        {
            if (angle.y >= 180)
            {
                if (IsWallDir(DirType.RIGHT))
                {
                    if (IsWallDir(DirType.LEFT))
                    {
                        return DirType.FORWARD;
                    }
                    else
                    {
                        return DirType.LEFT;
                    }
                }
                else
                {
                    return DirType.RIGHT;
                }
            }
            else
            {
                if (IsWallDir(DirType.LEFT))
                {
                    if (IsWallDir(DirType.RIGHT))
                    {
                        return DirType.FORWARD;
                    }
                    else
                    {
                        return DirType.RIGHT;
                    }
                }
                else
                {
                    return DirType.LEFT;
                }
            }
        }
        else
        {
            return DirType.BACK;
        }
    }
    /// <summary>
    /// 壁を避けた方向に行くように変更(右)
    /// </summary>
    /// <param name="angle">プレイヤーの向き</param>
    /// <param name="dir">変更前の脱出方向</param>
    DirType CheckAvoidWallRight(Vector3 angle, DirType dir)
    {
        if (IsWallDir(dir))
        {
            if ((angle.y >= 315 && angle.y <= 360) || (angle.y >= 0 && angle.y <= 45))
            {
                if (IsWallDir(DirType.FORWARD))
                {
                    if (IsWallDir(DirType.BACK))
                    {
                        return DirType.LEFT;
                    }
                    else
                    {
                        return DirType.BACK;
                    }
                }
                else 
                {
                    return DirType.FORWARD;
                }
            }
            else if (angle.y >= 135 && angle.y <= 225)
            {
                if (IsWallDir(DirType.BACK))
                {
                    if (IsWallDir(DirType.FORWARD))
                    {
                        return DirType.LEFT;
                    }
                    else
                    {
                        return DirType.FORWARD;
                    }
                }
                else
                {
                    return DirType.BACK;
                }
            }
            else
            {
                return DirType.LEFT;
            }
        }
        else
        {
            return DirType.RIGHT;
        }
    }
    /// <summary>
    /// 壁を避けた方向に行くように変更(左)
    /// </summary>
    /// <param name="angle">プレイヤーの向き</param>
    /// <param name="dir">変更前の脱出方向</param>
    DirType CheckAvoidWallLeft(Vector3 angle, DirType dir)
    {
        if (IsWallDir(dir))
        {
            if ((angle.y >= 315 && angle.y <= 360) || (angle.y >= 0 && angle.y <= 45))
            {
                if (IsWallDir(DirType.FORWARD))
                {
                    if (IsWallDir(DirType.BACK))
                    {
                        return DirType.RIGHT;
                    }
                    else
                    {
                        return DirType.BACK;
                    }
                }
                else
                {
                    return DirType.FORWARD;
                }
            }
            else if (angle.y >= 135 && angle.y <= 225)
            {
                if (IsWallDir(DirType.BACK))
                {
                    if (IsWallDir(DirType.FORWARD))
                    {
                        return DirType.RIGHT;
                    }
                    else
                    {
                        return DirType.FORWARD;
                    }
                }
                else
                {
                    return DirType.BACK;
                }
            }
            else
            {
                return DirType.RIGHT;
            }
        }
        else
        {
            return DirType.LEFT;
        }
    }

    /// <summary>
    /// 壁の方向かどうか
    /// </summary>
    bool IsWallDir(DirType dir)
    {
        if(hideController.GetHideObjWallContactDir().Contains(dir))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
