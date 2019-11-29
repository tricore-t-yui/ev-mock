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
    Quaternion exitRotation = default;                  // 脱出方向

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
        if (animator.gameObject.transform.rotation != exitRotation && !hideController.IsAnimRotation)
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

        // 各向きに合わせて脱出方向、座標を求める
        if ((angle.y >= 315 && angle.y <= 360) || (angle.y >= 0 && angle.y <= 45))
        {
            exitPos = new Vector3(hideObj.position.x, hideObj.position.y, hideObj.position.z + (hideObj.localScale.z / 2 + 1));
            exitRotation = Quaternion.Euler(90, 0, 0);
        }
        else if (angle.y >= 135 && angle.y <= 225)
        {
            exitPos = new Vector3(hideObj.position.x, hideObj.position.y, hideObj.position.z - (hideObj.localScale.z / 2 + 1));
            exitRotation = Quaternion.Euler(90, 0, -180);
        }
        else if(angle.y >= 180)
        {
            exitPos = new Vector3(hideObj.position.x - (hideObj.localScale.x / 2 + 1), hideObj.position.y, hideObj.position.z);
            exitRotation = Quaternion.Euler(90, 0, -270);
        }
        else
        {
            exitPos = new Vector3(hideObj.position.x + (hideObj.localScale.z / 2 + 1), hideObj.position.y, hideObj.position.z);
            exitRotation = Quaternion.Euler(90, 0, -90);
        }
    }
}
