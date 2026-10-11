using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRecoveryFinished", menuName = "SO/FSM/Decisions/RecoveryFinished", order = 0)]
public class EnemyRecoveryFinishedDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        var enemy = (Enemy)controller;
        return enemy.CheckIfCountdownElapsed(enemy.RecoveryDuration);
    }
}