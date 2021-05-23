using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{

    public Signal signal;
    public UnityEvent signalEvent;
    public void OnEvent() {
        signalEvent.Invoke();
    }

    private void OnEnable() {
        signal.SubscribeListener(this);
    }

    private void OnDisable() {
        signal.UnsubscribeListener(this);
    }
}
