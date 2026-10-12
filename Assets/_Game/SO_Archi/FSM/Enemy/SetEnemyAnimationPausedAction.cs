using UnityEngine;

[CreateAssetMenu(fileName = "SetAnimationPaused", menuName = "SO/FSM/Enemy/Actions/SetAnimation", order = -1)]
public class SetEnemyAnimationPausedAction : StateAction
{
    [SerializeField] private bool paused;

    public override void Act(StateController controller)
    {
        ((Enemy)controller).SetAnimationPaused(paused);
    }
}
