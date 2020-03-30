using System;
using System.Collections;
using UnityEngine;

public abstract class EventObject : MonoBehaviour
{
    protected Action onEndCallback;
    public void SetEndCallback(Action _onEndCallback) { onEndCallback = _onEndCallback; }
}
