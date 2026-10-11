using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDeathRequested", menuName = "SO/FSM/Decisions/EnemyDeathRequested")]
public class EnemyDeathRequestedDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return ((Enemy)controller).IsHealthDepleted;
    }
}