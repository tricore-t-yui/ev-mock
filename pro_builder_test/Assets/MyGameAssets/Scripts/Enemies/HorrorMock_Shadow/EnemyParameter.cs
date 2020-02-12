using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class RangeParameter
{
    public EnemyParameter.StateType stateType = default;
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

    [SerializeField]
    StateColliderEvent appearRange = default;
    [SerializeField]
    StateColliderEvent cautionRange = default;
    [SerializeField]
    StateColliderEvent fightingRange = default;
    [Space(5)]
    [SerializeField]
    SectorCollider viewRange = default;
    [SerializeField]
    SectorCollider attackRange = default;

    [Header("◆[Range Settings]")]
    [ValidateInput("IsAlwaysAppearFunc", "常時姿が見える状態になっています。\nAppear(見える範囲)の設定は無効になります。", MessageType = InfoMessageType.Info)]
    [SerializeField]
    bool isAlwaysAppear = false;
    public bool IsAlwaysAppear => isAlwaysAppear;

    public bool IsAlwaysAppearFunc(bool value)
    {
        return !value;
    }

    [SerializeField]
    RangeParameter[] rangeParameters = new RangeParameter[3];
    public RangeParameter[] RangeParameters => rangeParameters;
    
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
    float attackDistance = 0.5f;
    public float AttackDistance => attackDistance;


    [Space(15)]
    [Header("◆[State : Normal(通常状態)]◆")]
    [SerializeField]
    float normalMoveSpeed = 1;
    public float NormalMoveSpeed => normalMoveSpeed;

    [SerializeField]
    NormalStateType normalStateType = NormalStateType.Waiting;
    public NormalStateType NormalState => normalStateType;

    [SerializeField]
    WandererType wandererType = WandererType.Route;
    public WandererType Wanderer => wandererType;

    [Space(5)]
    [SerializeField]
    RouteCheckPointList routeCheckPoints = default;
    List<Vector3> worldCheckPoint = new List<Vector3>();
    public IReadOnlyList<Vector3> RouteCheckPoints => worldCheckPoint;

    [SerializeField]
    float randomRangeRadiusMin = 0.5f;
    public float RandomRangeRadiusMin => randomRangeRadiusMin;

    [SerializeField]
    float randomRangeRadiusMax = 1;
    public float RandomRangeRadiusMax => randomRangeRadiusMax;

    [Space(5)]
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


    [Space(15)]
    [Header("◆[Debug Settings]")]
    [SerializeField]
    Color appearRangeColor = Color.green;

    [SerializeField]
    Color cautionRangeColor = Color.yellow;

    [SerializeField]
    Color fightingRangeColor = Color.red;

    [SerializeField]
    Color randomRangeColor = Color.cyan;

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
            System.Array.ForEach(rangeParameters, elem => elem.appear = 0);
        }

    }

    /// <summary>
    /// 範囲の変更
    /// </summary>
    /// <param name="type"></param>
    public void ChangeRangeRadius(StateType type)
    {
        RangeParameter rangeParameter = System.Array.Find(rangeParameters, elem => elem.stateType == type);
        appearRange.Radius = rangeParameter.appear;
        cautionRange.Radius = rangeParameter.caution;
        fightingRange.Radius = rangeParameter.fighting;
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = appearRangeColor;
        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.up, rangeParameters[0].appear);
        UnityEditor.Handles.color = cautionRangeColor;
        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.up, rangeParameters[0].caution);
        UnityEditor.Handles.color = fightingRangeColor;
        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.up, rangeParameters[0].fighting);
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
        Gizmos.color = randomRangeColor;
        Vector3 pos = (InitialPosition != Vector3.zero) ? InitialPosition : transform.position;
        UnityEditor.Handles.color = Color.cyan;
        UnityEditor.Handles.DrawWireDisc(pos, Vector3.up, randomRangeRadiusMin);
        UnityEditor.Handles.DrawWireDisc(pos, Vector3.up, randomRangeRadiusMax);
        Gizmos.color = Color.white;
    }
}
