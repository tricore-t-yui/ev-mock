using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunAreaParameter : MonoBehaviour
{
    TunAreaData currentAreaData = null;

    [SerializeField]
    TunAreaDataManager dataManager = default;

    [SerializeField]
    PlayerHideController hideController = default;
    
    private void OnEnable()
    {
        currentAreaData = dataManager.GetTunAreaData(hideController.HideObj);
        if (currentAreaData == null) { return; }
        transform.position = currentAreaData.SpawnPos;
    }
}
