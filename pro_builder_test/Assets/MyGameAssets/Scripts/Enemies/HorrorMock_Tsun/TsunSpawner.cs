using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[System.Serializable]
public class SpawnPosParameter
{
    [HorizontalGroup("hg1",LabelWidth = 30,PaddingRight = 10)]
    public Vector2 pos = Vector2.zero;
    [HorizontalGroup("hg1")]
    [Range(0, 100)]
    public float rate = 100;
}

[System.Serializable]
public class SpawnParameter
{
    public Tsun spawnTsun = default;
    public float spawnDelayTime = 0;
    public List<SpawnPosParameter> spawnPos = default;
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
                    parameter.spawnTsun.Spawn(EnemyParameter.StateType.Caution,LotterySpawnPos(parameter),detectedShadowOfTsun.transform.position);

                    // スポーンステート変更
                    parameter.spawnState = SpawnParameter.SpawnState.Spawn;
                    parameter.spawnCounter = 0;
                }
            }
        }
    }

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
                    Gizmos.DrawLine(parameter.spawnTsun.transform.position,new Vector3(elem.pos.x,parameter.spawnTsun.transform.position.y,elem.pos.y));
                    Gizmos.DrawSphere(new Vector3(elem.pos.x, parameter.spawnTsun.transform.position.y, elem.pos.y), 0.1f);
                    Gizmos.color = Color.white;
                });
            }
        }
    }

    /// <summary>
    /// スポーン位置を抽選
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public Vector3 LotterySpawnPos(SpawnParameter parameter)
    {
        // 全ての出現確率の合計値を取得
        float totalRate = parameter.spawnPos.Sum(elem => elem.rate);

        // 乱数を生成
        float rand = Random.Range(0, totalRate);

        foreach(SpawnPosParameter posParameter in parameter.spawnPos)
        {
            // 乱数を確率で引いていく
            rand -= posParameter.rate;

            if (rand <= 0)
            {
                // 乱数が０になった時点の座標を返す
                return new Vector3(posParameter.pos.x, parameter.spawnTsun.transform.position.y, posParameter.pos.y);
            }
        }
        // ここにきたら抽選に失敗
        Debug.LogError("lottery failed.");
        return Vector3.zero;
    }
}
