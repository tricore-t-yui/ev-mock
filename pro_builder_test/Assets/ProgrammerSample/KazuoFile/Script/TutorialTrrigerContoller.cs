using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チュートリアル用トリガークラス
/// </summary>
public class TutorialTrrigerContoller : MonoBehaviour
{
    public bool IsHit { get; private set; } = false; // ヒットフラグ

    /// <summary>
    /// トリガーが当たったら
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            IsHit = true;
        }
    }

    /// <summary>
    /// トリガーから離れたら
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            IsHit = false;
        }
    }
}
