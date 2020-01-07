using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class KageSympathizeRangeCollider : MonoBehaviour
{
    KageAnimatorList kageAnimatorList = default;
    [SerializeField]
    Animator animator = default;

    HashSet<Animator> higherPriorityStateKageAnimators = new HashSet<Animator>();

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        kageAnimatorList = FindObjectOfType<KageAnimatorList>();
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        Debug.Log(higherPriorityStateKageAnimators.Count);
        if (higherPriorityStateKageAnimators.Count(elem => elem.GetInteger("currentStateLevel") > animator.GetInteger("currentStateLevel")) > 0)
        {
            animator.SetBool("isNearHigherPriorityStateKage", true);
        }
        else
        {
            animator.SetBool("isNearHigherPriorityStateKage", false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Kage")
        {
            Animator kageAnimator = kageAnimatorList.Animators[other.transform.parent.parent.gameObject.GetInstanceID()];
            higherPriorityStateKageAnimators.Add(kageAnimator);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Kage")
        {
            Animator kageAnimator = kageAnimatorList.Animators[other.transform.parent.parent.gameObject.GetInstanceID()];
            higherPriorityStateKageAnimators.Remove(kageAnimator);
        }
    }
}
