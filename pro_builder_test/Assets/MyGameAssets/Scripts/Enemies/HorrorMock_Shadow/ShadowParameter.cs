using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowParameter : MonoBehaviour
{
    public enum StateType
    {
        Normal,
        Alert,
        Caution,
        Fighting,
        Num,
    }
    [SerializeField]
    float normalMoveSpeed = 1;

    [SerializeField]
    float cautionMoveSpeed = 1.5f;

    [SerializeField]
    float fightingMoveSpeed = 2;

    [Space(15)]

    [SerializeField]
    float cautionRangeRadius = 1;

    [SerializeField]
    float fightingRangeRadius = 0.5f;

    [SerializeField]
    float attackDistance = 0.3f;

    [Space(15)]

    [SerializeField]
    float alertTimeOfSecond = 1;

    [Space(15)]

    [SerializeField]
    bool isAutoSpawn = true;
}
