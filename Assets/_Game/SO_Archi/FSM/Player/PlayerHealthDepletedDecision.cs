using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealthDepleted", menuName = "SO/FSM/Player/Decisions/HealthDepleted", order = -1)]
public class PlayerHealthDepletedDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return ((Player)controller).IsHealthDepleted;
    }
}