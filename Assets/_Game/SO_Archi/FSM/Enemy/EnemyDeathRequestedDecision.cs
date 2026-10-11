using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDeathRequested", menuName = "SO/FSM/Enemy/Decisions/DeathRequested", order = -1)]
public class EnemyDeathRequestedDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return ((Enemy)controller).IsHealthDepleted;
    }
}
