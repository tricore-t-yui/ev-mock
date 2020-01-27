using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人形ゲットクラス
/// </summary>
public class DollGetController : MonoBehaviour
{
    [SerializeField]
    GameObject leftArm = default;                           // 左手(人形を持つ手)
    [SerializeField]
    PlayerAnimationContoller animationContoller = default;  // インタラクト用関数クラス
    [SerializeField]
    GameController gameController = default;                // ゲームの流れクラス

    public bool IsDollGet { get; private set; } = false;    // 人形をゲットしたかどうか

    float distance = 0.75f;                                 // 人形との距離
    GameObject doll = null;                                 // 人形

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        IsDollGet = false;
    }

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        transform.LookAt(new Vector3(doll.transform.position.x, transform.position.y, doll.transform.position.z));
        transform.position = new Vector3(doll.transform.position.x + distance, transform.position.y,doll.transform.position.z);
    }

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="targetObj"></param>
    public void SetInfo(GameObject targetObj)
    {
        doll = targetObj;
        animationContoller.AnimStart(PlayerAnimationContoller.AnimationType.GETDOLL);

        enabled = true;
    }

    /// <summary>
    /// 人形ゲット
    /// </summary>
    public void DollGet()
    {
        doll.transform.parent = leftArm.transform;
        IsDollGet = true;
    }

    /// <summary>
    /// イベント終了処理
    /// </summary>
    public void EndDollGet()
    {
        enabled = false;

        // 帰り開始
        gameController.StartReturn();
    }
}
