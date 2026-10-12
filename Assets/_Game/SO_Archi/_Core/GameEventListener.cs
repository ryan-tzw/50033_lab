using UnityEngine;
using UnityEngine.Events;

public class GameEventListener<T> : MonoBehaviour
{
    [SerializeField] private GameEvent<T> gameEvent;
    [SerializeField] private UnityEvent<T> response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(T data)
    {
        response.Invoke(data);
    }
}

public class GameEventListener : GameEventListener<Void> { }