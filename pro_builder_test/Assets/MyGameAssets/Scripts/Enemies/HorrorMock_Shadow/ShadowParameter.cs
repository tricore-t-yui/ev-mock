using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowParameter : MonoBehaviour
{
    public enum StateType
    {
        Normal,
        Alert,
        Caution,
        Fighting,
    }

    [Header("◆[State : Normal(通常状態)]◆")]
    [SerializeField]
    float normalMoveSpeed = 1;
    public float NormalMoveSpeed => normalMoveSpeed;


    [Space(15)]
    [Header("◆[State : Alert(注意状態)]◆")]
    [SerializeField]
    float safeSoundLevelMax = 5;
    public float SafeSoundLevelMax => safeSoundLevelMax;

    [SerializeField]
    float alertTime = 1;
    public float AlertTime => alertTime;


    [Space(15)]
    [Header("◆[State : Caution(警戒状態)]◆")]
    [SerializeField]
    float cautionWaitTime = 0;
    public float CautionWaitTime => cautionWaitTime;

    [SerializeField]
    float cautionMoveSpeed = 1.5f;
    public float CautionMoveSpeed => cautionMoveSpeed;

    [SerializeField]
    float cautionRangeRadius = 1;


    [Space(15)]
    [Header("◆[State : Fighting(戦闘状態)]◆")]
    [SerializeField]
    float fightingWaitTime = 0;
    public float FightingWaitTime => fightingWaitTime;

    [SerializeField]
    float fightingMoveSpeed = 2;
    public float FightingMoveSpeed => fightingMoveSpeed;

    [SerializeField]
    float fightingRangeRadius = 0.5f;

    [SerializeField]
    float attackDistance = 0.3f;


    [Space(15)]
    [Header("[Other Settings]")]
    [SerializeField]
    bool isStaticState = false;

    [SerializeField]
    bool isAutoSpawn = true;

    // 初期位置
    public Vector3 InitialPosition { get; private set; } = Vector3.zero;

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        InitialPosition = transform.position;
    }
}
