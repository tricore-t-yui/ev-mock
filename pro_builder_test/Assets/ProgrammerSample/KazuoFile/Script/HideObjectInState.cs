using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectType = PlayerHideController.HideObjectType;

/// <summary>
/// 隠れている時のステート
/// </summary>
public class HideObjectInState : StateMachineBehaviour
{
    [SerializeField]
    PlayerHideController hideController = default;  // プレイヤーの隠れるアクションクラス
    [SerializeField]
    Transform player = default;                     // プレイヤー
    [SerializeField]
    ObjectType type = default;                      // オブジェクトタイプ

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ベットだったらベットの下まで座標移動
        if (!hideController.IsHide && type == ObjectType.BED)
        {
            // 座標移動はベッドの向きに応じて決める
            switch(hideController.HideObjDir)
            {
                case InteractFunction.DirType.FORWARD: player.position += Vector3.forward * 0.0125f; break;
                case InteractFunction.DirType.BACK: player.position += Vector3.back * 0.0125f; break;
                case InteractFunction.DirType.RIGHT: player.position += Vector3.right * 0.0125f; break;
                case InteractFunction.DirType.LEFT: player.position += Vector3.left * 0.0125f; break;
            }
        }
        // マウスの入力が途切れたら隠れるのをやめる
        else if (!Input.GetMouseButton(0))
        {
            switch (type)
            {
                case ObjectType.LOCKER: animator.SetTrigger("LockerOut"); break;
                case ObjectType.BED: animator.SetTrigger("BedOut"); break;
            }
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
