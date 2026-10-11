using UnityEngine;

[CreateAssetMenu(fileName = "BeginEnemyRecovery", menuName = "SO/FSM/Enemy/Actions/BeginRecovery", order = -1)]
public class BeginEnemyRecoveryAction : StateAction
{
    public override void Act(StateController controller)
    {
        ((Enemy)controller).BeginRecoveryState();
    }
}
