using UnityEngine;

[CreateAssetMenu(fileName = "TickEnemyDeath", menuName = "SO/FSM/Actions/TickEnemyDeath")]
public class TickEnemyDeathAction : StateAction
{
    public override void Act(StateController controller)
    {
        ((Enemy)controller).TickDeathState();
    }
}