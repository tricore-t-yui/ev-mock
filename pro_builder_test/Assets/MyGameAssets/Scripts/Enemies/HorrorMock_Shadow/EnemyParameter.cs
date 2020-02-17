using System.Collections;
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
    [SerializeField]
    [ListDrawerSettings(HideAddButton = true,HideRemoveButton = true,Expanded = true,ShowItemCount = false,ShowPaging = false,DraggableItems = false)]
    RangeParameter[] stateRanges = new RangeParameter[3]
    {
        new RangeParameter(StateType.Normal),
        new RangeParameter(StateType.Caution),
        new RangeParameter(StateType.Fighting),
    };
    public RangeParameter[] StateRanges => stateRanges;

    [Header("View Range(視野範囲(緑扇形)の設定)")]
    [Tooltip("視野範囲の角度を指定します。")]
    [SerializeField]
    float viewAngle = 120;
    public float ViewAngle => viewAngle;
    [Tooltip("視野範囲の距離を指定します。")]
    [SerializeField]
    float viewDistance = 1;
    public float ViewDistance => viewDistance;

    [Header("Attack Range(戦闘範囲(赤扇形)の設定)")]
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
    [ShowIf("IsNormalStateWanderer")]
    [Tooltip("徘徊の種類を指定します。ルート徘徊orランダム徘徊")]
    [SerializeField]
    WandererType wandererType = WandererType.Route;
    public WandererType Wanderer => wandererType;

    [ShowIf("IsNormalStateWanderer")]
    [Tooltip("ルート徘徊時のチェックポイントを設定します。")]
    [ShowIf("IsWandererRoute")]
    [SerializeField]
    RouteCheckPointList routeCheckPoints = default;
    List<Vector3> worldCheckPoint = new List<Vector3>();
    public IReadOnlyList<Vector3> RouteCheckPoints => worldCheckPoint;

    [ShowIf("IsNormalStateWanderer")]
    [Tooltip("ランダム徘徊時の最小範囲を設定します。")]
    [HideIf("IsWandererRoute")]
    [SerializeField]
    float randomRangeRadiusMin = 0.5f;
    public float RandomRangeRadiusMin => randomRangeRadiusMin;

    [ShowIf("IsNormalStateWanderer")]
    [Tooltip("ランダム徘徊時の最大範囲を設定します。")]
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
    [Tooltip("フェードの反転を行うかどうかを設定します" +
        "\nオフ：出現範囲に触れると現れて、離れると消える。" +
        "\nオン：出現範囲に触れると消えて、離れると現れる。")]
    bool inverse = false;
    public bool Inverse => inverse;

    [SerializeField]
    [Tooltip("距離によって透明度を変えるかどうかを設定します。" +
        "\nオフ：出現範囲に触れると指定の速度でじんわり現れる。" +
        "\nオン：出現範囲に触れた状態からプレイヤーの距離によって透明度が変わる。")]
    bool isTransparencyByDistance = false;
    public bool IsTransparencyByDistance => isTransparencyByDistance;

    [Tooltip("姿が現れるときの速度を設定します。" +
        "\n値が小さいほど、ゆっくりじんわり現れます。")]
    [HideIf("isTransparencyByDistance")]
    [SerializeField]
    [Range(0, 1)]
    float appearFadeTime = 0.1f;
    public float AppearFadeTime => appearFadeTime;

    [SerializeField]
    [Tooltip("透明度の最小値を設定します。" +
        "\nどんなに離れてもこの値より透明にはなりません。")]
    [ShowIf("isTransparencyByDistance")]
    [Range(0, 1)]
    float transparencyMin = 0.5f;
    public float TransparencyMin => transparencyMin;

    [SerializeField]
    [Tooltip("透明度の最大値を設定します。" +
        "\nどんなに近づいてもこの値よりは濃くなりません。")]
    [ShowIf("isTransparencyByDistance")]
    [Range(0, 1)]
    float transparencyMax = 1;
    public float TransparencyMax => transparencyMax;

    [Space(10)]
    [Tooltip("最初に音を聞いたときに、この値よりも大きなレベルの音を鳴らすと即警戒に移行します。")]
    [SerializeField]
    float safeSoundLevelMax = 5;
    public float SafeSoundLevelMax => safeSoundLevelMax;

    [Tooltip("最初に音を聞いたときに、警戒に移行するまでの時間を設定します。" +
        "\nこの時間までに警戒範囲から離れれば気づかれません。")]
    [SerializeField]
    float safeTime = 1;
    public float SafeTime => safeTime;

    [Tooltip("姿が見えていない状態で音を検知できるかどうかを設定します。\nオンの場合は姿が見えていなくてもプレイヤーが発した音を検知すれば警戒に移行して迫ってきます。")]
    [SerializeField]
    bool isDetectNoiseToTransparent = false;
    public bool IsDetectNoiseToTransparent => isDetectNoiseToTransparent;


    [Space(15)]
    [Title("[State : Caution(警戒状態)]")]
    [Tooltip("警戒状態時の待機時間を設定します。" +
        "\n音を聞いて近づいたが、結局プレイヤーを見つけられなかったときにこの時間でしばらく待機を行います。")]
    [SerializeField]
    float cautionWaitTime = 0;
    public float CautionWaitTime => cautionWaitTime;

    [SerializeField]
    [Tooltip("警戒状態時の移動速度を設定します。" +
        "\nプレイヤーが発した音を聞いたときはこの速度で近づいてきます。")]
    float cautionMoveSpeed = 1.5f;
    public float CautionMoveSpeed => cautionMoveSpeed;

    [SerializeField]
    [Tooltip("プレイヤーを見失ったり音を聞きとれなくなったときに、もとの徘徊ルートや待機位置に戻ります。" +
        "\n戻る際にもとの位置との距離がこの値よりも遠かったときは瞬間移動を行います。")]
    float returnWarpDistance = 1;
    public float ReturnWarpDistance => returnWarpDistance;

    [Space(15)]
    [Title("[State : Fighting(戦闘状態)]")]
    [SerializeField]
    [Tooltip("戦闘状態時の待機時間を設定します。" +
        "\nプレイヤーを途中で見失ったときにこの時間で待機を行います。")]
    float fightingWaitTime = 0;
    public float FightingWaitTime => fightingWaitTime;

    [SerializeField]
    [Tooltip("攻撃後の待機時間を設定します。" +
        "\nプレイヤーを攻撃したあとはこの時間でしばらく待機します。")]
    float attackedWaitTime = 0;
    public float AttackedWaitTime => attackedWaitTime;

    [SerializeField]
    [Tooltip("戦闘状態時の移動速度を設定します。" +
        "\nこの速度でプレイヤーを追いかけます。")]
    float fightingMoveSpeed = 2;
    public float FightingMoveSpeed => fightingMoveSpeed;    

    [SerializeField]
    [Range(0,100)]
    [Tooltip("プレイヤーに与えるダメージ値を設定します。")]
    float damage = 0;
    public float Damage => damage;

    [Header("Cry Sound Settings")]
    [SerializeField]
    CrySoundParameter cryParameter = default;
    public CrySoundParameter CryParameter => cryParameter;


    [Space(15)]
    [Title("[Special Action Settings(特殊アクション)]")]

    [SerializeField]
    [Tooltip("このフラグがオンの場合、攻撃してしばらく待機した後に消えます。")]
    bool isAttackedDisappear = false;
    public bool IsAttackedDisappear => isAttackedDisappear;

    [SerializeField]
    [Tooltip("このフラグがオンの場合、一定距離近づくと消えます。")]
    bool isApproachedDisappear = false;
    public bool IsApproachedDisappear => isApproachedDisappear;

    [SerializeField]
    [Tooltip("このフラグがオンの場合、カメラから外れた瞬間消えます。" +
        "\n注意：Unityの仕様でSceneビューとGameビューの両方のカメラから外れたときに動作します。" +
        "\nなので、対象の敵がSceneビューから見えない状態で試すようにしてください。")]
    bool isCameraFadeOutDisappear = false;
    public bool IsCameraFadeOutDisappear => isCameraFadeOutDisappear;

    [SerializeField]
    [Tooltip("上記のアクションの消えたときに初期位置にリスポーンさせるかどうかを設定します。")]
    bool isRespawn = false;
    public bool IsRespawn => isRespawn;

    [SerializeField]
    [Tooltip("どのくらい近づいたら消えるかを設定します。" +
        "\n敵にこの距離よりも近づくと消えます。")]
    float disappearDistance = 0;
    public float DisappearDistance => disappearDistance;

    [Space(15)]
    [Title("[Other Settings(その他)]")]
    [SerializeField]
    [Tooltip("敵の状態遷移を行うかどうかを設定します。" +
        "\nこのフラグがオンの場合は状態遷移が一切行われなくなります。")]
    bool isStaticState = false;
    public bool IsStaticState => isStaticState;

    [SerializeField]
    [Tooltip("ゲーム開始時に自動でスポーンさせるか設定します。" +
        "\nオフの場合は自動では出現しなくなります。")]
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
