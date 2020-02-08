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
    SphereCollider cautionRangeCollider = default;

    [SerializeField]
    SphereCollider fightingRangeCollider = default;

    [SerializeField]
    SectorCollider fieldOfView = default;

    [SerializeField]
    SectorCollider attackRange = default;

    [Space(15)]
    [Header("◆[State : Normal(通常状態)]◆")]
    [SerializeField]
    float normalMoveSpeed = 1;
    public float NormalMoveSpeed => normalMoveSpeed;

    [Space(10)]
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

    [Header("View Range Settings")]
    [SerializeField]
    float viewAngle = 120;
    [SerializeField]
    float viewDistance = 1;

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

    [Header("Attack Range Settings")]
    [SerializeField]
    float attackAngle = 120;
    [SerializeField]
    float attackDistance = 1;


    [Space(15)]
    [Header("◆[Special Action Settings]")]
    [SerializeField]
    SpecialActionType specialActionType = SpecialActionType.Normal;

    [Header("■HighSpeedAttack")]
    [SerializeField]
    float superFastMoveSpeed = 3;
    public float SuperFastMoveSpeed => superFastMoveSpeed;

    [SerializeField]
    float disappearDerayTime = 1;
    public float DisappearDerayTime => disappearDerayTime;

    [Header("■CameraFadeOutDisappear")]
    [SerializeField]
    float disappearDistance = 1;
    public float DisappearDistance => disappearDistance;

    [Header("■ApproachingDisappear")]
    [SerializeField]
    float approachingDistance = 1;
    public float ApproachingDistance => approachingDistance;

    [Space(15)]
    [Header("[Other Settings]")]
    [SerializeField]
    bool isStaticState = false;
    public bool IsStaticState => isStaticState;

    [SerializeField]
    bool isAutoSpawn = true;

    // 初期位置
    public Vector3 InitialPosition { get; private set; }

    /// <summary>
    /// 開始
    /// </summary>
    void Awake()
    {
        gameObject.SetActive(isAutoSpawn ? true : false); 

        // コライダーのサイズを反映
        cautionRangeCollider.radius = cautionRangeRadius;
        fightingRangeCollider.radius = fightingRangeRadius;
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

    void OnDrawGizmos()
    {
        // コライダーのサイズを反映
        cautionRangeCollider.radius = cautionRangeRadius;
        fightingRangeCollider.radius = fightingRangeRadius;
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
        Gizmos.DrawWireSphere(pos, randomRangeRadiusMin);
        Gizmos.DrawWireSphere(pos, randomRangeRadiusMax);
        Gizmos.color = Color.white;
    }
}
