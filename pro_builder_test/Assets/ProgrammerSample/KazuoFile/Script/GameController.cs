using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームの流れクラス
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField]
    PlayerStatusController statusController = default;  // プレイヤーのステータスクラス
    [SerializeField]
    KageSpawn kageSpawn = default;                      // 影人間生成クラス
    [SerializeField]
    PlayerMoveController moveController = default;      // プレイヤーの移動クラス
    [SerializeField]
    PlayerStateController stateController = default;    // プレイヤーの状態クラス
    [SerializeField]
    PlayerHealthController healthController = default;  // プレイヤーの体力クラス

    public bool IsReturn { get; private set; } = false; // 帰りのシーンかどうか

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "ReturnScene_1.2")
        {
            IsReturn = true;
        }
        else
        {
            IsReturn = false;
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 死んだらゲームオーバー
        if(healthController.IsDeath)
        {
            GameOver();
        }
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver()
    {
        statusController.ResetStatus();
        kageSpawn.ResetKage();
        moveController.ResetPos();
        stateController.ResetState();
    }

    /// <summary>
    /// 次のシーンへ移動
    /// </summary>
    /// <param name="isEnd">次のシーンが終わりかどうか</param>
    public void ChangeNextScene(bool isEnd)
    {
        if(isEnd)
        {
            SceneManager.LoadScene("GoingScene_1.3");
        }
        else
        {
            SceneManager.LoadScene("ReturnScene_1.2");
        }
    }
}
