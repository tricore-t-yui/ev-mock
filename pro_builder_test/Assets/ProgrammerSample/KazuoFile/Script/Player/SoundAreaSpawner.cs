using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

/// <summary>
/// 音領域のスポナー
/// </summary>
public class SoundAreaSpawner : MonoBehaviour
{
    /// <summary>
    /// 行動音
    /// </summary>
    public enum ActionSoundType
    {
        STEALTH,                // 息止め
        HIDE,                   // 隠れる
        WAIT,                   // 待機
        WALK,                   // 移動
        SQUAT,                  // しゃがみ 
        DASH,                   // ダッシュ
        DOOROPEN,               // ドア開閉
        DASHDOOROPEN,           // ダッシュでドア開閉
        SMALLCONFUSION,         // 息の小さな乱れ
        MEDIUMCONFUSION,        // 息の乱れ
        LARGECONFUSION,         // 息の大きな乱れ
        BREATHLESSNESS,         // 息切れ
        DEEPBREATH,             // 深呼吸
        DAMAGE,                 // ダメージ
        DAMAGEHALFHEALTH,       // 体力が半分
        DAMAGEPINCHHEALTH,      // 体力がピンチ
        BAREFOOT,               // 裸足
        BAREFOOTDAMAGEOBJECT,   // 裸足でダメージオブジェクトを踏んだとき
        SHOESDAMAGEOBJECT,      // 靴でダメージオブジェクトを踏んだとき
    }

    [SerializeField]
    GameObject areaCollider = default;                      // 音発生の領域
    [SerializeField]
    float areaMagnification = 0.3f;                         // 拡大倍率
    [SerializeField]
    float spawnframe = 75;                                  // スポーンするまでのフレーム数

    float areaRadius = 0;                                   // 音発生の領域の半径
    float soundLevel = 0;                                   // 音量レベル
    float totalSoundLevel = 0;                              // 音量レベルの合計
    float spawnframeCount = 0;                              // スポーン用フレームカウント
    List<Transform> spawnList = new List<Transform>();      // スポーンされたオブジェクトのリスト

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 音量レベルが変わるか、スポーンまでのフレーム数に達したら
        if (totalSoundLevel != soundLevel || spawnframeCount >= spawnframe)
        {
            // スポーン
            Spawn();
        }

        // スポーンされたオブジェクトが0より多いなら
        if (spawnList.Count > 0)
        {
            // デスポーン
            Despawn();
        }

        // カウント、音量レベルリセット
        spawnframeCount++;
        soundLevel = 0;

    }

    /// <summary>
    /// スポーン
    /// </summary>
    void Spawn()
    {
        // 合計値に適応、それに応じて領域拡大
        totalSoundLevel = soundLevel;
        areaRadius = 1 + (areaMagnification * totalSoundLevel);

        if (areaRadius < 0.25f) { return; }

        // スポーンしてリストに追加
        var spawn = PoolManager.Pools["SoundArea"].Spawn(areaCollider);
        spawnList.Add(spawn);

        // カウントをリセット
        spawnframeCount = 0;
    }

    /// <summary>
    /// デスポーン
    /// </summary>
    void Despawn()
    {
        List<Transform> DespawnList = new List<Transform>();      // スポーンされたオブジェクトのリスト

        foreach (var item in spawnList)
        {
            // デスポーンして、リストから削除
            if(!item.gameObject.activeInHierarchy)
            {
                PoolManager.Pools["SoundArea"].Despawn(item);
                DespawnList.Add(item);
            }
        }

        foreach(var item in DespawnList)
        {
            spawnList.Remove(item);
        }
    }

    /// <summary>
    /// 音のレベルの加算
    /// </summary>
    public void AddSoundLevel(ActionSoundType type)
    {
        // レベル加算用変数
        float addLevel = 0;

        // 行動音によって音レベルを加算
        switch (type)
        {
            case ActionSoundType.STEALTH: addLevel = -3; break;
            case ActionSoundType.WAIT: addLevel = 1; break;
            case ActionSoundType.WALK: addLevel = 3; break;
            case ActionSoundType.SQUAT: addLevel = -2; break;
            case ActionSoundType.SMALLCONFUSION: addLevel = 1; break;
            case ActionSoundType.MEDIUMCONFUSION: addLevel = 1; break;
            case ActionSoundType.LARGECONFUSION: addLevel = 1; break;
            case ActionSoundType.HIDE: addLevel = 0; break;
            case ActionSoundType.DOOROPEN: addLevel = 1; break;
            case ActionSoundType.DEEPBREATH: addLevel = 3; break;
            case ActionSoundType.DASHDOOROPEN: addLevel = 4; break;
            case ActionSoundType.DASH: addLevel = 7; break;
            case ActionSoundType.BREATHLESSNESS: addLevel = 4; break;
            case ActionSoundType.DAMAGE: addLevel = 3; break;
            case ActionSoundType.DAMAGEHALFHEALTH: addLevel = 5; break;
            case ActionSoundType.DAMAGEPINCHHEALTH: addLevel = 8; break;
            case ActionSoundType.BAREFOOT: addLevel = -3; break;
            case ActionSoundType.SHOESDAMAGEOBJECT: addLevel = 2; break;
            case ActionSoundType.BAREFOOTDAMAGEOBJECT: addLevel = 10; break;
        }

        soundLevel += addLevel;
    }

    /// <summary>
    /// 音発生の領域の半径のゲット関数
    /// </summary>
    public float GetColliderRadius()
    {
        return areaRadius;
    }
}
