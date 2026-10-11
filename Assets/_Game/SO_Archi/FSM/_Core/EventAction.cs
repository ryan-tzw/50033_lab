using System;
using UnityEngine;

public enum ActionType
{
    Default = -1,
    Attack = 0, 
}

[Serializable]
public struct EventAction
{
    [SerializeField] private ActionType type;
    [SerializeField] private StateAction action;
    
    public ActionType Type => type;
    public StateAction Action => action;
}