using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 影人間全体の管理クラス
/// </summary>
public class KageManager : MonoBehaviour
{
    [SerializeField]
    PlayerDamageController damageController = default;

    [SerializeField]
    KageStateParameter[] kageList = default;
    public KageStateParameter[] KageList => kageList;

    // 体のコライダー
    List<GameObject> bodyColliderObjects = new List<GameObject>();

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        foreach(KageStateParameter kage in kageList)
        {
            // コライダーのオブジェクトを取得
            bodyColliderObjects.Add(kage.transform.Find("Collider").Find("KageBody").gameObject);
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // 無敵中に影人間の当たり判定を消す
        if (damageController.IsInvincible)
        {
            bodyColliderObjects.ForEach(coll =>
            {
                coll.SetActive(false);
            });
        }
        else
        {
            bodyColliderObjects.ForEach(coll =>
            {
                coll.SetActive(true);
            });
        }
    }

    /// <summary>
    /// 全ての影人間の状態をリセットする
    /// </summary>
    /// <returns></returns>
    public void ResetAllKage()
    {
        foreach (KageStateParameter kage in kageList)
        {
            kage.gameObject.SetActive(false);
        }
        foreach (KageStateParameter kage in kageList)
        {
            if (kage.IsAutoSpawn)
            {
                kage.gameObject.SetActive(true);
            }
        }

    }
}
