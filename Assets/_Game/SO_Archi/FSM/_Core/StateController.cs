using System;
using UnityEngine;

public abstract class StateController : MonoBehaviour
{
    [Header("State Machine")] 
    [SerializeField] private State startState;
    [SerializeField] private State remainState;
    [SerializeField] private State previousState;
    [SerializeField] private State currentState;
    [SerializeField] private bool isActive = true;

    public State StartState => startState;
    public State RemainState => remainState;
    public State CurrentState => currentState;
    public bool IsActive => isActive;
    public bool TransitionStateChanged { get; private set; }
    public float StateTimeElapsed { get; private set; }

    protected virtual void OnEnable()
    {
        ResetToStartState();
    }

    protected virtual void Update()
    {
        if (!isActive || !currentState) return;
        StateTimeElapsed += Time.deltaTime;
        TransitionStateChanged = false;
        currentState.UpdateState(this);
    }

    public bool TransitionToState(State nextState)
    {
        if (!nextState || nextState == remainState || nextState == currentState) return false;

        currentState?.Exit(this);

        previousState = currentState;
        currentState = nextState;
        StateTimeElapsed = 0f;
        TransitionStateChanged = true;

        currentState.Enter(this);
        return true;
    }

    public void ResetToStartState()
    {
        currentState?.Exit(this);

        previousState = null;
        currentState = startState;
        StateTimeElapsed = 0f;
        TransitionStateChanged = false;
        
        currentState?.Enter(this);
    }

    public bool CheckIfCountdownElapsed(float duration)
    {
        return StateTimeElapsed >= duration;
    }

    public void DoEventTriggeredAction(ActionType type = ActionType.Default)
    {
        currentState?.DoEventTriggeredActions(this, type);
    }

    public void SetStateMachineActive(bool active)
    {
        isActive = active;
    }

    protected void OnDrawGizmos()
    {
        if (!currentState) return;
        Gizmos.color = currentState.SceneGizmoColor;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}