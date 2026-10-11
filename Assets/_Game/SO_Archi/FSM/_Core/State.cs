using System;
using UnityEngine;

[CreateAssetMenu(menuName="SO/FSM/State")]
public class State : ScriptableObject
{
    [SerializeField] private StateAction[] setupActions          = Array.Empty<StateAction>();
    [SerializeField] private StateAction[] actions               = Array.Empty<StateAction>();
    [SerializeField] private EventAction[] eventTriggeredActions = Array.Empty<EventAction>();
    [SerializeField] private StateAction[] exitActions           = Array.Empty<StateAction>();
    [SerializeField] private Transition[]  transitions           = Array.Empty<Transition>();
    [SerializeField] private Color sceneGizmoColor;
    public Color SceneGizmoColor => sceneGizmoColor;

    public void Enter(StateController controller)
    {
        DoActions(setupActions, controller);
    }

    public void UpdateState(StateController controller)
    {
        DoActions(actions, controller);
        CheckTransitions(controller);
    }

    public void Exit(StateController controller)
    {
        DoActions(exitActions, controller);
    }

    public void DoEventTriggeredActions(StateController controller, ActionType type = ActionType.Default)
    {
        foreach (var eventAction in eventTriggeredActions)
        {
            if (eventAction.Type == type && eventAction.Action)
            {
                eventAction.Action.Act(controller);
            }
        }
    }

    private static void DoActions(StateAction[] actions, StateController controller)
    {
        foreach (var action in actions)
        {
            if (action)
            {
                action.Act(controller);
            }
        }
    }

    private void CheckTransitions(StateController controller)
    {
        if (controller.TransitionStateChanged) return;

        foreach (var transition in transitions)
        {
            if (!transition.Decision) continue;

            var nextState = transition.Decision.Decide(controller) ? transition.TrueState : transition.FalseState;

            if (controller.TransitionToState(nextState))
            {
                break;
            }
        }
    }
}