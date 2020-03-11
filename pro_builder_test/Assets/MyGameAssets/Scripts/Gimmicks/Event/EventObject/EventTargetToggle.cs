using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("イベントオブジェクト：指定オブジェクトの中からランダムでひとつONにする")]
public class EventTargetToggle : EventObject
{
    [SerializeField, LabelText("トグルするオブジェクトリスト")]
    List<GameObject> targetObjectList = default;

    // 指定オブジェクトのOn/Off
    void OnEnable()
    {
        foreach (var item in targetObjectList)
        {
            item.SetActive(false);
        }
        var idx = Random.Range(0, targetObjectList.Count * 100 - 1) / 100;
        targetObjectList[idx].SetActive(true);

        gameObject.SetActive(false);
        onEndCallback?.Invoke();
    }
}
