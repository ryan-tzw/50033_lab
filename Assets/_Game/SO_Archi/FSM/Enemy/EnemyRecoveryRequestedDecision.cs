using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRecoveryRequested", menuName = "SO/FSM/Enemy/Decisions/RecoveryRequested", order = -1)]
public class EnemyRecoveryRequestedDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return ((Enemy)controller).RecoveryRequested;
    }
}
