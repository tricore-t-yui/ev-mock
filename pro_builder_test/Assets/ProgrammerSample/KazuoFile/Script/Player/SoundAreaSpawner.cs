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
        BREATHHOLD,             // 息止め
        HIDE,                   // 隠れる
        WAIT,                   // 待機
        WALK,                   // 歩き
        STEALTH,                // 忍び歩き
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
        FALL,                   // 落下
    }

    [SerializeField]
    GameObject areaCollider = default;                      // 音発生の領域
    [SerializeField]
    float areaMagnification = 0.3f;                         // 拡大倍率
    [SerializeField]
    float spawnframe = 50;                                  // スポーンするまでのフレーム数
    [SerializeField]
    PlayerSoundData soundData = default;                    // 音発生の追加量

    float areaRadius = 0;                                   // 音発生の領域の半径
    float soundLevel = 0;                                   // 音量レベル
    public float TotalSoundLevel { get; private set; } = 0; // 音量レベルの合計
    float spawnframeCount = 0;                              // スポーン用フレームカウント
    List<Transform> spawnList = new List<Transform>();      // スポーンされたオブジェクトのリスト

    public bool IsDamageObjectSound { get; private set; } = false;

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // 音量レベルが変わるか、スポーンまでのフレーム数に達したら
        if (TotalSoundLevel != soundLevel || spawnframeCount >= spawnframe)
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

        // スポーンまでのフレーム数に達したらダメージオブジェクトを反応させる
        if (spawnframeCount >= spawnframe)
        {
            IsDamageObjectSound = false;
        }
    }

    /// <summary>
    /// スポーン
    /// </summary>
    void Spawn()
    {
        // 合計値に適応、それに応じて領域拡大
        TotalSoundLevel = soundLevel;
        areaRadius = 1 + (areaMagnification * TotalSoundLevel);
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
            case ActionSoundType.BREATHHOLD: addLevel = soundData.BreathHold; break;
            case ActionSoundType.WAIT: addLevel = soundData.Wait; break;
            case ActionSoundType.WALK: addLevel = soundData.Walk; break;
            case ActionSoundType.STEALTH: addLevel = soundData.Stealth; break;
            case ActionSoundType.SQUAT: addLevel = soundData.Squat; break;
            case ActionSoundType.HIDE: addLevel = soundData.Hide; break;
            case ActionSoundType.DOOROPEN: addLevel = soundData.DoorOpen; break;
            case ActionSoundType.DEEPBREATH: addLevel = soundData.DeepBreath; break;
            case ActionSoundType.DASHDOOROPEN: addLevel = soundData.DashDoorOpen; break;
            case ActionSoundType.DASH: addLevel = soundData.Dash; break;
            case ActionSoundType.BREATHLESSNESS: addLevel = soundData.Breathlessness; break;
            case ActionSoundType.DAMAGE: addLevel = soundData.Damage; break;
            case ActionSoundType.DAMAGEHALFHEALTH: addLevel = soundData.HalfBreath; break;
            case ActionSoundType.DAMAGEPINCHHEALTH: addLevel = soundData.PinchBreath; break;
            case ActionSoundType.BAREFOOT: addLevel = soundData.Barefoot; break;
            case ActionSoundType.SHOESDAMAGEOBJECT: addLevel = soundData.ShoesObjectDamage; break;
            case ActionSoundType.BAREFOOTDAMAGEOBJECT: addLevel = soundData.BarefootObjectDamage; break;
            case ActionSoundType.SMALLCONFUSION: addLevel = soundData.BreathSmallConfusion; break;
            case ActionSoundType.MEDIUMCONFUSION: addLevel = soundData.BreathMediumConfusion; break;
            case ActionSoundType.LARGECONFUSION: addLevel = soundData.BreathLargeConfusion; break;
            case ActionSoundType.FALL: addLevel = soundData.Fall; break;
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

    /// <summary>
    /// ダメージオブジェクトの音領域拡大フラグのセット関数
    /// </summary>
    /// <param name="flag"></param>
    public void SetIsDamageObjectSound(bool flag)
    {
        IsDamageObjectSound = flag;
    }
}
