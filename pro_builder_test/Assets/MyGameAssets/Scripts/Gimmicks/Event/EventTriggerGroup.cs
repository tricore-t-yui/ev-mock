using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("複数のイベントをグループ化し、管理する。実行中は有効なスイッチリストが表示されます")]
public class EventTriggerGroup : MonoBehaviour
{
    [SerializeField, LabelText("有効なスイッチリスト"), ReadOnly]
    List<string> EnabledSwitchList = new List<string>();

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
        if (eventTrigger.EnableSwitchNameList.Length != 0)
        {
            foreach (var item in eventTrigger.EnableSwitchNameList)
            {
                if(!EnabledSwitchList.Contains(item)) EnabledSwitchList.Add(item);
            }
        }
        // 無効にするスイッチ持っていたら無効化
        foreach (var item in eventTrigger.DisableSwitchNameList)
        {
            EnabledSwitchList.Remove(item);
        }
    }

    /// <summary>
    /// トリガー死んだ
    /// </summary>
    public void OnTriggerDestroy(EventTrigger eventTrigger)
    {
        eventTriggerList.Remove(eventTrigger);
    }

    /// <summary>
    /// スイッチが一致するかどうか
    /// </summary>
    public bool IsSwitchOn(string[] switchStringList)
    {
        foreach (var switchString in switchStringList)
        {
            if (EnabledSwitchList.Contains(switchString))
                return true;
        }
        return false;
    }
}
