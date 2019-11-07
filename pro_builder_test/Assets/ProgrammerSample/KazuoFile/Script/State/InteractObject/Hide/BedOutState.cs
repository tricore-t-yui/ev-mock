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
    bool isRotation = false;                            // 脱出方向回転フラグ

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 回転フラグを立てる
        isRotation = true;

        // 隠れるアクションクラス取得
        hideController = animator.gameObject.GetComponent<PlayerHideController>();

        // 脱出方向、座標を求める
        ExitTransform();
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
        if (animator.gameObject.transform.rotation != exitRotation && isRotation)
        {
            Quaternion rotation = Quaternion.RotateTowards(animator.gameObject.transform.rotation, exitRotation, exitRotationSpeed);
            animator.gameObject.transform.rotation = rotation;
        }
        // 回転し終わったらフラグを切る
        else
        {
            isRotation = false;
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 隠れる終了
        animator.SetBool("HideEnd", true);
    }

    /// <summary>
    /// 脱出方向、座標を求める
    /// </summary>
    void ExitTransform()
    {
        Transform target = hideController.HideObj.transform;

        switch (hideController.HideObjDir)
        {
            case DirType.FORWARD:
                exitPos = new Vector3(target.position.x, target.position.y, target.position.z - (target.localScale.z / 2 + 1));
                exitRotation = Quaternion.Euler(90, 0, -180); break;
            case DirType.BACK:
                exitPos = new Vector3(target.position.x, target.position.y, target.position.z + (target.localScale.z / 2 + 1));
                exitRotation = Quaternion.Euler(90, 0, 0); break;
            case DirType.RIGHT:
                exitPos = new Vector3(target.position.x - (target.localScale.x / 2 + 1), target.position.y, target.position.z);
                exitRotation = Quaternion.Euler(90, 0, -270); break;
            case DirType.LEFT:
                exitPos = new Vector3(target.position.x + (target.localScale.z / 2 + 1), target.position.y, target.position.z);
                exitRotation = Quaternion.Euler(90, 0, -90); break;
        }
    }
}