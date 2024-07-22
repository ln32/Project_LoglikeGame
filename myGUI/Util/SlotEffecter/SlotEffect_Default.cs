using System;
using UnityEngine;
using UnityEngine.Events;

public class SlotEffect_Default : MonoBehaviour
{
    [SerializeField] internal UnityEvent event_DEFAULT, event_ONFOCUS, event_ABLE, event_DISABLE, event_INFO;
}

[Serializable]
internal class ColorSet
{
    public Color DEFAULT, ONFOCUS, ABLE, DISABLE, INFO_FOCUS;
}