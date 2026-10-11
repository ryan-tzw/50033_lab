using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRecoveryFinished", menuName = "SO/FSM/Enemy/Decisions/RecoveryFinished", order = -1)]
public class EnemyRecoveryFinishedDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        var enemy = (Enemy)controller;
        return enemy.CheckIfCountdownElapsed(enemy.RecoveryDuration);
    }
}
