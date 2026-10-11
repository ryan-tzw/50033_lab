using UnityEngine;

[CreateAssetMenu(fileName = "RaisePlayerDied", menuName = "SO/FSM/Player/Actions/RaisePlayerDied", order = -1)]
public class RaisePlayerDiedAction : StateAction
{
    public override void Act(StateController controller)
    {
        ((Player)controller).RaisePlayerDied();
    }
}