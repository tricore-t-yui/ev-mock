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
    float exitRotationSpeed = 2;                    // 脱出方向へ向くスピード
    [SerializeField]
    bool isPlayer = default;                        // プレイヤーかどうか

    Quaternion exitRotation = default;              // 脱出方向
    bool isExitRotation = false;                    // 脱出方向に向くかどうかのフラグ

    /// <summary>
    /// ステートに入った瞬間
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // フラグを立てる
        isExitRotation = true;

        // オブジェクトタイプに合わせた、オブジェクトから出る向きを求める
        if (type == ObjectType.BED)
        {
            // ベッド
            switch (hideController.HideObjDir)
            {
                case InteractFunction.DirType.FORWARD: exitRotation = Quaternion.Euler(90, 0, -180); break;
                case InteractFunction.DirType.BACK: exitRotation = Quaternion.Euler(90, 0, 0); break;
                case InteractFunction.DirType.RIGHT: exitRotation = Quaternion.Euler(90, 0, -90); break;
                case InteractFunction.DirType.LEFT: exitRotation = Quaternion.Euler(90, 0, -270); break;
            }
        }
        else
        {
            // ロッカー
            switch (hideController.HideObjDir)
            {
                case InteractFunction.DirType.FORWARD: exitRotation = Quaternion.Euler(0, 180, 0); break;
                case InteractFunction.DirType.BACK: exitRotation = Quaternion.Euler(0, 0, 0); break;
                case InteractFunction.DirType.RIGHT: exitRotation = Quaternion.Euler(0, 90, 0); break;
                case InteractFunction.DirType.LEFT: exitRotation = Quaternion.Euler(0, 270, 0); break;
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

        // オブジェクトから出る向きに向いていなかったら回転させる
        if (player.rotation != exitRotation && isExitRotation)
        {
            Quaternion rotation = Quaternion.RotateTowards(player.rotation, exitRotation, exitRotationSpeed);
            player.rotation = rotation;
        }
        // 出る向きを向いたらフラグを立てて回転させないようにする
        else
        {
            isExitRotation = false;
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
