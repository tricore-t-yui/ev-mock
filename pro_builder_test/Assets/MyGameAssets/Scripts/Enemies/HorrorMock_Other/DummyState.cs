using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyState : StateBase
{
    public override void Entry()
    {
        Debug.LogError("Error");
    }
}
