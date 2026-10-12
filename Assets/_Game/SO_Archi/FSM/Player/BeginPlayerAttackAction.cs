using UnityEngine;

[CreateAssetMenu(fileName = "BeginPlayerAttack", menuName = "SO/FSM/Player/Actions/BeginAttack", order = -1)]
public class BeginPlayerAttackAction : StateAction
{
    public override void Act(StateController controller)
    {
        ((Player)controller).BeginAttackState();
    }
}