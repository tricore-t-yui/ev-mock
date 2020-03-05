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
        [LabelText("(未実装) 視線当たり判定")]
        ViewCollide,
        [LabelText("時間")]
        Time
    }
    [SerializeField, LabelText("発動種別")]
    TriggerKind kind = TriggerKind.Collide;

    [SerializeField, LabelText("発動に必要なスイッチ名")]
    string needSwitchName = "";

    [SerializeField, LabelText("発動できなくなるスイッチ名(マウスオーバーで詳細"), Tooltip("ここに記入されているスイッチがオンだと発動しなくなる。" +
    "\n例：Soundと記入すると、他のトリガーによってSoundスイッチがONになった場合はこのトリガーが発動しなくなる")]
    string banSwitch = "";

    [SerializeField, LabelText("発動でONになるスイッチ名")]
    string enableSwitchName = "";
    public string EnableSwitchName => enableSwitchName;

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
        if (string.IsNullOrEmpty(needSwitchName) || group.EnabledSwitchList.Contains(needSwitchName))
        {
            if (string.IsNullOrEmpty(banSwitch) || !group.EnabledSwitchList.Contains(banSwitch))
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
            triggerObject.gameObject.SetActive(true);
            isDoingEvent = true;

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
        && kind == TriggerKind.Collide
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
        // TODO: 実装
    }
}
