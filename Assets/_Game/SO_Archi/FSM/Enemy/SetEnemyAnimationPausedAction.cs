using UnityEngine;

[CreateAssetMenu(fileName = "SetEnemyAnimationPaused", menuName = "SO/FSM/Actions/SetEnemyAnimation")]
public class SetEnemyAnimationPausedAction : StateAction
{
    [SerializeField] private bool paused;

    public override void Act(StateController controller)
    {
        ((Enemy)controller).SetAnimationPaused(paused);
    }
}