using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Signal", menuName = "Rogue Knight/Signal", order = 0)]
public class Signal : ScriptableObject {
    public List<SignalListener> listeners = new List<SignalListener>();

    public void Fire() {
        foreach (SignalListener s in listeners) {
            if (s)
                s.OnEvent();
        }
    }

    public void SubscribeListener(SignalListener listener) {
        listeners.Add(listener);
    }

    public void UnsubscribeListener(SignalListener listener) {
        listeners.Remove(listener);
    }
}