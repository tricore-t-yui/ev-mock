using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("イベントオブジェクト：指定オブジェクトのアニメーション再生。子以外をアニメーションさせる場合に使用。対象のAnimatorのチェックは事前にはずしておこう")]
public class EventPlayAnimation : EventObject
{
    [SerializeField, LabelText("再生するアニメーション")]
    Animator playAnim = default;

    [SerializeField, LabelText("セットするtriggerパラメータの名前")]
    string triggerName = default;

    void OnEnable()
    {
        playAnim.enabled = true;
        if(!string.IsNullOrEmpty(triggerName))
        {
            playAnim.SetTrigger(triggerName);
        }
        gameObject.SetActive(false);
        onEndCallback?.Invoke();
    }
}
