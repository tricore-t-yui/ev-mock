using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    [SerializeField]
    Animator[] texts = default;

    int nowNum = 0;
    bool isEnd = false;

    /// <summary>
    /// 起動処理
    /// </summary>
    void OnEnable()
    {
        nowNum = 0;
        texts[nowNum].gameObject.transform.parent.gameObject.SetActive(true);
        isEnd = false;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isEnd)
        {
            texts[nowNum].SetTrigger("Hide");
            nowNum++;
            texts[nowNum].gameObject.transform.parent.gameObject.SetActive(true);
        }

        if(Input.GetMouseButtonDown(1) && nowNum > 0)
        {
            texts[nowNum].SetTrigger("Hide");
            nowNum--;
            texts[nowNum].gameObject.transform.parent.gameObject.SetActive(true);
        }

        if (nowNum >= texts.Length - 1)
        {
            isEnd = true;
        }
        else
        {
            isEnd = false;
        }
    }
}
