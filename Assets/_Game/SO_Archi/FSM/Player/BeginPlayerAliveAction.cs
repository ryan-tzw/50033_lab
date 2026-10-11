using UnityEngine;

[CreateAssetMenu(fileName = "BeginPlayerAlive", menuName = "SO/FSM/Player/Actions/BeginPlayerAlive", order = -1)]
public class BeginPlayerAliveAction : StateAction
{
    public override void Act(StateController controller)
    {
        ((Player)controller).BeginAliveState();
    }
}
