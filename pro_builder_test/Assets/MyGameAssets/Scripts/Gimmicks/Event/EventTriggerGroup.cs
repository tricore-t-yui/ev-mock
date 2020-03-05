using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("複数のイベントをグループ化し、管理する。実行中は有効なスイッチリストが表示されます")]
public class EventTriggerGroup : MonoBehaviour
{
    [LabelText("有効なスイッチリスト"), ReadOnly]
    public List<string> EnabledSwitchList = new List<string>();

    List<EventTrigger> eventTriggerList = new List<EventTrigger>();

    // Start is called before the first frame update
    void Start()
    {
        // 見た目オフになってるやつは拾わない
        foreach (var item in GetComponentsInChildren<EventTrigger>())
        {
            eventTriggerList.Add(item);
        }
    }

    /// <summary>
    /// 発動した
    /// </summary>
    public void OnTriggered(EventTrigger eventTrigger)
    {
        // スイッチ持ってたら有効化スイッチをON
        if (!string.IsNullOrEmpty(eventTrigger.EnableSwitchName) && !EnabledSwitchList.Contains(eventTrigger.EnableSwitchName))
        {
            EnabledSwitchList.Add(eventTrigger.EnableSwitchName);
        }
    }

    /// <summary>
    /// トリガー死んだ
    /// </summary>
    public void OnTriggerDestroy(EventTrigger eventTrigger)
    {
        eventTriggerList.Remove(eventTrigger);
    }
}
