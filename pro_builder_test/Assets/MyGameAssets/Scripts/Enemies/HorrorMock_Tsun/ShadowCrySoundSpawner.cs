using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[System.Serializable]
[TypeInfoBox("影人間は戦闘状態時に奇声を発します。\nここでは、奇声に見立てた円の当たり判定の設定を行います。\nツンにこの当たり判定があたると（奇声を聞くと）近寄ってきます。")]
public class CrySoundParameter
{
    [Tooltip("音に見立てたコライダーの拡大速度を設定します。")]
    public float soundExpansionSpeed = 1;
    [Tooltip("音に見立てたコライダーの縮小速度を設定します。")]
    public float soundReductionSpeed = 1;
    [Tooltip("音に見立てたコライダーの最大サイズを設定します。" +
        "\nこのサイズまで拡大して、そのあと縮小します。")]
    public float soundSizeMax = 1;
    [Tooltip("音に見立てたコライダーの出現間隔を設定します。" +
        "\nこの間隔でコライダーが出現します。")]
    public float soundSpawnInterval = 1;
}

public class ShadowCrySoundSpawner : MonoBehaviour
{
    [SerializeField]
    SpawnPool pool = default;
    [SerializeField]
    EnemyParameter parameter = default;
    [SerializeField]
    Transform spawnedParent = default;

    List<ShadowCrySoundController> soundControllers = new List<ShadowCrySoundController>();

    float spawnIntervalCount = 0;

    void Awake()
    {
        foreach (Transform child in transform)
        {
            soundControllers.Add(child.GetComponent<ShadowCrySoundController>());
        }
        soundControllers.ForEach(elem => elem.Initialize(parameter));
    }

    void OnEnable()
    {
        spawnIntervalCount = 0;
    }

    void Update()
    {
        spawnIntervalCount += Time.deltaTime;
        if (spawnIntervalCount >= parameter.CryParameter.soundSpawnInterval)
        {
            pool.Spawn("CrySoundCollider",transform.position,Quaternion.identity,spawnedParent);
            spawnIntervalCount = 0;
        }

        foreach (Transform child in spawnedParent)
        {
            if (!child.gameObject.activeSelf)
            {
                pool.Despawn(child,transform);
            }
        }
    }
}
