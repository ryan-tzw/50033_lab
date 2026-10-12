using UnityEngine;

[CreateAssetMenu(fileName = "PlayerIsAttacking", menuName = "SO/FSM/Player/Decisions/IsAttacking", order = -1)]
public class PlayerIsAttackingDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return ((Player)controller).IsAttacking;
    }
}