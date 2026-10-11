using UnityEngine;

[CreateAssetMenu(menuName="SO/FSM/Actions/Set Movement")]
public class SetEnemyMovementAction : StateAction
{
    [SerializeField] private bool moveEnabled;

    public override void Act(StateController controller)
    {
        var enemy = (Enemy)controller;
        enemy.GetComponent<EnemyMovement>().SetMovementEnabled(moveEnabled);
    }
    
}