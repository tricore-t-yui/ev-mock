﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームの流れクラス
/// </summary>
public class GameController : MonoBehaviour
{
    [SerializeField]
    bool isDisplayCursor = false;                       // カーソルを表示するかどうか

    [SerializeField]
    PlayerStatusController statusController = default;  // プレイヤーのステータスクラス
    [SerializeField]
    PlayerMoveController moveController = default;      // プレイヤーの移動クラス
    [SerializeField]
    PlayerStateController stateController = default;    // プレイヤーの状態クラス
    [SerializeField]
    PlayerHealthController healthController = default;  // プレイヤーの体力クラス
    [SerializeField]
    PlayerDamageController damageController = default;  // 隠れるクラス
    [SerializeField]
    GameObject tunObject = default;                     // ツンのオブジェクト
    [SerializeField]
    KageManager kageManager = default;                  // 影人間の管理クラス
    [SerializeField]
    EnemySpawn[] enemySpawn = default;                  // 影人間生成クラス
    [SerializeField]
    TrapTunGroupController[] trapTunGroup = default;    // 罠ツンのグループクラス

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

        stateController.ResetAreaName();

        if (!isDisplayCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 死んだらゲームオーバー
        if(healthController.IsDeath && !damageController.enabled)
        {
            GameOver();
        }

        // リセット(デバック用)
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameOver();
        }
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver()
    {
        // 各リセット
        statusController.ResetStatus();
        moveController.ResetPos();
        stateController.ResetState();
        stateController.ResetAreaName();
        StartCoroutine(kageManager.ResetAllKage());
        if (tunObject != null) tunObject.SetActive(false);

        foreach (var item in enemySpawn)
        {
            item.ResetEnemy();
        }

        foreach (var item in trapTunGroup)
        {
            item.ResetTrapTun();
        }
    }

    /// <summary>
    /// 次のシーンへ移動
    /// </summary>
    public void ChangeNextScene()
    {
        if(IsReturn)
        {
            SceneManager.LoadScene("GoingScene_1.2");
        }
        else
        {
            SceneManager.LoadScene("ReturnScene_1.2");
        }
    }
}
