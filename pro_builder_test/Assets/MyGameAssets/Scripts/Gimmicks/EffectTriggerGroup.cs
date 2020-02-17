using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EffectTriggerGroup
{
    static List<ScreenEffectTrigger> screenEffectList = new List<ScreenEffectTrigger>();
    static List<SoundTrigger> soundTriggerList = new List<SoundTrigger>();

    public static void Add(ScreenEffectTrigger add)
    {
        screenEffectList.Add(add);
    }
    public static void Add(SoundTrigger add)
    {
        soundTriggerList.Add(add);
    }
    public static void Remove(ScreenEffectTrigger remove)
    {
        if(screenEffectList.Contains(remove)) screenEffectList.Remove(remove);
    }
    public static void Remove(SoundTrigger remove)
    {
        if(soundTriggerList.Contains(remove)) soundTriggerList.Remove(remove);
    }
    public static void DestroyGroup(int group)
    {
        if (group != 0)
        {
            screenEffectList.Where(x => x.TriggerGroup == group).All(x => { GameObject.Destroy(x.gameObject); return true; });
            soundTriggerList.Where(x => x.TriggerGroup == group).All(x => { GameObject.Destroy(x.gameObject); return true; });
        }
    }
}