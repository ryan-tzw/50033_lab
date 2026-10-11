using UnityEngine;

[CreateAssetMenu(menuName="SO/FSM/Actions/Set Movement")]
public class SetEnemyMovementAction : StateAction
{
    [SerializeField] private bool moveEnabled;

    public override void Act(StateController controller)
    {
        ((Enemy)controller).SetMovementEnabled(moveEnabled);
    }
    
}