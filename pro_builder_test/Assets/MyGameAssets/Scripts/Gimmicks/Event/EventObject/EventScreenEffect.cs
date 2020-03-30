using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[TypeInfoBox("イベントオブジェクト：画像表示。フェード、シェイク、表示時間操作等の多機能。音を再生させたい場合は子にAudioSourceを置けばいい。\n" +
"表示する画像の見た目やサイズは、子にあるImageで調整する")]
public class EventScreenEffect : EventObject
{
    [SerializeField, LabelText("表示時間")]
    float showTime = 2.0f;

    [SerializeField, Range(0, 100), LabelText("画面シェイクの強さ")]
    int shakeRate = 0;

    [SerializeField, Range(0, 1), LabelText("フェードの補完数値。1に近づくほど速い")]
    float fadeSpeedRate = 0.5f;

    Image image;
    CanvasGroup group;

    void OnEnable()
    {
        // 真下にCanvasがいる前提
        // HACK: yui-t mockじゃないときはスプライト指定が必要
        image = GetComponentInChildren<Image>(true);
        group = GetComponentInChildren<CanvasGroup>(true);
        if(gameObject.activeInHierarchy)
        {
            if (shakeRate > 0)
                StartCoroutine(DoShake());
            StartCoroutine(PlayInternal());
        }
    }

    IEnumerator DoShake()
    {
        var pos = image.rectTransform.position;

        while (true)
        {
            var magnitude = shakeRate * 0.25f;
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            image.rectTransform.position = new Vector3(x, y, pos.z);
            yield return null;
        }
    }

    IEnumerator PlayInternal()
    {
        group.alpha = 0.0f;
        while (group.alpha < 0.999f)
        {
            group.alpha = Mathf.Lerp(group.alpha, 1.0f, fadeSpeedRate);
            yield return null;
        }
        group.alpha = 1.0f;

        yield return new WaitForSeconds(showTime);

        while (group.alpha > 0.001f)
        {
            group.alpha = Mathf.Lerp(group.alpha, 0.0f, fadeSpeedRate);
            yield return null;
        }
        group.alpha = 0.0f;

        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
        onEndCallback?.Invoke();
    }
}
