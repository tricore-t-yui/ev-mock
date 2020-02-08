using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHideObjectAccesser : MonoBehaviour
{
    Dictionary<int, Animator> hideAnimator = new Dictionary<int, Animator>();

    [SerializeField]
    NavMeshAgent agent = default;

    PlayerHideController hideController = default;
    PlayerDamageEvent damageEvent = default;
    GameObject currentHide = default;
    Animator currentHideAnimator = default;
    bool isHide = false;
    int hideType = 0;
    bool isApproachHide = false;

    void Start()
    {
        // ハイドコントローラー
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        hideController = player.GetComponent<PlayerHideController>();

        damageEvent = FindObjectOfType<PlayerDamageEvent>() ?? damageEvent;

        // ロッカー
        foreach ( GameObject hide in GameObject.FindGameObjectsWithTag("Locker"))
        {
            Animator animator = hide.GetComponent<Animator>();
            hideAnimator.Add(hide.GetInstanceID(), animator);
        }

        // ベッド
        foreach (GameObject hide in GameObject.FindGameObjectsWithTag("Bed"))
        {
            Animator animator = hide.GetComponent<Animator>();
            hideAnimator.Add(hide.GetInstanceID(), animator);
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        currentHide = hideController.HideObj;
        isHide = (hideController.IsHideLocker || hideController.IsHideBed);
        hideType = (isHide ? (hideController.IsHideLocker ? 1 : 2) : 0);
    }

    public void OnCautionRangeStay(Collider other)
    {
        if (isHide)
        {
            isApproachHide = true;
        }
    }

    public void OnArrivaledHide(Collider other)
    {
        if (isHide)
        {
            isApproachHide = false;
            transform.LookAt(other.transform);
            currentHideAnimator = hideAnimator[currentHide.GetInstanceID()];
            if (hideType == 1)
            {
                currentHideAnimator.SetBool("DragOut", true);
                damageEvent.Invoke(transform, 50);
                currentHideAnimator.SetBool("DragOut", false);
            }
            else
            {
                damageEvent.Invoke(transform, 50);
            }
        }
    }
}
