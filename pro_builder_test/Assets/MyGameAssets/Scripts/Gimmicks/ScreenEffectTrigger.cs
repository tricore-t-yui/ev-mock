using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// トリガーエフェクト表示のトリガー
/// </summary>
public class ScreenEffectTrigger : MonoBehaviour
{
    [SerializeField, Tooltip("フラグがオンなら発動チャンスが一度きり")]
    bool isOneChance = false;

    [SerializeField, Range(1, 100), Tooltip("イベントの出現確率")]
    int appearRate = 100;

    [SerializeField, Tooltip("表示時間")]
    float showTime = 2.0f;

    [SerializeField, Range(0, 100), Tooltip("画面シェイクの強さ")]
    int shakeRate = 0;

    [SerializeField, Tooltip("表示する画像のリスト。複数あった場合はランダム再生")]
    List<Sprite> spriteList;

    [SerializeField, Tooltip("表示と同時に鳴らすサウンド。なければ鳴らさない")]
    AudioClip clip;

    Image image;
    CanvasGroup group;
    AudioSource source;

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        group = GetComponentInChildren<CanvasGroup>();
        source = GetComponentInChildren<AudioSource>();
    }

    /// <summary>
    /// トリガーに触れたとき
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        var rate = Random.Range(0.0f, 1.0f);
        Debug.Log("Enter rate:" + rate);
        if (rate > 1.0f - (appearRate / 100.0f))
        {
            if (shakeRate > 0)
                StartCoroutine(DoShake());
            StartCoroutine(PlayInternal());
        }
        else if (isOneChance)
        {
            Destroy(gameObject);
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
        if (source && clip)
        {
            source.clip = clip;
            source.Play();
        }

        if (spriteList.Count == 1)
        {
            image.sprite = spriteList[0];
        }
        else
        {
            var idx = Random.Range(0, spriteList.Count * 100 - 1) / 100;
            //Debug.Log("idx:" + idx);
            image.sprite = spriteList[idx];
        }

        group.alpha = 0.0f;
        while (group.alpha < 0.999f)
        {
            group.alpha = Mathf.Lerp(group.alpha, 1.0f, 0.5f);
            yield return null;
        }
        group.alpha = 1.0f;

        yield return new WaitForSeconds(showTime);

        while (group.alpha > 0.001f)
        {
            group.alpha = Mathf.Lerp(group.alpha, 0.0f, 0.5f);
            yield return null;
        }
        group.alpha = 0.0f;

        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
