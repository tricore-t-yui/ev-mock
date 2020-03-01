using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventBridge : MonoBehaviour
{
    EnemyBase enemy;
    private void Start()
    {
        enemy = transform.parent.GetComponent<EnemyBase>();
    }

    public void AttackPlayerByAnimation()
    {
        enemy.AttackPlayerByAnimation();
    }
}
