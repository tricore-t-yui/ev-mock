using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("時間発生、当たり発生、視線発生によって、直下のEventObjectをランダムで起動させる。" +
"\nイベントオブジェクトはデフォルト非表示にすること")]
public class EventTrigger : MonoBehaviour
{
    enum TriggerKind
    {
        [LabelText("当たり判定")]
        Collide,
        [LabelText("視線当たり判定")]
        ViewCollide,
        [LabelText("時間")]
        Time,
        [LabelText("インタラクト")]
        Interact
    }

#if DEBUG
    [SerializeField, LabelText("デバッグ：トリガー範囲表示")]
    bool debugDrawRange = true;
#endif

    [SerializeField, LabelText("発動種別")]
    TriggerKind kind = TriggerKind.Collide;
    public bool IsViewCollide { get { return kind == TriggerKind.ViewCollide; } }

    [SerializeField, Range(0.1f, 100.0f), EnableIf("kind", TriggerKind.ViewCollide), LabelText("視線レイの有効距離")]
    float viewColideRange = 5.0f;
    public float ViewColideRange => viewColideRange;

    [SerializeField, LabelText("発動に必要なスイッチ名(カンマ区切り複数可)")]
    string needSwitchName = "";

    [SerializeField, LabelText("発動できなくなるスイッチ名(マウスオーバーで詳細"), Tooltip("ここに記入されているスイッチがオンだと発動しなくなる。" +
    "\n例：Soundと記入すると、他のトリガーによってSoundスイッチがONになった場合はこのトリガーが発動しなくなる(カンマ区切り複数可)")]
    string banSwitch = "";

    [SerializeField, LabelText("発動でONになるスイッチ名(カンマ区切り複数可)")]
    string enableSwitchName = "";

    [SerializeField, LabelText("発動でOFFになるスイッチ名(カンマ区切り複数可)")]
    string disableSwitchName = "";

    [SerializeField, LabelText("発動チャンス回数(ゼロなら無限)")]
    int chanceNum = 0;

    [SerializeField, LabelText("発動回数(ゼロなら無限)")]
    int triggerNum = 0;

    [SerializeField, Range(0.1f, 300), LabelText("発動クールダウンタイム")]
    float triggerCoolDown = 0.5f;

    [SerializeField, Range(1, 100), LabelText("イベントの出現確率")]
    int appearRate = 100;
    public int AppearRate => appearRate;

    [SerializeField, Range(-100, 100), LabelText("抽選が外れるたびに加算/減算される出現確率")]
    int addAppearRate = 0;

    List<EventObject> eventObjectList = new List<EventObject>();
    int chanceCount = 0;
    int triggerCount = 0;

    EventTriggerGroup group;
    float lastEventEndTime;
    bool isDoingEvent;

    string[] banSwitchNameList;
    string[] needSwitchNameList;
    public string[] EnableSwitchNameList    { get; private set; }
    public string[] DisableSwitchNameList   { get; private set; }

    private void Awake()
    {
        banSwitchNameList = new string[0];
        needSwitchNameList = new string[0];
        EnableSwitchNameList = new string[0];
        DisableSwitchNameList = new string[0];
        if (!string.IsNullOrEmpty(needSwitchName)) needSwitchNameList = needSwitchName.Split(',');
        if (!string.IsNullOrEmpty(banSwitch)) banSwitchNameList = banSwitch.Split(',');
        if (!string.IsNullOrEmpty(enableSwitchName)) EnableSwitchNameList = enableSwitchName.Split(',');
        if (!string.IsNullOrEmpty(disableSwitchName)) DisableSwitchNameList = disableSwitchName.Split(',');
    }

    void Start()
    {
        // HACK: mockだから親がトリガー確定
        group = transform.parent.GetComponent<EventTriggerGroup>();

        foreach (var item in GetComponentsInChildren<EventObject>(true))
        {
            item.gameObject.SetActive(false);
            eventObjectList.Add(item);
        }
        lastEventEndTime = Time.timeSinceLevelLoad;
        if (kind != TriggerKind.Time)
        {
            lastEventEndTime -= triggerCoolDown;    // 初回はクールダウンなしで発生できるようにするため
        }
    }
    private void Update()
    {
        // 時間発動の場合はチェック
        if (kind == TriggerKind.Time)
        {
            if (!isDoingEvent && Time.timeSinceLevelLoad - lastEventEndTime > triggerCoolDown)
            {
                CheckAndExecuteEvent();
            }
        }
    }

    /// <summary>
    /// 発生確率加算
    /// </summary>
    public void OnAddAppearRate(int add)
    {
        appearRate += add;
    }

    /// <summary>
    ///  自身のスイッチが有効かどうか
    /// </summary>
    /// <returns></returns>
    bool IsEnableSwitch()
    {
        if (string.IsNullOrEmpty(needSwitchName) || group.IsSwitchOn(needSwitchNameList))
        {
            if (string.IsNullOrEmpty(banSwitch) || !group.IsSwitchOn(banSwitchNameList))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// イベント確率計算して実行
    /// </summary>
    void CheckAndExecuteEvent()
    {
        // スイッチあったらスイッチが入っている場合のみ加算
        if(!IsEnableSwitch())
        {
            return;
        }
        var rate = Random.Range(0.0f, 1.0f);
        if (rate > 1.0f - (appearRate / 100.0f))
        {
            EventObject triggerObject;
            if (eventObjectList.Count == 1)
            {
                triggerObject = eventObjectList[0];
            }
            else
            {
                var idx = Random.Range(0, eventObjectList.Count * 100 - 1) / 100;
                triggerObject = eventObjectList[idx];
            }
            triggerObject.SetEndCallback(OnEndEventObject);
            isDoingEvent = true;
            triggerObject.gameObject.SetActive(true);

            // グループに起動通知
            group.OnTriggered(this);
        }
        else
        {
            ++chanceCount;

            if (chanceNum != 0 && chanceCount >= chanceNum)
            {
                DestroyObject();
            }
            else
            {
                appearRate += addAppearRate;
                if (appearRate > 100)
                {
                    appearRate = 100;
                }
                else if (appearRate < 0)
                {
                    Destroy(gameObject);
                }
            }
            lastEventEndTime = Time.timeSinceLevelLoad;
        }
    }

    /// <summary>
    /// イベント終了コールバック
    /// </summary>
    void OnEndEventObject()
    {
        isDoingEvent = false;
        lastEventEndTime = Time.timeSinceLevelLoad;
        ++triggerCount;
        if (triggerNum != 0 && triggerCount >= triggerNum)
        {
            DestroyObject();
        }
    }

    /// <summary>
    /// オブジェクトが死ぬべきときに呼ばれる
    /// </summary>
    void DestroyObject()
    {
        group.OnTriggerDestroy(this);
        Destroy(gameObject);
    }

    /// <summary>
    ///  当たり判定
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player"
        && other.gameObject.layer == LayerMask.NameToLayer("Player")
        && !isDoingEvent
        && (kind == TriggerKind.Collide || kind == TriggerKind.Interact)
        && Time.timeSinceLevelLoad - lastEventEndTime >= triggerCoolDown
        )
        {
            CheckAndExecuteEvent();
        }
    }

    /// <summary>
    /// 視線発動
    /// </summary>
    public void OnViewEnter()
    {
        if (!isDoingEvent
           && Time.timeSinceLevelLoad - lastEventEndTime >= triggerCoolDown
           )
        {
            CheckAndExecuteEvent();
        }
    }

#if UNITY_EDITOR
#if DEBUG
    BoxCollider debugBox;
    SphereCollider debugSphere;
    bool debugGizmoCheck = false;
    private void OnDrawGizmos()
    {
        if (!debugGizmoCheck)
        {
            debugBox = GetComponent<BoxCollider>();
            debugSphere = GetComponent<SphereCollider>();
            debugGizmoCheck = true;
        }
        if (debugDrawRange)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = new Color(0.6f, 1.0f, 0.6f, 1.0f);
            if (debugBox)
            {
                Gizmos.DrawWireCube(debugBox.center, debugBox.size);
            }
            if (debugSphere)
            {
                Gizmos.DrawWireSphere(debugSphere.center, debugSphere.radius);
            }
        }
    }
#endif
#endif
}
