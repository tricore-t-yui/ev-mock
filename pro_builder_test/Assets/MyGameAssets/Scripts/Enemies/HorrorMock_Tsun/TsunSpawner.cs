﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[System.Serializable]
public class SpawnParameter
{
    public Tsun spawnTsun = default;
    public float spawnDelayTime = 0;
    public List<Vector2> spawnPos = default;
    public List<ShadowHuman> shadowTrigger = default;

    [HideInInspector]
    public float spawnCounter = 0;
    [HideInInspector]
    public enum SpawnState
    {
        Not,
        Start,
        Spawn,
    }
    public SpawnState spawnState = SpawnState.Not;
}

public class TsunSpawner : MonoBehaviour
{
    [SerializeField]
    List<SpawnParameter> spawnParameters = default;

    // ツンが感知した影人間
    ShadowHuman detectedShadowOfTsun = default;

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        // 乱数のシード値を設定
        Random.InitState((int)System.DateTime.Now.Ticks);

        // 全てのツンをオフにする
        spawnParameters.ForEach(elem => elem.spawnTsun.gameObject.SetActive(false));
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        foreach(SpawnParameter parameter in spawnParameters)
        {
            // 活動中のツンが１体でもいる場合は何もしない（自分以外）
            if (spawnParameters.Exists(elem => elem.spawnState == SpawnParameter.SpawnState.Spawn &&
                                               elem != parameter)) { continue; }

            if (parameter.spawnState == SpawnParameter.SpawnState.Not)
            {
                // １体でも戦闘状態の影人間がいれば
                if (parameter.shadowTrigger.Exists(
                    elem => elem.currentState == EnemyParameter.StateType.Fighting))
                {
                    // スポーンフラグをオン
                    parameter.spawnState = SpawnParameter.SpawnState.Start;

                    // おこている影人間を取得（複数いる場合は最後におこたやーつ）
                    detectedShadowOfTsun = parameter.shadowTrigger.FindLast(elem => elem.currentState == EnemyParameter.StateType.Fighting);
                }
            }

            // スポーン中
            else if (parameter.spawnState == SpawnParameter.SpawnState.Spawn)
            {
                if (parameter.spawnTsun.currentState == EnemyParameter.StateType.Caution)
                {
                    if (detectedShadowOfTsun != null)
                    {
                        parameter.spawnTsun.SetTargetPos(detectedShadowOfTsun.transform.position);
                    }
                }
                // 通常状態に戻った
                if (parameter.spawnTsun.currentState == EnemyParameter.StateType.Normal)
                {
                    // デスポーン
                    parameter.spawnTsun.gameObject.SetActive(false);
                    parameter.spawnState = SpawnParameter.SpawnState.Not;
                    detectedShadowOfTsun = null;
                }
            }

            // カウントを回して一定時間後にスポーンさせる
            else if (parameter.spawnState == SpawnParameter.SpawnState.Start)
            {
                parameter.spawnCounter += Time.deltaTime;
                //一定時間経過した
                if (parameter.spawnCounter > parameter.spawnDelayTime)
                {
                    // スポーンさせる
                    parameter.spawnTsun.Spawn(EnemyParameter.StateType.Caution,detectedShadowOfTsun.transform.position, LotterySpawnPos(parameter));

                    // スポーンステート変更
                    parameter.spawnState = SpawnParameter.SpawnState.Spawn;
                    parameter.spawnCounter = 0;
                }
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // ツンと影人間を線で結ぶ
        foreach(SpawnParameter parameter in spawnParameters)
        {
            foreach(ShadowHuman shadow in parameter.shadowTrigger)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(parameter.spawnTsun.transform.position, shadow.transform.position);
                Gizmos.color = Color.white;

                parameter.spawnPos.ForEach(elem =>
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawLine(parameter.spawnTsun.transform.position,new Vector3(elem.x,parameter.spawnTsun.transform.position.y,elem.y));
                    Gizmos.DrawSphere(new Vector3(elem.x, parameter.spawnTsun.transform.position.y, elem.y), 0.1f);
                    Gizmos.color = Color.white;
                });
            }
        }
    }
#endif

    /// <summary>
    /// スポーン位置を抽選
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public Vector3 LotterySpawnPos(SpawnParameter parameter)
    {
        Vector3 ret = Vector3.zero;
        float minDist = float.MaxValue;
        foreach (var elem in parameter.spawnPos)
        {
            var spawnPosv3 = new Vector3(elem.x, parameter.spawnTsun.transform.position.y, elem.y);
            var dist = (detectedShadowOfTsun.transform.position - spawnPosv3).sqrMagnitude;
            if(dist < minDist)
            {
                ret = spawnPosv3;
                minDist = dist;
            }
        }
        return ret;
    }
}
