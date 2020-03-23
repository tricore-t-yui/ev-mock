using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStageClear : MonoBehaviour
{
    public GameObject clearObject;
    public List<GameObject> goalOffList;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        var onis = Object.FindObjectsOfType(typeof(Oni));
        bool clear = false;
        foreach (var item in onis)
        {
            var gameObj = (Oni)item;
            if(gameObj != null)
            {
                gameObj.gameObject.SetActive(false);
                clear = true;
            }
        }
        if(clear)
        {
            foreach (var item in goalOffList)
            {
                item.SetActive(false);
            }
            clearObject.SetActive(true);
        }
    }
}
