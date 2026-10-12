using System.Collections.Generic;
using UnityEngine;

public class GameEvent<T> : ScriptableObject
{
    private readonly List<GameEventListener<T>> _listeners = new();

    public virtual void Raise(T data)
    {
        for (var i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(data);
        }
    }

    public void RegisterListener(GameEventListener<T> listener)
    {
        if (!_listeners.Contains(listener)) _listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener<T> listener)
    {
        _listeners.Remove(listener);
    }
}

public class Void { }

[CreateAssetMenu(fileName = "GameEvent", menuName = "SO/GameEvent", order = -1)]
public class GameEvent : GameEvent<Void>
{
    public void Raise()
    {
        base.Raise(new Void());
    }
}
