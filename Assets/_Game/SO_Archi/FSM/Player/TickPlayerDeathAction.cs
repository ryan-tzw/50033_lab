using UnityEngine;

[CreateAssetMenu(fileName = "TickPlayerDeath", menuName = "SO/FSM/Player/Actions/TickDeath", order = -1)]
public class TickPlayerDeathAction : StateAction
{
    public override void Act(StateController controller)
    {
        ((Player)controller).TickDeathState();
    }
}