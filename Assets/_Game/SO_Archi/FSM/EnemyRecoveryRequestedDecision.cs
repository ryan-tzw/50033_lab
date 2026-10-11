using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRecoveryRequested", menuName = "SO/FSM/Decisions/RecoveryRequested", order = 0)]
public class EnemyRecoveryRequestedDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return ((Enemy)controller).RecoveryRequested;
    }
}