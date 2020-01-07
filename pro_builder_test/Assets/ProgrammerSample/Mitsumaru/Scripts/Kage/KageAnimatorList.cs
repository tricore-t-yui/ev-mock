using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 影人間のアニメーターリスト
/// </summary>
public class KageAnimatorList : MonoBehaviour
{
    public Dictionary<int, Animator> Animators { get; private set; } = new Dictionary<int, Animator>();

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        GameObject[] kages = GameObject.FindGameObjectsWithTag("Kage");
        Animators = kages.ToDictionary(kageKey => kageKey.gameObject.GetInstanceID(), kageValue => kageValue.GetComponent<Animator>());
    }
}
