using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TunAreaDataManager : MonoBehaviour
{
    Dictionary<int, TunAreaData> tunAreaDataTable = new Dictionary<int, TunAreaData>();

    [SerializeField]
    PlayerHideController playerHideController = default;
    [SerializeField]
    GameObject tun = default;

    private void Awake()
    {
        foreach(TunAreaData areaData in GetComponentsInChildren<TunAreaData>())
        {
            foreach(GameObject hideObject in areaData.HideObject)
            {
                tunAreaDataTable.Add(hideObject.GetInstanceID(), areaData);
            }
        }
    }

    public TunAreaData GetTunAreaData(GameObject hideObject)
    {
        if (hideObject == null) { return null; }
        return tunAreaDataTable[hideObject.GetInstanceID()];
    }

    private void Update()
    {
        if (playerHideController.enabled)
        {
            tun.SetActive(true);
        }
    }
}
