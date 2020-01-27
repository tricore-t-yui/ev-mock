using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 親オブジェクトの親に影響されない
/// </summary>
public class AsyncParentPosition : MonoBehaviour
{
    Vector3 initPos = Vector3.zero;

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        initPos = transform.position;
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        transform.position = initPos;
    }
}
