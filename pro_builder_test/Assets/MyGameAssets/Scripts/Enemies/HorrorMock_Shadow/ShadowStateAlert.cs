using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowState = ShadowParameter.StateType;

public class ShadowStateAlert : ShadowStateBase
{
    SoundAreaSpawner soundSpawner = default;
    float alertTimeCounter = 0;
    float currentSoundLevel = 0;
    float appearFadeCounter = 0;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="sound"></param>
    public ShadowStateAlert(SoundAreaSpawner sound)
    {
        soundSpawner = sound;
    }

    public override void Entry()
    {
        appearFadeCounter = 0;
        alertTimeCounter = parameter.AlertTime;
        meshRenderer.enabled = true;
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        
    }
}
