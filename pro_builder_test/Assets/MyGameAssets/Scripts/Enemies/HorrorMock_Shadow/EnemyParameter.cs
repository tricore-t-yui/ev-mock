﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class RangeParameter
{
    public RangeParameter(EnemyParameter.StateType type)
    {
        stateType = type;
    }

    [EnableIf("isActivate")]
    public EnemyParameter.StateType stateType = default;
    bool isActivate = false;
    [Range(0,100)]
    public float appear = 0;
    [Range(0.1f, 100)]
    public float caution = 0;
    [Range(0.1f, 100)]
    public float fighting = 0;
}

public class EnemyParameter : MonoBehaviour
{
    // 状態の種類
    public enum StateType
    {
        Normal,     // 通常状態
        Caution,    // 警戒状態
        Fighting,   // 戦闘状態
    }

    // 通常状態の種類
    public enum NormalStateType
    {
        Waiting,    // 待機状態
        Wanderer,   // 徘徊状態
    }

    // 徘徊の種類
    public enum WandererType
    {
        Route,      // ルート
        Random,     // ランダム
    }

    bool isActivate = false;

    [EnableIf("isActivate")]
    [SerializeField]
    StateColliderEvent appearRange = default;

    [EnableIf("isActivate")]
    [SerializeField]
    StateColliderEvent cautionRange = default;

    [EnableIf("isActivate")]
    [SerializeField]
    StateColliderEvent fightingRange = default;

    [EnableIf("isActivate")]
    [Space(5)]
    [SerializeField]
    SectorCollider viewRange = default;

    [EnableIf("isActivate")]
    [SerializeField]
    SectorCollider attackRange = default;

    [Title("[Range Parameters(範囲系パラメータ)]")]
    [Tooltip("距離に限らず常に姿が見えるようになります。")]
    [ValidateInput("IsAlwaysAppearFunc", "常に姿が見える状態になっています。\nAppear(見える範囲)の設定は無効になります。", MessageType = InfoMessageType.Info)]
    [SerializeField]
    bool isAlwaysAppear = false;
    public bool IsAlwaysAppear => isAlwaysAppear;

    public bool IsAlwaysAppearFunc(bool value)
    {
        return !value;
    }

    [SerializeField]
    [ListDrawerSettings(HideAddButton = true,HideRemoveButton = true,Expanded = true,ShowItemCount = false,ShowPaging = false,DraggableItems = false)]
    RangeParameter[] stateRanges = new RangeParameter[3]
    {
        new RangeParameter(StateType.Normal),
        new RangeParameter(StateType.Caution),
        new RangeParameter(StateType.Fighting),
    };
    public RangeParameter[] StateRanges => stateRanges;

    [Header("View Range")]
    [Tooltip("視野範囲の角度を指定します。")]
    [SerializeField]
    float viewAngle = 120;
    public float ViewAngle => viewAngle;
    [Tooltip("視野範囲の距離を指定します。")]
    [SerializeField]
    float viewDistance = 1;
    public float ViewDistance => viewDistance;

    [Header("Attack Range")]
    [Tooltip("攻撃範囲の角度を指定します。")]
    [SerializeField]
    float attackAngle = 120;
    public float AttackAngle => attackAngle;
    [Tooltip("攻撃範囲の距離を指定します。")]
    [SerializeField]
    float attackDistance = 0.5f;
    public float AttackDistance => attackDistance;


    [Space(15)]
    [Title("[State : Normal(通常状態)]")]
    [Tooltip("通常状態時の移動速度を設定します。")]
    [SerializeField]
    float normalMoveSpeed = 1;
    public float NormalMoveSpeed => normalMoveSpeed;

    [Tooltip("通常状態時の種類を指定します。待機型or徘徊型")]
    [SerializeField]
    NormalStateType normalStateType = NormalStateType.Waiting;
    public NormalStateType NormalState => normalStateType;

    [Space(10)]
    [Tooltip("徘徊の種類を指定します。")]
    [ShowIf("IsNormalStateWanderer")]
    [SerializeField]
    WandererType wandererType = WandererType.Route;
    public WandererType Wanderer => wandererType;

    [ShowIf("IsNormalStateWanderer")]
    [ShowIf("IsWandererRoute")]
    [SerializeField]
    RouteCheckPointList routeCheckPoints = default;
    List<Vector3> worldCheckPoint = new List<Vector3>();
    public IReadOnlyList<Vector3> RouteCheckPoints => worldCheckPoint;

    [ShowIf("IsNormalStateWanderer")]
    [HideIf("IsWandererRoute")]
    [SerializeField]
    float randomRangeRadiusMin = 0.5f;
    public float RandomRangeRadiusMin => randomRangeRadiusMin;

    [ShowIf("IsNormalStateWanderer")]
    [HideIf("IsWandererRoute")]
    [SerializeField]
    float randomRangeRadiusMax = 1;
    public float RandomRangeRadiusMax => randomRangeRadiusMax;

    bool IsNormalStateWanderer()
    {
        return normalStateType == NormalStateType.Wanderer;
    }

     bool IsWandererRoute()
     {
        return wandererType == WandererType.Route;
     }

    [Space(10)]
    [SerializeField]
    [Range(0, 1)]
    float appearFadeTime = 0.1f;
    public float AppearFadeTime => appearFadeTime;

    [SerializeField]
    float safeSoundLevelMax = 5;
    public float SafeSoundLevelMax => safeSoundLevelMax;

    [SerializeField]
    float safeTime = 1;
    public float SafeTime => safeTime;

    [SerializeField]
    bool isDetectNoiseToTransparent = false;
    public bool IsDetectNoiseToTransparent => isDetectNoiseToTransparent;


    [Space(15)]
    [Title("[State : Caution(警戒状態)]")]
    [SerializeField]
    float cautionWaitTime = 0;
    public float CautionWaitTime => cautionWaitTime;

    [SerializeField]
    float cautionMoveSpeed = 1.5f;
    public float CautionMoveSpeed => cautionMoveSpeed;

    [SerializeField]
    float returnWarpDistance = 1;
    public float ReturnWarpDistance => returnWarpDistance;

    [Space(15)]
    [Title("[State : Fighting(戦闘状態)]")]
    [SerializeField]
    float fightingWaitTime = 0;
    public float FightingWaitTime => fightingWaitTime;

    [SerializeField]
    float attackedWaitTime = 0;
    public float AttackedWaitTime => attackedWaitTime;

    [SerializeField]
    float fightingMoveSpeed = 2;
    public float FightingMoveSpeed => fightingMoveSpeed;

    [SerializeField]
    [Range(0,100)]
    float damage = 0;
    public float Damage => damage;


    [Space(15)]
    [Title("[Special Action Settings(特殊アクション)]")]

    [SerializeField]
    bool isAttackedDisappear = false;
    public bool IsAttackedDisappear => isAttackedDisappear;

    [SerializeField]
    bool isApproachedDisappear = false;
    public bool IsApproachedDisappear => isApproachedDisappear;

    [SerializeField]
    bool isCameraFadeOutDisappear = false;
    public bool IsCameraFadeOutDisappear => isCameraFadeOutDisappear;

    [SerializeField]
    bool isRespawn = false;
    public bool IsRespawn => isRespawn;

    [SerializeField]
    float disappearDistance = 0;
    public float DisappearDistance => disappearDistance;


    [Space(15)]
    [Title("[Other Settings(その他)]")]
    [SerializeField]
    bool isStaticState = false;
    public bool IsStaticState => isStaticState;

    [SerializeField]
    bool isInitialAutoSpawn = true;

    // 初期位置
    public Vector3 InitialPosition { get; private set; }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        InitialPosition = transform.position;

        if (routeCheckPoints != null)
        {
            foreach (Vector2 checkPoint in routeCheckPoints)
            {
                Vector2 newPos = checkPoint + new Vector2(transform.position.x, transform.position.z);
                Vector3 newWorldPos = new Vector3(newPos.x, transform.position.y, newPos.y);
                worldCheckPoint.Add(newWorldPos);
            }
        }

        // 常時姿が見える状態
        if (isAlwaysAppear)
        {
            // 全ての見える範囲をオニする
            System.Array.ForEach(stateRanges, elem => elem.appear = 0);
        }

    }

    /// <summary>
    /// 範囲の変更
    /// </summary>
    /// <param name="type"></param>
    public void ChangeRangeRadius(StateType type)
    {
        RangeParameter rangeParameter = System.Array.Find(stateRanges, elem => elem.stateType == type);
        appearRange.Radius = rangeParameter.appear;
        cautionRange.Radius = rangeParameter.caution;
        fightingRange.Radius = rangeParameter.fighting;
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = new Color(0,1,0,0.5f);
        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.up, stateRanges[0].appear);
        UnityEditor.Handles.color = new Color(1, 1, 0, 0.5f);
        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.up, stateRanges[0].caution);
        UnityEditor.Handles.color = new Color(1, 0, 0, 0.5f);
        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.up, stateRanges[0].fighting);
        UnityEditor.Handles.color = Color.white;

        viewRange.Angle      = viewAngle;
        viewRange.Distance   = viewDistance;
        attackRange.Angle    = attackAngle;
        attackRange.Distance = attackDistance;

        // ルートを線で描画
        if (routeCheckPoints != null)
        {
            for (int i = 0; i < routeCheckPoints.Count; i++)
            {
                Vector3 checkPoint = Vector3.zero;
                Vector3 nextCheckPoint = Vector3.zero;
                if (!UnityEditor.EditorApplication.isPlaying)
                {
                    int nextIndex = (i == routeCheckPoints.Count - 1) ? 0 : i + 1;
                    checkPoint = new Vector3(routeCheckPoints[i].x, transform.position.y, routeCheckPoints[i].y);
                    nextCheckPoint = new Vector3(routeCheckPoints[nextIndex].x, transform.position.y, routeCheckPoints[nextIndex].y);
                    checkPoint += new Vector3(transform.position.x, 0, transform.position.z);
                    nextCheckPoint += new Vector3(transform.position.x, 0, transform.position.z);
                }
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(checkPoint, nextCheckPoint);
                Gizmos.DrawWireSphere(checkPoint, 0.1f);
                Gizmos.color = Color.white;
            }
        }

        // ランダム範囲を描画
        if (randomRangeRadiusMin > randomRangeRadiusMax)
        {
            randomRangeRadiusMin = randomRangeRadiusMax;
        }
        if (randomRangeRadiusMax < randomRangeRadiusMin)
        {
            randomRangeRadiusMax = randomRangeRadiusMin;
        }
        Gizmos.color = Color.cyan;
        Vector3 pos = (InitialPosition != Vector3.zero) ? InitialPosition : transform.position;
        UnityEditor.Handles.color = Color.cyan;
        UnityEditor.Handles.DrawWireDisc(pos, Vector3.up, randomRangeRadiusMin);
        UnityEditor.Handles.DrawWireDisc(pos, Vector3.up, randomRangeRadiusMax);
        Gizmos.color = Color.white;
    }
}
