using UnityEngine;

[CreateAssetMenu(fileName = "BeginPlayerDeath", menuName = "SO/FSM/Player/Actions/BeginDeath", order = -1)]
public class BeginPlayerDeathAction : StateAction
{
    public override void Act(StateController controller)
    {
        ((Player)controller).BeginDeathState();
    }
}