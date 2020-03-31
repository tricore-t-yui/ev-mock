using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームの流れクラス
/// </summary>
public class GameController : MonoBehaviour
{
    /// <summary>
    /// 帰りシーン遷移方法
    /// </summary>
    enum ReturnTransitionMethod
    {
        ANOTHERSCENE,   // 別シーン
        SAMESCENE,      // 同じシーン
    }

    [SerializeField]
    bool isDebug = false;   // デバックかどうか
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
    DollGetController dollGetController = default;      // 人形ゲットクラス
    [SerializeField]
    TutorialTriggerManager tutorialTriggerManager = default;    // チュートリアルトリガー管理クラス
    [SerializeField]
    ReturnTransitionMethod returnTransitionMethod = ReturnTransitionMethod.ANOTHERSCENE;    // 帰りのシーンの遷移方法

    [SerializeField]
    GameObject goingEnemy = default;                    // 行きの敵
    [SerializeField]
    GameObject returnEnemy = default;                   // 帰りの敵

    [SerializeField]
    EnemySpawn[] enemySpawn = default;                  // 影人間生成クラス
    [SerializeField]
    TrapTunGroupController[] trapTunGroup = default;    // 罠ツンのグループクラス

    public bool IsReturn { get; private set; } = false; // 帰りのシーンかどうか

    ShadowHuman[] shadows = default;
    ShadowHuman[] shadowGimmicks = default;
    Tsun[] tsuns = default;

    /// <summary>
    /// 開始処理
    /// </summary>
    void Start()
    {
        List<ShadowHuman> shadowHumens = FindObjectsOfType<ShadowHuman>().ToList();
        shadows = shadowHumens.FindAll(elem => elem.tag == "ShadowHuman").ToArray();
        shadowGimmicks = shadowHumens.FindAll(elem => elem.tag == "ShadowGimmick").ToArray();
        foreach (ShadowHuman shadow in shadowGimmicks)
        {
            shadow.gameObject.SetActive(false);
        }

        switch (returnTransitionMethod)
        {
            case ReturnTransitionMethod.ANOTHERSCENE:
                if (SceneManager.GetActiveScene().name == "ReturnScene_1.2")
                {
                    IsReturn = true;
                }
                else
                {
                    IsReturn = false;
                }
                break;
            case ReturnTransitionMethod.SAMESCENE:
                if(dollGetController.IsDollGet)
                {
                    IsReturn = true;
                }
                else
                {
                    IsReturn = false;
                }
                break;
        }

        EnemyReset();
        stateController.ResetAreaName();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 死んだらゲームオーバー
        if (healthController.IsDeath && !damageController.enabled)
        {
            GameOver();
        }

        // リセット(デバック用)
        if(isDebug && Input.GetKeyDown(KeyCode.Alpha1))
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
        EnemyReset();

        if (!IsReturn)
        {
            //tutorialTriggerManager.TriggerReset();
            foreach(ShadowHuman shadow in shadows)
            {
                shadow.gameObject.SetActive(false);
                shadow.Spawn(EnemyParameter.StateType.Normal);
            }
            foreach(ShadowHuman shadow in shadowGimmicks)
            {
                shadow.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 帰り開始
    /// </summary>
    public void StartReturn()
    {
        switch (returnTransitionMethod)
        {
            case ReturnTransitionMethod.ANOTHERSCENE:
                // NOTE:k.oishi シーン遷移はいったんコメントアウト
                //if (IsReturn)
                //{
                //    SceneManager.LoadScene("GoingScene_1.2");
                //}
                //else
                //{
                //    SceneManager.LoadScene("ReturnScene_1.2");
                //}
                break;
            case ReturnTransitionMethod.SAMESCENE:
                IsReturn = true;
                break;
        }

        EnemyReset();
    }

    /// <summary>
    /// 敵のリセット
    /// </summary>
    void EnemyReset()
    {
        if (!IsReturn)
        {
            if (returnTransitionMethod == ReturnTransitionMethod.SAMESCENE)
            {
                //goingEnemy?.SetActive(true);
                //returnEnemy?.SetActive(false);
            }
            foreach (var item in trapTunGroup)
            {
                item?.gameObject.SetActive(false);
            }
        }
        else
        {
            if (returnTransitionMethod == ReturnTransitionMethod.SAMESCENE)
            {
                goingEnemy.SetActive(false);
                returnEnemy.SetActive(true);
            }
            foreach (var item in trapTunGroup)
            {
                item.gameObject.SetActive(true);
            }
        }

        kageManager?.ResetAllKage();
        if (tunObject != null) tunObject.SetActive(false);

        foreach (var item in enemySpawn)
        {
            item?.ResetEnemy();
        }
        foreach (var item in trapTunGroup)
        {
            item?.ResetTrapTun();
        }
    }
}
