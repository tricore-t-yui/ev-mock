using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectType = PlayerHideController.HideObjectType;

/// <summary>
/// 隠れるのをやめる時のステート
/// </summary>
public class HideObjectOutState : StateMachineBehaviour
{
    [SerializeField]
    PlayerHideController hideController = default;  // 隠れるアクション管理クラス
    [SerializeField]
    Transform player = default;                     // プレイヤー
    [SerializeField]
    ObjectType type = default;                      // オブジェクトタイプ
    [SerializeField]
    bool isPlayer = default;                        // プレイヤーかどうか

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ベットだったらベットの外まで座標移動
        if (type == ObjectType.BED)
        {
            // 座標移動はベッドの向きに応じて決める
            switch (hideController.HideObjDir)
            {
                case InteractFunction.DirType.FORWARD: player.rotation = Quaternion.Euler(90, 0, -180);  break;
                case InteractFunction.DirType.BACK: player.rotation = Quaternion.Euler(90, 0, 0); break;
                case InteractFunction.DirType.RIGHT: player.rotation = Quaternion.Euler(90, 0, -90); break;
                case InteractFunction.DirType.LEFT: player.rotation = Quaternion.Euler(90, 0, -270); break;
            }
        }
    }

    /// <summary>
    /// ステートに入っている間
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ベットだったらベットの外まで座標移動
        if (type == ObjectType.BED)
        {
            // 座標移動はベッドの向きに応じて決める
            switch (hideController.HideObjDir)
            {
                case InteractFunction.DirType.FORWARD: player.position += Vector3.back * 0.02f; break;
                case InteractFunction.DirType.BACK: player.position += Vector3.forward * 0.02f; break;
                case InteractFunction.DirType.RIGHT: player.position += Vector3.left * 0.02f; break;
                case InteractFunction.DirType.LEFT: player.position += Vector3.right * 0.02f; break;
            }
        }
    }

    /// <summary>
    /// ステートを出た瞬間
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // プレイヤーだったら初期化を始める
        if (isPlayer)
        {
            hideController.EndAction();
        }
    }
}
