using UnityEngine;

[CreateAssetMenu(fileName = "TickEnemyDeath", menuName = "SO/FSM/Enemy/Actions/TickDeath", order = -1)]
public class TickEnemyDeathAction : StateAction
{
    public override void Act(StateController controller)
    {
        ((Enemy)controller).TickDeathState();
    }
}
