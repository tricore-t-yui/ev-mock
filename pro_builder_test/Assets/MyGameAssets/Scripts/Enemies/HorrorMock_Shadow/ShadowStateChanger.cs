using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowStateChanger : MonoBehaviour
{
    [SerializeField]
    Animator animator = default;
    Transform[] stateObjects = new Transform[(int)ShadowParameter.StateType.Num];
    public int CurrentStateTypeId { get; private set; } = 0;
    int prevStateTypeId = 0;

    void Start()
    {
        for (int i = 0; i < (int)ShadowParameter.StateType.Num; i++)
        {
            stateObjects[i] = transform.GetChild(i);
            stateObjects[i].gameObject.SetActive(false);
        }
        stateObjects[CurrentStateTypeId].gameObject.SetActive(true);
    }

    void Update()
    {
        CurrentStateTypeId = animator.GetInteger("StateTypeId");

        if (prevStateTypeId != CurrentStateTypeId)
        {
            System.Array.ForEach(stateObjects, state => state.gameObject.SetActive(false));
            stateObjects[CurrentStateTypeId].gameObject.SetActive(true);
        }
        prevStateTypeId = CurrentStateTypeId;
    }
}
