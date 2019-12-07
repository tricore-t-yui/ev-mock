using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームの流れクラス
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField]
    PlayerStatusController statusController = default;
    [SerializeField]
    KageSpawn kageSpawn = default;
    [SerializeField]
    PlayerMoveController moveController = default;
    [SerializeField]
    PlayerStateController stateController = default;
    [SerializeField]
    PlayerHealthController healthController = default;

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
}
