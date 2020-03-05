using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("イベントオブジェクト：通常（子のゲームオブジェクトが自動で何かする。通常はこちらを使用）")]
public class EventNormal : EventObject
{
    [SerializeField, Range(0.1f, 300), LabelText("生きている時間。")]
    float lifeTime = 2.0f;
    public void OnEnable()
    {
        StartCoroutine(SelfDeleteInternal());
    }

    IEnumerator SelfDeleteInternal()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
        onEndCallback?.Invoke();
    }
}
