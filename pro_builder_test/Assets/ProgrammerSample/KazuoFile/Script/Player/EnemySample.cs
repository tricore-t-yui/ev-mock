using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySample : MonoBehaviour
{
    [SerializeField]
    HideStateController hideStateController = default;
    [SerializeField]
    CapsuleCollider capsuleCollider = default;

    void OnWillRenderObject()
    {
        if (Camera.current.name == "MoveCamera")
        {
            hideStateController.VisibleEnemy(gameObject.transform, capsuleCollider.height);
        }
    }
}
