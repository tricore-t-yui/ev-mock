using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class ClipParameter
{
    public ClipParameter(string animName,string clipName)
    {
        animationName = animName;
        oldClipName = clipName;
    }

    bool isActivate = false;
    [EnableIf("isActivate")]
    public string animationName = null;
    [EnableIf("isActivate")]
    public string oldClipName = null;
    public AnimationClip newClip = null;
}

public class AnimationClipChanger : MonoBehaviour
{
    [SerializeField]
    AnimatorOverrideController overrideController = default;

    [SerializeField]
    ClipParameter[] clipParameters = 
    {
        new ClipParameter("戦闘待機","attackWaitLoop"),
    };

    void Start()
    {
        foreach(ClipParameter parameter in clipParameters)
        {
            if (parameter.newClip != null)
            {
                overrideController[parameter.oldClipName] = parameter.newClip;
            }
        }
    }
}
