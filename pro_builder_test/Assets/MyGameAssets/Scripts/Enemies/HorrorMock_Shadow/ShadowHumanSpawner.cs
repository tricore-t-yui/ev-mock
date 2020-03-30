using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ShadowHumanSpawner : MonoBehaviour
{
    public enum Type
    {
        Spawn,
        Despawn,
    }

    [SerializeField]
    ShadowHuman shadowHuman = default;

    GameObject player = default;

    [SerializeField]
    [EnumToggleButtons]
    Type spawnerType = Type.Spawn;

    [SerializeField]
    float spawnTime = 1;
    float spawnTimeCounter = 0;

    bool isSpawn = false;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        if (isSpawn)
        {
            spawnTimeCounter += Time.deltaTime;
            if (spawnTimeCounter > spawnTime)
            {
                if (spawnerType == Type.Spawn)
                {
                    shadowHuman.Spawn(EnemyParameter.StateType.Fighting,player.transform.position);
                }
                else
                {
                    shadowHuman.gameObject.SetActive(false);
                }
                isSpawn = false;
                spawnTimeCounter = 0;
                transform.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isSpawn = true;
        }
    }
}
