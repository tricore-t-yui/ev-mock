using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RangeParameter
{
    [Header("State Range")]
    public ShadowParameter.StateType stateType = default;
    public float appearRangeRadius = 4;
    public float cautionRangeRadius = 3;
    public float fightingRangeRadius = 2;
}

public class ShadowParameter : MonoBehaviour
{
    public enum StateType
    {
        Normal,
        Caution,
        Fighting,
    }

    public enum NormalStateType
    {
        Waiting,
        Wanderer,
    }

    public enum WandererType
    {
        Route,
        Random,
    }

    public enum SpecialActionType
    {
        Normal,
        HighSpeedAttack,
        CameraFadeOutDisappear,
        ApproachingDisappear,
    }

    [SerializeField]
    SphereCollider appearRangeCollider = default;

    [SerializeField]
    SphereCollider cautionRangeCollider = default;

    [SerializeField]
    SphereCollider fightingRangeCollider = default;

    [SerializeField]
    SectorCollider fieldOfView = default;

    [SerializeField]
    SectorCollider attackRange = default;


    [Header("◆[Range Settings]")]
    [SerializeField]
    RangeParameter[] rangeParameters = new RangeParameter[3];

    [Header("View Range")]
    [SerializeField]
    float viewAngle = 120;
    public float ViewAngle => viewAngle;
    [SerializeField]
    float viewDistance = 1;
    public float ViewDistance => viewDistance;

    [Header("Attack Range")]
    [SerializeField]
    float attackAngle = 120;
    public float AttackAngle => attackAngle;
    [SerializeField]
    float attackDistance = 1;
    public float AttackDistance => attackDistance;

    [Space(15)]
    [Header("◆[State : Normal(通常状態)]◆")]
    [SerializeField]
    float normalMoveSpeed = 1;
    public float NormalMoveSpeed => normalMoveSpeed;

    [Space(5)]
    [SerializeField]
    NormalStateType normalStateType = NormalStateType.Waiting;
    public NormalStateType NormalState => normalStateType;

    [SerializeField]
    WandererType wandererType = WandererType.Route;
    public WandererType Wanderer => wandererType;

    [SerializeField]
    RouteCheckPointList routeCheckPoints = default;
    List<Vector2> worldCheckPoint = new List<Vector2>();
    public IReadOnlyList<Vector2> RouteCheckPoints => worldCheckPoint;

    [SerializeField]
    float randomRangeRadiusMin = 0.5f;
    public float RandomRangeRadiusMin => randomRangeRadiusMin;

    [SerializeField]
    float randomRangeRadiusMax = 1;
    public float RandomRangeRadiusMax => randomRangeRadiusMax;

    [SerializeField]
    [Range(0, 1)]
    float appearFadeTime = 0.1f;
    public float AppearFadeTime => appearFadeTime;

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


    [Space(15)]
    [Header("◆[State : Fighting(戦闘状態)]◆")]
    [SerializeField]
    float fightingWaitTime = 0;
    public float FightingWaitTime => fightingWaitTime;

    [SerializeField]
    float fightingMoveSpeed = 2;
    public float FightingMoveSpeed => fightingMoveSpeed;


    [Space(15)]
    [Header("◆[Special Action Settings]")]

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
    [Header("[Other Settings]")]
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
        gameObject.SetActive(isInitialAutoSpawn ? true : false);

        // コライダーのサイズを反映
        ChangeStateRangeRadius(StateType.Normal);
        fieldOfView.Angle = viewAngle;
        fieldOfView.Distance = viewDistance;
        attackRange.Angle = attackAngle;
        attackRange.Distance = attackDistance;

        InitialPosition = transform.position;

        foreach(Vector2 checkPoint in routeCheckPoints)
        {
            worldCheckPoint.Add(checkPoint + new Vector2(transform.position.x, transform.position.z));
        }
    }

    /// <summary>
    /// ステートごとの範囲の変更
    /// </summary>
    /// <param name="type"></param>
    public void ChangeStateRangeRadius(StateType type)
    {
        appearRangeCollider.radius = rangeParameters[(int)type].appearRangeRadius;
        cautionRangeCollider.radius = rangeParameters[(int)type].cautionRangeRadius;
        fightingRangeCollider.radius = rangeParameters[(int)type].fightingRangeRadius;
    }

    void OnDrawGizmos()
    {
        // コライダーのサイズを反映
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            ChangeStateRangeRadius(StateType.Normal);
        }
        fieldOfView.Angle = viewAngle;
        fieldOfView.Distance = viewDistance;
        attackRange.Angle = attackAngle;
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
                    int nextIndex = (i == routeCheckPoints.Count - 1) ? 0 : i + 1 ;
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
        UnityEditor.Handles.DrawWireDisc(pos,Vector3.up, randomRangeRadiusMin);
        UnityEditor.Handles.DrawWireDisc(pos,Vector3.up, randomRangeRadiusMax);
        Gizmos.color = Color.white;
        Gizmos.color = Color.white;
    }
}
