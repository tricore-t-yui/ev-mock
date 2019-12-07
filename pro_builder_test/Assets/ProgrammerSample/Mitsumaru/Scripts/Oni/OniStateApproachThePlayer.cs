using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OniStateApproachThePlayer : StateMachineBehaviour
{
    enum PlayerConditionKind
    {
        Normal,
        HideInLocker,
        HideInBed,
        Num,
    }

    // 現在のプレイヤーの態勢
    PlayerConditionKind currentPlayerCondition = PlayerConditionKind.Normal;

    [SerializeField]
    float[] attackPossibleDistance = new float[(int)PlayerConditionKind.Num];
    bool isAttackPossibleDistance = false;

    GameObject player = null;

    // ナビメッシュ
    NavMeshAgent navMesh = null;

    // プレイヤーのハイドコントローラー
    PlayerHideController playerHideController = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player") ?? player;

        // ナビメッシュの取得
        navMesh = animator.GetComponent<NavMeshAgent>() ?? navMesh;
        navMesh.SetDestination(player.transform.position);

        // プレイヤーのハイドコントローラーを取得
        playerHideController = FindObjectOfType<PlayerHideController>() ?? playerHideController;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        navMesh.speed = animator.GetFloat("moveSpeed");
        navMesh.SetDestination(player.transform.position);
        if (animator.GetBool("isPlayerHide"))
        {
            if (playerHideController.IsHideLocker)
            {
                currentPlayerCondition = PlayerConditionKind.HideInLocker;
            }
            else if (playerHideController.IsHideBed)
            {
                currentPlayerCondition = PlayerConditionKind.HideInBed;
            }
        }
        else
        {
            currentPlayerCondition = PlayerConditionKind.Normal;
        }

        float distance = (player.transform.position - animator.transform.position).magnitude;
        
        if (distance < attackPossibleDistance[(int)currentPlayerCondition])
        {
            if (!isAttackPossibleDistance)
            {
                animator.SetTrigger("attackStart");
                animator.SetInteger("playerConditionKindId", (int)currentPlayerCondition);
                isAttackPossibleDistance = true;
            }
        }
        else
        {
            isAttackPossibleDistance = false;
        }
    }
}
