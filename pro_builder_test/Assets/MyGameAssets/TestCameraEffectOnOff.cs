using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Rewired;

public class TestCameraEffectOnOff : MonoBehaviour
{
    [SerializeField]
    PostProcessVolume postProcessVolume;

    int postProcessNum;
    int offIndex = -1;

    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        postProcessNum = postProcessVolume.profile.settings.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("PostProcessToggle"))
        {
            ++offIndex;
            if(offIndex >= postProcessNum)
            {
                offIndex = -1;
            }
            //postProcessVolume.enabled = !postProcessVolume.enabled;
            int count = 0;
            foreach (var item in postProcessVolume.profile.settings)
            {
                bool flag = count != offIndex;
                item.SetAllOverridesTo(flag);
                ++count;
            }
            Debug.Log("postProcessNum:" + postProcessNum + " offIndex:" + offIndex);
        }
    }
}
