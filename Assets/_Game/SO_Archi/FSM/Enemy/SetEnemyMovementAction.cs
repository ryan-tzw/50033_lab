using UnityEngine;

[CreateAssetMenu(fileName = "SetEnemyMovement", menuName = "SO/FSM/Enemy/Actions/SetMovement", order = -1)]
public class SetEnemyMovementAction : StateAction
{
    [SerializeField] private bool moveEnabled;

    public override void Act(StateController controller)
    {
        ((Enemy)controller).SetMovementEnabled(moveEnabled);
    }
    
}
