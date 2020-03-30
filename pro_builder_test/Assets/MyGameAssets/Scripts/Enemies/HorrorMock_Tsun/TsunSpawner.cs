using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[System.Serializable]
public class SpawnParameter
{
    public Tsun tsun = default;
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
        Spawned,
    }
    public SpawnState spawnState = SpawnState.Not;
}

public class TsunSpawner : MonoBehaviour
{
    [SerializeField]
    SpawnParameter spawnParameter = default;

    // ツンが感知した影人間
    ShadowHuman detectedShadowOfTsun = default;

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        // 乱数のシード値を設定
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        if (spawnParameter.spawnState == SpawnParameter.SpawnState.Not)
        {
            // １体でも戦闘状態の影人間がいれば
            if (spawnParameter.shadowTrigger.Exists(
                elem => elem.currentState == EnemyParameter.StateType.Fighting))
            {
                // スポーンフラグをオン
                spawnParameter.spawnState = SpawnParameter.SpawnState.Start;

                // おこている影人間を取得（複数いる場合は最後におこたやーつ）
                detectedShadowOfTsun = spawnParameter.shadowTrigger.FindLast(elem => elem.currentState == EnemyParameter.StateType.Fighting);
            }
        }
        // カウントを回して一定時間後にスポーンさせる
        else if (spawnParameter.spawnState == SpawnParameter.SpawnState.Start)
        {
            spawnParameter.spawnCounter += Time.deltaTime;
            //一定時間経過した
            if (spawnParameter.spawnCounter > spawnParameter.spawnDelayTime)
            {
                // スポーンさせる
                spawnParameter.tsun.Spawn(EnemyParameter.StateType.Caution, detectedShadowOfTsun.transform.position, LotterySpawnPos());
                spawnParameter.tsun.OnSpawned();

                // スポーンステート変更
                spawnParameter.spawnState = SpawnParameter.SpawnState.Spawn;
                spawnParameter.spawnCounter = 0;
            }
        }
        // スポーン中
        else if (spawnParameter.spawnState == SpawnParameter.SpawnState.Spawn)
        {
            // 通常状態に戻った
            if (spawnParameter.tsun.currentState == EnemyParameter.StateType.Normal)
            {
                // デスポーン
                spawnParameter.tsun.Spawn(EnemyParameter.StateType.Normal);
                spawnParameter.spawnState = SpawnParameter.SpawnState.Not;
                detectedShadowOfTsun = null;
            }
        }
    }

    /// <summary>
    /// スポーン位置を抽選
    /// </summary>
    /// <returns></returns>
    Vector3 LotterySpawnPos()
    {
        Vector3 ret = Vector3.zero;
        float minDist = float.MaxValue;
        foreach (var elem in spawnParameter.spawnPos)
        {
            var spawnPosv3 = new Vector3(transform.position.x + elem.x, transform.position.y, transform.position.z + elem.y);
            var dist = (detectedShadowOfTsun.transform.position - spawnPosv3).sqrMagnitude;
            if (dist < minDist)
            {
                ret = spawnPosv3;
                minDist = dist;
            }
        }
        return ret;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // スポーンポジション設定していない状態ならスフィアだけ出す
        foreach (var pos in spawnParameter.spawnPos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector3(transform.position.x + pos.x, transform.position.y, transform.position.z + pos.y), 0.1f);
        };
    }
#endif
}
