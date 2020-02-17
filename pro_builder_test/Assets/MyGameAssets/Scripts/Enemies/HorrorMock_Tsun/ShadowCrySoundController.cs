using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class ShadowCrySoundController : MonoBehaviour
{
    [SerializeField]
    SphereCollider soundCollider = default;

    EnemyParameter parameter = default;

    public enum SoundState
    {
        Expansion,
        Reduction,
    }

    public void Initialize(EnemyParameter parameter)
    {
        this.parameter = parameter;
    }

    SoundState state = SoundState.Expansion;

    void Update()
    {
        switch (state)
        {
            case SoundState.Expansion:
                soundCollider.radius += parameter.CryParameter.soundExpansionSpeed;
                if (soundCollider.radius >= parameter.CryParameter.soundSizeMax)
                {
                    state = SoundState.Reduction;
                }
                break;
            case SoundState.Reduction:
                soundCollider.radius -= parameter.CryParameter.soundReductionSpeed;
                if (soundCollider.radius <= 0.001f)
                {
                    state = SoundState.Expansion;
                    gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
}
