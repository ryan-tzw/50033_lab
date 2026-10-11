using UnityEngine;

[CreateAssetMenu(fileName = "BeginEnemyDeath", menuName = "SO/FSM/Actions/BeginEnemyDeath")]
public class BeginEnemyDeathAction : StateAction
{
    public override void Act(StateController controller)
    {
        ((Enemy)controller).BeginDeathState();
    }
}