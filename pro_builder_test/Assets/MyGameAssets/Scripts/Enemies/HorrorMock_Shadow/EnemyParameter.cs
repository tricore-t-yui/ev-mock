using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class RangeParameter
{
    [EnableIf("isTransparencyByDistance"), Range(0,100), Tooltip("透明状態から可視状態になる範囲")]
    public float appear = 4;
    [EnableIf("isTransparencyByDistance"), Range(0, 100), Tooltip("完全可視状態になる範囲")]
    public float fullAppear = 1;
    [Range(0.1f, 30), Tooltip("ベースの聴覚範囲")]
    public float noiseHear = 2.5f;
    [Range(0, 10), Tooltip("ノイズを聞いたときに決定されるターゲットポジションの分布誤差（メートル）")]
    public float noiseTargetPosRandomRange = 1.0f;
    [Range(0, 30), Tooltip("警戒状態時に加算延長される聴覚範囲")]
    public float noiseHearAddRangeCaution = 0.1f;
    [Range(0, 30), Tooltip("攻撃状態時に加算延長される聴覚範囲")]
    public float noiseHearAddRangeFighting = 0.1f;
    [Range(0.1f, 30), Tooltip("一定以上の音が鳴った時に直接攻撃状態に移行する範囲")]
    public float directDetect = 1;
    [Range(0.1f, 30), Tooltip("攻撃状態で、音が聞こえる限り攻撃状態を保つ範囲")]
    public float fighting = 1;

    [Space(10)]
    [Tooltip("距離によって透明度を変えるかどうかを設定します。" +
        "\nオフ：出現範囲に触れると指定の速度でじんわり現れる。" +
        "\nオン：出現範囲に触れた状態からプレイヤーの距離によって透明度が変わる。")]
    public bool isTransparencyByDistance = false;

    [Tooltip("姿が現れるときの速度を設定します。" +
        "\n値が小さいほど、ゆっくりじんわり現れます。")]
    [ShowIf("isTransparencyByDistance")]
    [Range(0, 1)]
    public float appearFadeTime = 0.1f;

    [Tooltip("透明度の最小値を設定します。" +
        "\nどんなに離れてもこの値より透明にはなりません。")]
    [ShowIf("isTransparencyByDistance")]
    [Range(0, 1)]
    public float transparencyMin = 0;

    [Tooltip("透明度の最大値を設定します。" +
        "\nどんなに近づいてもこの値よりは濃くなりません。")]
    [ShowIf("isTransparencyByDistance")]
    [Range(0, 1)]
    public float transparencyMax = 1;

    [Tooltip("フェードの反転を行うかどうかを設定します" +
        "\nオフ：出現範囲に触れると現れて、離れると消える。" +
        "\nオン：出現範囲に触れると消えて、離れると現れる。")]
    [ShowIf("isTransparencyByDistance")]
    public bool inverseTransparency = false;

    [Header("View Range(視野範囲(緑扇形)の設定。ステートごとに設定。上から通常、警戒、戦闘)")]

    [SerializeField]
    [Tooltip("視野範囲の角度を指定します。")]
    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, ShowItemCount = false, ShowPaging = false, DraggableItems = false)]
    public float[] viewAngles =
    {
        5,
        25,
        120,
    };

    [SerializeField]
    [Tooltip("視野範囲の距離を指定します。")]
    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, ShowItemCount = false, ShowPaging = false, DraggableItems = false)]
    public float[] viewDistances =
    {
        0.1f,
        3,
        20,
    };

    [SerializeField]
    [Header("Attack Range(攻撃ヒット範囲(赤扇形)の設定)")]
    [Tooltip("攻撃範囲の角度を指定します。")]
    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, ShowItemCount = false, ShowPaging = false, DraggableItems = false)]
    public float attackAngle = 120.0f;

    [SerializeField]
    [Tooltip("攻撃ヒット範囲の距離を指定します。")]
    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, ShowItemCount = false, ShowPaging = false, DraggableItems = false)]
    public float attackRange = 1.5f;
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
    TriggerEventDispatcher appearRange = default;

    [EnableIf("isActivate")]
    [SerializeField]
    TriggerEventDispatcher noiseHearRange = default;

    [EnableIf("isActivate")]
    [SerializeField]
    TriggerEventDispatcher fightingRange = default;

    [EnableIf("isActivate")]
    [SerializeField]
    TriggerEventDispatcher directDetectRange = default;

    [EnableIf("isActivate")]
    [Space(5)]
    [SerializeField]
    SectorCollider viewRange = default;

    [EnableIf("isActivate")]
    [SerializeField]
    SectorCollider attackRange = default;

    [SerializeField]
    [Tooltip("初期ステートを設定します。" +
        "\nこのステートから開始します。")]
    StateType initialState = StateType.Normal;
    public StateType InitialState => initialState;

    /////////////////////////////////////////////////
    [Title("[Range Parameters(範囲系パラメータ)]")]
    /////////////////////////////////////////////////
    [SerializeField]
    [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, Expanded = true, ShowItemCount = false, ShowPaging = false, DraggableItems = false)]
    RangeParameter rangeParameter;
    public RangeParameter RangeParameter => rangeParameter;

    /////////////////////////////////////////////////
    [Space(15)]
    [Title("[State : Normal(通常状態)]")]
    /////////////////////////////////////////////////
    [Tooltip("通常状態時の移動速度を設定します。")]
    [SerializeField]
    float normalMoveSpeed = 0.5f;
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
    float randomRangeRadiusMin = 3;
    public float RandomRangeRadiusMin => randomRangeRadiusMin;

    [ShowIf("IsNormalStateWanderer")]
    [Tooltip("ランダム徘徊時の最大範囲を設定します。")]
    [HideIf("IsWandererRoute")]
    [SerializeField]
    float randomRangeRadiusMax = 3.5f;
    public float RandomRangeRadiusMax => randomRangeRadiusMax;

    bool IsNormalStateWanderer()
    {
        return normalStateType == NormalStateType.Wanderer;
    }

     bool IsWandererRoute()
     {
        return wandererType == WandererType.Route;
     }

    [SerializeField, Range(0.1f, 100), Tooltip("直接攻撃状態に移行する音のレベル")]
    float directDetectSoundLevel = 6;
    public float DirectDetectSoundLevel => directDetectSoundLevel;

    public bool InverseTransparency => rangeParameter.inverseTransparency;
    public bool IsTransparencyByDistance => rangeParameter.isTransparencyByDistance;
    public float AppearFadeTime => rangeParameter.appearFadeTime;
    public float TransparencyMin => rangeParameter.transparencyMin;
    public float TransparencyMax => rangeParameter.transparencyMax;
    public float NoiseTargetPosRandomRange => rangeParameter.noiseTargetPosRandomRange;
    public float AttackRange => rangeParameter.attackRange;

    [Tooltip("最初に音を聞いたときに、警戒に移行するまでの時間を設定します。" +
        "\nこの時間までに警戒範囲から離れれば気づかれません。")]
    [SerializeField]
    float safeTime = 3;
    public float SafeTime => safeTime;

    [Tooltip("姿が見えていない状態で音を検知できるかどうかを設定します。\nオンの場合は姿が見えていなくてもプレイヤーが発した音を検知すれば警戒に移行して迫ってきます。")]
    [SerializeField]
    bool isDetectNoiseToTransparent = false;
    public bool IsDetectNoiseToTransparent => isDetectNoiseToTransparent;

    /////////////////////////////////////////////////
    [Space(15)]
    [Title("[State : Caution(警戒状態)]")]
    /////////////////////////////////////////////////
    [SerializeField]
    [Tooltip("警戒状態時の移動速度を設定します。" +
        "\nプレイヤーが発した音を聞いたときはこの速度で近づいてきます。")]
    float cautionMoveSpeed = 1;
    public float CautionMoveSpeed => cautionMoveSpeed;

    [Tooltip("警戒状態時の発見時待機時間")]
    [SerializeField]
    float cautionStartWaitTime = 1;
    public float CautionStartWaitTime => cautionStartWaitTime;

    [Tooltip("警戒状態時の見失った際の待機時間：最低値" +
        "\n音を聞いて近づいたが、結局プレイヤーを見つけられなかったときにこの時間でしばらく待機を行います。")]
    [SerializeField]
    float cautionEndWaitTimeMin = 1;
    public float CautionEndWaitTimeMin => cautionEndWaitTimeMin;

    [Tooltip("警戒状態時の見失った際の待機時間：最大値" +
        "\n音を聞いて近づいたが、結局プレイヤーを見つけられなかったときにこの時間でしばらく待機を行います。")]
    [SerializeField]
    float cautionEndWaitTimeMax = 4;
    public float CautionEndWaitTimeMax => cautionEndWaitTimeMax;

    [SerializeField]
    [Tooltip("プレイヤーを見失ったり音を聞きとれなくなったときに、もとの徘徊ルートや待機位置に戻ります。" +
        "\n戻る際にもとの位置との距離がこの値よりも遠かったときは瞬間移動を行います。")]
    float returnWarpDistance = 10;
    public float ReturnWarpDistance => returnWarpDistance;

    /////////////////////////////////////////////////
    [Space(15)]
    [Title("[State : Fighting(戦闘状態)]")]
    /////////////////////////////////////////////////
    [SerializeField]
    [Tooltip("戦闘状態時の移動速度を設定します。" +
        "\nこの速度でプレイヤーを追いかけます。")]
    float fightingMoveSpeed = 2;
    public float FightingMoveSpeed => fightingMoveSpeed;

    [SerializeField]
    [Tooltip("攻撃後の待機時間を設定します。" +
        "\nプレイヤーを攻撃したあとはこの時間でしばらく待機します。")]
    float attackedWaitTime = 3;
    public float AttackedWaitTime => attackedWaitTime;

    [SerializeField]
    [Range(0,100)]
    [Tooltip("プレイヤーに与えるダメージ値を設定します。")]
    float damage = 0;
    public float Damage => damage;

    [Header("Cry Sound Settings")]
    [SerializeField]
    CrySoundParameter cryParameter = default;
    public CrySoundParameter CryParameter => cryParameter;

    /////////////////////////////////////////////////
    [Space(15)]
    [Title("[Special Action Settings(特殊アクション)]")]
    /////////////////////////////////////////////////
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
    float disappearDistance = 4;
    public float DisappearDistance => disappearDistance;

    [SerializeField]
    [Tooltip("一定距離近づいたあと、しばらく待機してから消えるかどうかを設定します。" +
        "\nオンの場合は一定時間待機してから消えます。")]
    bool isDisappearWait = false;
    public bool IsDisappearWait => isDisappearWait; 

    [SerializeField]
    [ShowIf("isDisappearWait")]
    [Tooltip("一定距離近づいてから消えるまでの待機時間を設定します。" +
        "\nこの時間の経過後にその場から消えます。")]
    float disappearWaitTime = 2;
    public float DisappearWaitTime => disappearWaitTime;

    /////////////////////////////////////////////////
    [Space(15)]
    [Title("[Other Settings(その他)]")]
    /////////////////////////////////////////////////
    [SerializeField]
    [Tooltip("敵の状態遷移を行うかどうかを設定します。" +
        "\nこのフラグがオンの場合は状態遷移が一切行われなくなります。")]
    bool isStaticState = false;
    public bool IsStaticState => isStaticState;

    [SerializeField]
    [Tooltip("このフラグがONだと攻撃せずに意味もなくついてきます")]
    bool dontAttack = false;
    public bool DontAttack => dontAttack;

    [SerializeField]
    [Tooltip("ゲーム開始時に自動でスポーンさせるか設定します。" +
        "\nオフの場合は自動では出現しなくなります。")]
    bool isInitialAutoSpawn = true;

    // 初期位置
    public Vector3 InitialPosition { get; private set; }
    public Quaternion InitialRotation { get; private set; }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        if (rangeParameter.viewAngles[0] < 0.01f || rangeParameter.viewDistances[0] < 0.01f)
        {
            viewRange.gameObject.SetActive(false);
        }
        if (rangeParameter.attackAngle < 0.01f || rangeParameter.attackRange < 0.01f)
        {
            attackRange.gameObject.SetActive(false);
        }
        InitialPosition = transform.position;
        InitialRotation = transform.rotation;

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
    public void ChangeRangeRadius(StateType type)
    {
        appearRange.Radius = RangeParameter.appear;
        fightingRange.Radius = RangeParameter.fighting;
        directDetectRange.Radius = RangeParameter.directDetect;

        // 状態によって聴覚範囲のみレンジ加算を行う
        if(type == StateType.Caution)
        {
            noiseHearRange.Radius = RangeParameter.noiseHear + RangeParameter.noiseHearAddRangeCaution;
        }
        else if (type == StateType.Fighting)
        {
            noiseHearRange.Radius = RangeParameter.noiseHear + RangeParameter.noiseHearAddRangeFighting;
        }
        else
        {
            noiseHearRange.Radius = RangeParameter.noiseHear;
        }
#if UNITY_EDITOR
        currentType = type;
#endif

        viewRange.Angle = rangeParameter.viewAngles[(int)type];
        viewRange.Distance = rangeParameter.viewDistances[(int)type];
        attackRange.Angle = rangeParameter.attackAngle;
        attackRange.Distance = rangeParameter.attackRange;
    }

#if UNITY_EDITOR && DEBUG
    public bool DrawGizmo { get; set; }
    StateType currentType = StateType.Normal;
    private void OnDrawGizmos()
    {
        if (!DrawGizmo) return;
        UnityEditor.Handles.color = new Color(0,1,0,0.5f);
        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.up, RangeParameter.appear);
        UnityEditor.Handles.color = new Color(1, 1, 0.3f, 0.5f);
        if(currentType == StateType.Caution)
            UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.up, RangeParameter.noiseHear + RangeParameter.noiseHearAddRangeCaution);
        else if (currentType == StateType.Fighting)
            UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.up, RangeParameter.noiseHear + RangeParameter.noiseHearAddRangeFighting);
        else
            UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.up, RangeParameter.noiseHear);
        UnityEditor.Handles.color = new Color(1, 1, 0, 0.5f);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, RangeParameter.directDetect);
        UnityEditor.Handles.color = new Color(1, 0, 0, 0.5f);
        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.up, RangeParameter.fighting);
        UnityEditor.Handles.color = Color.white;

        // ルートを線で描画
        if (normalStateType == NormalStateType.Wanderer && wandererType == WandererType.Route)
        {
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
        }

        // ランダム範囲を描画
        if (normalStateType == NormalStateType.Wanderer && wandererType == WandererType.Random)
        {
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
#endif
}
