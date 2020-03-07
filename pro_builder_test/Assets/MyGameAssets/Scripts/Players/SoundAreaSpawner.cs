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
        HIDE,                   // 隠れる
        WAIT,                   // 待機
        WALK,                   // 歩き
        BREATHHOLD_WALK,        // 歩き(息止め)
        STEALTH,                // 忍び歩き
        BREATHHOLD_STEALTH,     // 忍び歩き(息止め)
        SQUAT,                  // しゃがみ 
        BREATHHOLD_SQUAT,       // しゃがみ (息止め)
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
        STAMINA,                // スタミナの消費量に応じて
    }

    [SerializeField]
    GameObject areaCollider = default;                      // 音発生の領域
    [SerializeField]
    float areaMagnification = 0.3f;                         // 拡大倍率
    [SerializeField]
    float spawnframe = 50;                                  // スポーンするまでのフレーム数
    [SerializeField]
    PlayerSoundData soundData = default;                    // 音発生の追加量
    [SerializeField]
    PlayerBreathController breathController;
    [SerializeField]
    PlayerEvents playerEvents;

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
        if ((TotalSoundLevel != soundLevel || spawnframeCount >= spawnframe) && soundLevel != 0)
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
    /// 音のレベルのセット
    /// </summary>
    public void SetSoundLevel(ActionSoundType type)
    {
        // レベル加算用変数
        float setLevel = 0;
        // 行動音によって音レベルを加算
        switch (type)
        {
            case ActionSoundType.WAIT: setLevel = soundData.Wait; break;
            case ActionSoundType.WALK: setLevel = soundData.Walk; break;
            case ActionSoundType.BREATHHOLD_WALK: setLevel = soundData.BreathHoldWalk; break;
            case ActionSoundType.STEALTH: setLevel = soundData.Stealth; break;
            case ActionSoundType.BREATHHOLD_STEALTH: setLevel = soundData.BreathHoldStealth; break;
            case ActionSoundType.SQUAT: setLevel = soundData.Squat; break;
            case ActionSoundType.BREATHHOLD_SQUAT: setLevel = soundData.BreathHoldSquat; break;
            case ActionSoundType.HIDE: setLevel = soundData.Hide; break;
            case ActionSoundType.DOOROPEN: setLevel = soundData.DoorOpen; break;
            case ActionSoundType.DEEPBREATH: setLevel = soundData.DeepBreath; break;
            case ActionSoundType.DASHDOOROPEN: setLevel = soundData.DashDoorOpen; break;
            case ActionSoundType.DASH: setLevel = soundData.Dash; break;
            case ActionSoundType.BREATHLESSNESS: setLevel = soundData.Breathlessness; break;
            case ActionSoundType.DAMAGE: setLevel = soundData.Damage; break;
            case ActionSoundType.DAMAGEHALFHEALTH: setLevel = soundData.HalfBreath; break;
            case ActionSoundType.DAMAGEPINCHHEALTH: setLevel = soundData.PinchBreath; break;
            case ActionSoundType.BAREFOOT: setLevel = soundData.Barefoot; break;
            case ActionSoundType.SHOESDAMAGEOBJECT: setLevel = soundData.ShoesObjectDamage; break;
            case ActionSoundType.BAREFOOTDAMAGEOBJECT: setLevel = soundData.BarefootObjectDamage; break;
            case ActionSoundType.FALL: setLevel = soundData.Fall; break;
            case ActionSoundType.STAMINA:setLevel = soundData.Stamina; break;
        }

        // 係数計算
        var confusionFactor = 1.0f;
        switch(breathController.State) // 息切れレベル
        {
            case PlayerBreathController.BrethState.SMALLCONFUSION:
                confusionFactor = soundData.BreathSmallConfusionFactor;
                break;
            case PlayerBreathController.BrethState.MEDIUMCONFUSION:
                confusionFactor = soundData.BreathMediumConfusionFactor;
                break;
            case PlayerBreathController.BrethState.BREATHLESSNESS:
                confusionFactor = soundData.BreathMediumConfusionFactor;
                break;
            default:
                break;
        }

        // 息止め中以外は、息切れレベルの騒音を掛け算
        if(!playerEvents.IsBreathHold)
        {
            setLevel *= confusionFactor;
        }

        // 大きい音が鳴ったら大きい音優先
        if (setLevel > soundLevel)
        {
            soundLevel = setLevel;
            //Debug.Log("sound level:" + soundLevel + " increased:" + type + " playerStateController.IsBreathHold:" + playerEvents.IsBreathHold);
        }
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
