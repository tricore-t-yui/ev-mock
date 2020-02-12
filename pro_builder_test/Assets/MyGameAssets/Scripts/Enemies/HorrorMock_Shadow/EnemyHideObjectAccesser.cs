using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHideObjectAccesser : MonoBehaviour
{
    [SerializeField]
    Animator animator = default;
    [SerializeField]
    NavMeshAgent agent = default;

    GameObject player = default;

    [SerializeField]
    float lockerAttackRange = 1;
    [SerializeField]
    float bedAttackRange = 1;

    PlayerHideController hideController = default;
    Dictionary<int, Animator> hideAnimatorTable = new Dictionary<int, Animator>();

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        // ハイドコントローラーを取得
        hideController = GameObject.FindObjectOfType<PlayerHideController>();
        // プレイヤー
        player = GameObject.FindGameObjectWithTag("Player");

        // ロッカー
        foreach (GameObject hide in GameObject.FindGameObjectsWithTag("Locker"))
        {
            Animator anim = hide.GetComponent<Animator>();
            hideAnimatorTable.Add(hide.GetInstanceID(), anim);
        }

        // ベッド
        foreach (GameObject hide in GameObject.FindGameObjectsWithTag("Bed"))
        {
            Animator anim = hide.GetComponent<Animator>();
            hideAnimatorTable.Add(hide.GetInstanceID(), anim);
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        
    }
}
