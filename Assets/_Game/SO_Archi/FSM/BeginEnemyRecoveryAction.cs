using UnityEngine;

[CreateAssetMenu(menuName = "SO/FSM/Actions/Begin Recovery")]
public class BeginEnemyRecoveryAction : StateAction
{
    public override void Act(StateController controller)
    {
        ((Enemy)controller).BeginRecoveryState();
    }
}