using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("イベントオブジェクト：指定オブジェクトのOn/Off。子以外のオブジェクトを操作する場合に使用")]
public class EventTargetOnOff : EventObject
{
    [SerializeField, LabelText("On/Offにするオブジェクト")]
    GameObject targetObject = default;

    [SerializeField, LabelText("on/off")]
    bool onoff = default;

    // 指定オブジェクトのOn/Off
    void OnEnable()
    {
        targetObject.SetActive(onoff);
        onEndCallback?.Invoke();
    }
}
