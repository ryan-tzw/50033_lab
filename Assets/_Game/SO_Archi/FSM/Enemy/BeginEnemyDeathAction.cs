using UnityEngine;

[CreateAssetMenu(fileName = "BeginEnemyDeath", menuName = "SO/FSM/Enemy/Actions/BeginDeath", order = -1)]
public class BeginEnemyDeathAction : StateAction
{
    public override void Act(StateController controller)
    {
        ((Enemy)controller).BeginDeathState();
    }
}
