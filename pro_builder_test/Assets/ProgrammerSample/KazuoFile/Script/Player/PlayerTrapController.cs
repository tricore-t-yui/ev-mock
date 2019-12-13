using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrapController : MonoBehaviour
{
    [SerializeField]
    InteractFunction interactController = default;              // インタラクト用関数クラス
    [SerializeField]
    PlayerAnimationContoller animationContoller = default;      // アニメーション管理クラス

    public Transform TrapPos { get; private set; } = default;   // 敵のTransform

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        // 初期化
        interactController.CommonInit();
    }

    /// <summary>
    /// 罠座標の登録
    /// </summary>
    public void SetInfo(Transform trapPos)
    {
        TrapPos = trapPos;
        transform.LookAt(new Vector3(trapPos.position.x, transform.position.y, trapPos.position.z));

        animationContoller.AnimStart(PlayerAnimationContoller.AnimationType.TRAP);
    }

    /// <summary>
    /// 各アクションの終了
    /// </summary>
    public void EndTrapAction()
    {
        // アニメーションが再生され終わったら終了処理
        if (animationContoller.EndAnimation(PlayerAnimationContoller.EndAnimationType.TRAP))
        {
            animationContoller.SetEndAnimationFlag(PlayerAnimationContoller.EndAnimationType.TRAP);
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            interactController.CommonEndAction();

            // 処理終了
            enabled = false;
        }
    }
}
