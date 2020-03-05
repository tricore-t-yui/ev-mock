using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("イベントオブジェクト：指定オブジェクトのアニメーション再生。子以外をアニメーションさせる場合に使用")]
public class EventPlayAnimation : EventObject
{
    [SerializeField, LabelText("再生するアニメーション")]
    Animator playAnim = default;

    void OnEnable()
    {
        playAnim.enabled = true;
        onEndCallback?.Invoke();
    }
}
