using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHideObjectAccesser : MonoBehaviour
{
    public enum HideType
    {
        Locker,
        Bed,
    }

    HideType hideType = HideType.Locker;

    [SerializeField]
    Animator animator = default;
    [SerializeField]
    NavMeshAgent agent = default;
    [SerializeField]
    EnemyParameter parameter = default;

    GameObject player = default;

    PlayerDamageEvent damageEvent = default;

    [SerializeField]
    float[] attackRange = new float[System.Enum.GetNames(typeof(HideType)).Length];

    PlayerHideController hideController = default;
    Dictionary<int, Animator> hideAnimatorTable = new Dictionary<int, Animator>();

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        damageEvent = FindObjectOfType<PlayerDamageEvent>();

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
        if (hideController.HideObj == null) { return; }

        if (hideController.HideObj.tag == "Locker") { hideType = HideType.Locker; }
        else if (hideController.HideObj.tag == "Bed") { hideType = HideType.Bed; }

        // 警戒か戦闘のみ
        if (animator.GetInteger("AnimatorStateTypeId") != 0)
        {
            GameObject hide = hideController.HideObj;
            if ((hide.transform.position - transform.position).magnitude < attackRange[(int)hideType])
            {
                if (hide.tag == "Locker")
                {
                    Animator anim = hideAnimatorTable[hide.GetInstanceID()];
                    anim.SetBool("DragOut", true);
                    anim.SetBool("DragOut", false);
                }
                damageEvent.Invoke(transform, parameter.Damage);
            }
        }
    }
}
