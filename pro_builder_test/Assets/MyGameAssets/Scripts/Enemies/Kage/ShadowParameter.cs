using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShadowParameter : MonoBehaviour
{
    public enum WaitingType
    {
        Stand,
        HeadKnock,
        Sit,
    }

    [SerializeField]
    Animator animator = default;

    [SerializeField]
    [Tooltip("自動でスポーンを行うかどうか。\nオフの場合は自動で出現しなくなります。")]
    bool isAutoSpawn = true;

    [SerializeField]
    [Tooltip("スポーン時のスピードを設定します。")]
    float spawnSpeed = 0;

    [SerializeField]
    [Tooltip("デスポーン時のスピードを設定します。")]
    float despawnSpeed = 0;

    [SerializeField]
    WaitingType waiting = WaitingType.Stand;
    public WaitingType Waiting => waiting;

    void Awake()
    {
        if (isAutoSpawn)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        animator.SetFloat("SpawnSpeed", spawnSpeed);
        animator.SetFloat("DespawnSpeed", despawnSpeed);
        animator.SetInteger("WaitingTypeId", (int)waiting);
    }


}
